using System.Collections.Generic;
using System.Linq;
using NuclearGames.Physics_LE.Colliders.Interfaces;
using NuclearGames.Physics_LE.Utils.Extensions;
using NuclearGames.Physics_LE.Utils.Mathematics;
using UnityEngine;

namespace NuclearGames.Physics_LE.Bodies {
    
    /// <summary>
    /// Упрощенный вариант Rigidbody
    /// </summary>
    public class SimpleRigidbody {

#region Public

#region Fields

        /// <summary>
        /// Блокировка линейного движения по любой из осей
        /// </summary>
        public Vector3 LinearLockAxisFactors;
        
        /// <summary>
        /// Блокировка вращения по любой из осей
        /// </summary>
        public Vector3 AngularLockAxisFactors;

#endregion

#region Properties

        /// <summary>
        /// Масса компонента
        /// </summary>
        public float Mass {
            get => _mass;
            set {
                lock (_tickSynchronization) {
                    _mass = value;
                    _inverseMass = Mathf.Approximately(_mass, 0) ? 0 : 1 / _mass;
                    UpdateLocalInertiaTensorFromColliders();
                }
            }
        }

        /// <summary>
        /// Локальная точка, в которой расположен центр масс
        /// </summary>
        public Vector3 LocalCenterOfMass {
            get => _localCenterOfMass;
            set {
                lock (_tickSynchronization) {
                    _localCenterOfMass = value;
                    UpdateLocalInertiaTensorFromColliders();
                }
            }
        }
        
        public Vector3 GlobalCenterOfMass => _transform.TransformPoint(LocalCenterOfMass);

        /// <summary>
        /// Линейная скорость
        /// </summary>
        public Vector3 LinearVelocity {
            get => _linearVelocity;
            set => _linearVelocity = value; 
        }

        /// <summary>
        /// Угловая скорость
        /// </summary>
        public Vector3 AngularVelocity {
            get => _angularVelocity;
            set => _angularVelocity = value;
        }

        public Transform Transform => _transform;

#endregion

#region Constructors

        public SimpleRigidbody(Transform transform) {
            _transform = transform;
            _attachedColliders = new List<ISimpleCollider>();
        }
        
        public SimpleRigidbody(Transform transform, IEnumerable<ISimpleCollider> colliders) {
            _transform = transform;
            _attachedColliders = new List<ISimpleCollider>(colliders);
        }

#endregion


#region Functions
        
        /// <summary>
        /// Применяет локальный вектор силы к центру масс тела.
        /// <para>Вектор будет преображен в глобальную систему координат по средствам метода <see cref="Transform.TransformVector"/> (с учетом <see cref="Transform.Scale"/></para>
        /// <param name="localForce">Локальный вектор силы, применимый к телу</param>
        /// </summary>
        public void AddLocalForce(in Vector3 localForce) {
            var worldForce = _transform.TransformVector(localForce);
            AddForce(in worldForce);
        }

        /// <summary>
        /// Применяет глобальный вектор силы к центру масс тела
        /// <param name="force">Глобальный вектор силы, применимый к телу</param>
        /// </summary>
        public void AddForce(in Vector3 force) {
            lock (_tickSynchronization) {
                _lastExternalForce += force;
            }
        }

        /// <summary>
        /// Применяет локальный вектор силы к локальной точке на теле.
        /// <para>Вектор будет преображен в глобальную систему координат по средствам метода <see cref="Transform.TransformVector"/> (с учетом <see cref="Transform.Scale"/></para>
        /// <para>Вектор силы, примененный не к центру масс, раскладывается на саму силу и момент вращения, создаваемой этой силой на рычаге</para>
        /// </summary>
        /// <param name="localForce">Локальный вектор силы, приложенный к телу</param>
        /// <param name="localPosition">Локальная точка, куда приложена сила</param>
        public void AddLocalForceAtLocalPosition(in Vector3 localForce, in Vector3 localPosition) {
            var worldForce = _transform.TransformVector(localForce);
            AddForceAtLocalPosition(in worldForce, in localPosition);
        }

        /// <summary>
        /// Применяет глобальный вектор силы к локальной точке на теле.
        /// <para>Вектор силы, примененный не к центру масс, раскладывается на саму силу и момент вращения, создаваемой этой силой на рычаге</para>
        /// </summary>
        /// <param name="force">Глобальный вектор силы, приложенный к телу</param>
        /// <param name="localPosition">Локальная точка, куда приложена сила</param>
        public void AddForceAtLocalPosition(in Vector3 force, in Vector3 localPosition) {
            var worldPosition = _transform.TransformPoint(localPosition);
            AddForceAtPosition(in force, in worldPosition);
        }

        /// <summary>
        /// Применяет локальный вектор силы к глобальной точке на теле.
        /// <para>Вектор будет преображен в глобальную систему координат по средствам метода <see cref="Transform.TransformVector"/> (с учетом <see cref="Transform.Scale"/></para>
        /// <para>Вектор силы, примененный не к центру масс, раскладывается на саму силу и момент вращения, создаваемой этой силой на рычаге</para>
        /// </summary>
        /// <param name="localForce">Локальный вектор силы, приложенный к телу</param>
        /// <param name="position">Глобальная точка, куда приложена сила</param>
        public void AddLocalForceAtPosition(in Vector3 localForce, in Vector3 position) {
            var worldForce = _transform.TransformVector(localForce);
            AddForceAtPosition(in worldForce, in position);
        }

        /// <summary>
        /// Применяет глобальный вектор силы к глобальной точке на теле.
        /// <para>Вектор силы, примененный не к центру масс, раскладывается на саму силу и момент вращения, создаваемой этой силой на рычаге</para>
        /// </summary>
        /// <param name="force">Глобальный вектор силы, приложенный к телу</param>
        /// <param name="position">Глобальная точка, куда приложена сила</param>
        public void AddForceAtPosition(in Vector3 force, in Vector3 position) {
            lock (_tickSynchronization) {
                _lastExternalForce += force;

                _lastExternalTorque += Vector3.Cross((position - GlobalCenterOfMass), force);
            }
        }

        /// <summary>
        /// Применяет локальный момент вращения к телу
        /// </summary>
        /// <para>Вектор будет преображен в глобальную систему координат по средствам метода <see cref="Transform.TransformVector"/> (с учетом <see cref="Transform.Scale"/></para>
        /// <param name="localTorque">Локальный вектор силы, создающий момент вращения</param>
        public void AddLocalTorque(in Vector3 localTorque) {
            var torque = _transform.TransformVector(localTorque);
            AddTorque(in torque);
        }
        
        /// <summary>
        /// Применяет момент вращения к телу
        /// </summary>
        /// <param name="torque">Вектор силы, создающий момент вращения</param>
        public void AddTorque(in Vector3 torque) {
            lock (_tickSynchronization) {
                _lastExternalTorque += torque;
            }
        }
        
        /// <summary>
        /// Вызываем цикл обновления физики
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime) {
            lock (_tickSynchronization) {
                // Обновляем глобальный инвертированный тензор инерции
                UpdateGlobalInertiaTensor();
            
                // Считаем скорости 
                UpdateVelocities(in deltaTime, 
                                 out Vector3 newLinearVelocity, out Vector3 newAngularVelocity);
                
                // Cчитаем позици и угол поворота
                UpdatePositionAndRotation(in deltaTime, 
                                          newLinearVelocity, newAngularVelocity, 
                                          out Vector3 newGlobalCenterOfMassPosition, out Quaternion newRotation);
                
                // Обновляем значения
                UpdateBodyStates(in newLinearVelocity, in newAngularVelocity, in newGlobalCenterOfMassPosition, newRotation);

                // Сбарсываем все накопленные силы и моменты
                ResetCache();
            }
        }

#endregion

#endregion

#region Private

#region Fields

        /// <summary>
        /// Синхронизатор. Используется, чтобы все операции проводились последовательно
        /// </summary>
        private readonly object _tickSynchronization = new object();

        /// <summary>
        /// Масса тела
        /// </summary>
        private float _mass;
        
        /// <summary>
        /// Инвертированная масса тела
        /// </summary>
        private float _inverseMass;

        /// <summary>
        /// Линейная скорость
        /// </summary>
        private Vector3 _linearVelocity;

        /// <summary>
        /// Угловая скорость
        /// </summary>
        private Vector3 _angularVelocity;

        /// <summary>
        /// Локальный центр масс
        /// </summary>
        private Vector3 _localCenterOfMass;
        
        /// <summary>
        /// Сумма последних внешних сил
        /// </summary>
        private Vector3 _lastExternalForce;
        
        /// <summary>
        /// Сумма последних внешних моментов вращения
        /// </summary>
        private Vector3 _lastExternalTorque;

        /// <summary>
        /// Инвертированный тенсор вращения тела в локальной системе координат
        /// </summary>
        private Vector3 _inverseInertiaTensorLocal;
        
        /// <summary>
        /// Инвертированный тенсор вращения тела в глобальный системе координат
        /// </summary>
        private Matrix3x3 _inverseInertiaTensorGlobal;

        private readonly Transform _transform;
        private readonly List<ISimpleCollider> _attachedColliders;

#endregion

#region Functions

#region Update cycle

        /// <summary>
        /// Вычисляет новые значения скоростей
        /// </summary>
        private void UpdateVelocities(in float deltaTime, out Vector3 newLinearVelocity, out Vector3 newAngularVelocity) {
            // v(t + dt) = v(t) + dt * m^-1 * F;
            newLinearVelocity = _lastExternalForce;
            newLinearVelocity.Scale(LinearLockAxisFactors);
            Vector3Extensions.Scale(ref newLinearVelocity, _inverseMass * deltaTime);
            Vector3Extensions.Add(ref newLinearVelocity, _linearVelocity);

            // w(t + dt) = w(t) + dt * I^-1 * T;
            newAngularVelocity = _inverseInertiaTensorGlobal * _lastExternalTorque;
            newAngularVelocity.Scale(AngularLockAxisFactors);
            Vector3Extensions.Scale(ref newAngularVelocity, in deltaTime);
            Vector3Extensions.Add(ref newAngularVelocity, in _angularVelocity);
        }

        /// <summary>
        /// Вычисляем новые позицию и поворот
        /// </summary>
        private void UpdatePositionAndRotation(in float deltaTime, 
            in Vector3 newLinearVelocity, in Vector3 newAngularVelocity,
            out Vector3 newGlobalCenterOfMassPosition, out Quaternion newRotation) {

            // x(t + dt) = x(t) +          dt * v(t + dt);
            newGlobalCenterOfMassPosition = newLinearVelocity;
            Vector3Extensions.Scale(ref newGlobalCenterOfMassPosition, in deltaTime);
            Vector3Extensions.Add(ref newGlobalCenterOfMassPosition, GlobalCenterOfMass);

            // Q(t + dt) = Q(t) + 0.5 * dt * w(t + dt) * Q(t);
            newRotation = _transform.rotation;
            var tempRotation = QuaternionExtensions.New(0, in newAngularVelocity) * newRotation;
            QuaternionExtensions.Scale(ref tempRotation, deltaTime);
            QuaternionExtensions.Scale(ref tempRotation, 0.5f * deltaTime);
            QuaternionExtensions.Add(ref newRotation, in tempRotation);
        }

        /// <summary>
        /// Обновляем позиционирование 
        /// </summary>
        /// <param name="newLinearVelocity"></param>
        /// <param name="newAngularVelocity"></param>
        /// <param name="newGlobalCenterOfMassPosition"></param>
        /// <param name="newRotation"></param>
        private void UpdateBodyStates(in Vector3 newLinearVelocity, in Vector3 newAngularVelocity, in Vector3 newGlobalCenterOfMassPosition, Quaternion newRotation) {
            _linearVelocity =  newLinearVelocity;
            _angularVelocity = newAngularVelocity;
            
            newRotation.Normalize();
            _transform.SetPositionAndRotation(newGlobalCenterOfMassPosition - newRotation * LocalCenterOfMass, 
                                              newRotation);
        }

        /// <summary>
        /// Сбрасывает все накопленные значения (силы и моменты вращения)
        /// </summary>
        private void ResetCache() {
            _lastExternalForce = Vector3.zero;
            _lastExternalTorque = Vector3.zero;
        }

#endregion

#region Inertia Tesnor

        /// <summary>
        /// Обновляет Инвертированный тенсор вращения тела в глобальный системе координат
        /// </summary>
        private void UpdateGlobalInertiaTensor() {
            Matrix3x3 orientation = _transform.rotation.GetMatrix();
            ComputeWorldInertiaTensorInverse(in orientation,
                                             in _inverseInertiaTensorLocal,
                                             out _inverseInertiaTensorGlobal);
        }

        /// <summary>
        /// Вычисляет инвертированный локальный тензор инерции
        /// </summary>
        private void UpdateLocalInertiaTensorFromColliders() {
            ComputeInertiaTensorLocal(out var inertiaTensorLocal);
            _inverseInertiaTensorLocal = new Vector3(
                inertiaTensorLocal.x.IsEqual(0) ? 0 : 1 / inertiaTensorLocal.x,
                inertiaTensorLocal.y.IsEqual(0) ? 0 : 1 / inertiaTensorLocal.y,
                inertiaTensorLocal.z.IsEqual(0) ? 0 : 1 / inertiaTensorLocal.z
            );
        }
        
        /// <summary>
        /// Вычисляет локальный тензор инерции из коллайдеров, их теоретической массы и фактицеской массы тела
        /// </summary>
        private void ComputeInertiaTensorLocal(out Vector3 localInertiaTensor) {
            Matrix3x3 tempLocalInertiaTensor = new Matrix3x3();
            
            // Вычисляем общую массу коллайдеров
            float totalColliderMass = _attachedColliders.Sum(c => c.MassDensity * c.Volume);
            
            for (int i = 0; i < _attachedColliders.Count; i++) {
                
                // Вычисляем массу коллайдера исходя из Mass 
                ISimpleCollider attachedCollider = _attachedColliders[i];
                float realMass = attachedCollider.Volume * attachedCollider.MassDensity * Mass / totalColliderMass;
                
                // Вычисляем локальный тензор инерции коллайдера
                Vector3 localColliderInertiaTensor = attachedCollider.GetLocalInertiaTensor(realMass);
                
                // Преобразуем локальный тензор инерции коллайдера в тензор тела
                Matrix3x3 rotationMatrix = attachedCollider.ColliderToBodyRotation.GetMatrix();
                Matrix3x3 rotationMatrixTranspose = rotationMatrix.GetTranspose();
                rotationMatrixTranspose.MultiplyRow(0, localColliderInertiaTensor.x);
                rotationMatrixTranspose.MultiplyRow(1, localColliderInertiaTensor.y);
                rotationMatrixTranspose.MultiplyRow(2, localColliderInertiaTensor.z);
                Matrix3x3 inertiaTensor = rotationMatrix * rotationMatrixTranspose;
                
                // Используем теорему параллельных прямых для переноса тензора инерции (w.r.t) колладера на тензор инерции тела
                Vector3 offset = attachedCollider.ColliderCenterToBodyPosition - LocalCenterOfMass;
                var sqrOffset = offset.sqrMagnitude;
                Matrix3x3 offsetMatrix = new Matrix3x3(sqrOffset, 0, 0, 
                                                       0, sqrOffset, 0, 
                                                       0, 0, sqrOffset);
                for(int row = 0; row < 3; row++) {
                    offsetMatrix.AddToRow(in row, offset * (-offset[row]));
                }
                offsetMatrix.Increase(realMass);
                
                tempLocalInertiaTensor.AddMatrix(inertiaTensor);
                tempLocalInertiaTensor.AddMatrix(offsetMatrix);
            }

            localInertiaTensor = new Vector3(tempLocalInertiaTensor[0, 0],
                                             tempLocalInertiaTensor[1, 1],
                                             tempLocalInertiaTensor[2, 2]);
        }

#endregion

#endregion

#endregion

#region UTILS

        /// <summary>
        /// Вычисляет инвертированный тензор инерции в глобальной системе координат по ориентации тела и его инвертированному локальному тензору
        /// </summary>
        /// <param name="orientation">Матрица ориентации тела в пространстве</param>
        /// <param name="inverseInertiaTensorLocal">Инвертированный локальный тензор</param>
        /// <param name="outInverseInertiaTensorWorld">Результирующий инвертированный глобальный тензор инерции</param>
        public static void ComputeWorldInertiaTensorInverse(in Matrix3x3 orientation, in Vector3 inverseInertiaTensorLocal, out Matrix3x3 outInverseInertiaTensorWorld) {
            outInverseInertiaTensorWorld = new Matrix3x3 {
                [0, 0] = orientation[0,0] * inverseInertiaTensorLocal.x,
                [0, 1] = orientation[1,0] * inverseInertiaTensorLocal.x,
                [0, 2] = orientation[2,0] * inverseInertiaTensorLocal.x,
                
                [1, 0] = orientation[0,1] * inverseInertiaTensorLocal.y,
                [1, 1] = orientation[1,1] * inverseInertiaTensorLocal.y,
                [1, 2] = orientation[2,1] * inverseInertiaTensorLocal.y,
                
                [2, 0] = orientation[0,2] * inverseInertiaTensorLocal.z,
                [2, 1] = orientation[1,2] * inverseInertiaTensorLocal.z,
                [2, 2] = orientation[2,2] * inverseInertiaTensorLocal.z
            };

            outInverseInertiaTensorWorld = orientation * outInverseInertiaTensorWorld;
        }

#endregion


    }
}
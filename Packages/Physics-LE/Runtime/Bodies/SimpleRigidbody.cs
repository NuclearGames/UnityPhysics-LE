using System;
using UnityEngine;

namespace NuclearGames.Physics_LE.Bodies {
    
    /// <summary>
    /// Упрощенный вариант Rigidbody
    /// </summary>
    public class SimpleRigidbody : MonoBehaviour {

#region Public

#region Fields

        /// <summary>
        /// Масса компонента
        /// </summary>
        public float Mass;

        /// <summary>
        /// Локальная точка, в которой расположен центр масс
        /// </summary>
        public Vector3 LocalCenterOfMass;

#endregion

#region Properties

        public Vector3 GlobalCenterOfMass => transform.TransformPoint(LocalCenterOfMass);

#endregion


#region Functions
        
        /// <summary>
        /// Применяет локальный вектор силы к центру масс тела.
        /// <para>Вектор будет преображен в глобальную систему координат по средствам метода <see cref="Transform.TransformVector"/> (с учетом <see cref="Transform.Scale"/></para>
        /// <param name="localForce">Локальный вектор силы, применимый к телу</param>
        /// </summary>
        public void AddLocalForce(in Vector3 localForce) {
            var worldForce = transform.TransformVector(localForce);
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
            var worldForce = transform.TransformVector(localForce);
            AddForceAtLocalPosition(in worldForce, in localPosition);
        }

        /// <summary>
        /// Применяет глобальный вектор силы к локальной точке на теле.
        /// <para>Вектор силы, примененный не к центру масс, раскладывается на саму силу и момент вращения, создаваемой этой силой на рычаге</para>
        /// </summary>
        /// <param name="force">Глобальный вектор силы, приложенный к телу</param>
        /// <param name="localPosition">Локальная точка, куда приложена сила</param>
        public void AddForceAtLocalPosition(in Vector3 force, in Vector3 localPosition) {
            var worldPosition = transform.TransformPoint(localPosition);
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
            var worldForce = transform.TransformVector(localForce);
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

                _lastTorque += Vector3.Cross((position - GlobalCenterOfMass), force);
            }
        }

#endregion

#endregion

#region Private

#region Fields

        private readonly object _tickSynchronization = new object();

        private Vector3 _lastExternalForce;
        private Vector3 _lastTorque;

#endregion

#endregion


    }
}
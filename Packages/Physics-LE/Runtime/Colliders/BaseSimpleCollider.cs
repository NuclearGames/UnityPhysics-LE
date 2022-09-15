using NuclearGames.Physics_LE.Colliders.Interfaces;
using NuclearGames.Physics_LE.Utils.Extensions;
using UnityEngine;

namespace NuclearGames.Physics_LE.Colliders {
    public abstract class BaseSimpleCollider : ISimpleCollider {
        /// <summary>
        /// Плотность материала коллайдера
        /// </summary>
        public float MassDensity => 1;

        /// <summary>
        /// Объем коллайдера
        /// </summary>
        public float Volume {
            get {
                if (!Baked) {
                    UpdateVolume();
                }

                return _volume;
            }
        }

        /// <summary>
        /// Позиция центральной точки коллайдера относительно тела
        /// </summary>
        public Vector3 ColliderCenterToBodyPosition {
            get {
                if (!Baked)  {
                    UpdateToBodyPosition();
                }
                return _centerToBodyPosition;
            }
        }

        /// <summary>
        /// Локальное смещение центра коллайдера от центра его Transform
        /// </summary>
        public Vector3 LocalCenter {
            get => _colliderLocalCenterOffsetPosition;
            set {
                _colliderLocalCenterOffsetPosition = value;
                UpdateToBodyPosition();
            }
        }

        /// <summary>
        /// Поворот коллайдера относительно тела
        /// </summary>
        public Quaternion ColliderToBodyRotation {
            get {
                if (!Baked)  {
                    UpdateToBodyRotation();
                }
                return _toBodyRotation;
            }
        }

        /// <summary>
        /// Изменяется ли положение колладера относительно тела
        /// <para>True - Не меняется, False - меняется</para>
        /// </summary>
        internal bool Baked {
            get => _baked;
            set {
                if (value) {
                    UpdateToBodyRotation();
                    UpdateToBodyPosition();
                    UpdateVolume();
                }
                _baked = value;
            }
        } 
        
        private Vector3 _colliderLocalCenterOffsetPosition;
        
        private bool _baked;
        private protected float _volume;
        private Vector3 _centerToBodyPosition;
        private Quaternion _toBodyRotation;

        private readonly Transform _bodyTransform;
        private readonly Transform _colliderTransform;
        protected BaseSimpleCollider(Transform bodyTransform, Transform colliderTransform, bool baked = false) {
            _bodyTransform = bodyTransform;
            _colliderTransform = colliderTransform;

            Baked = baked;
        }
        
        /// <summary>
        /// Возвращает локальный тензор инерции коллайдера
        /// </summary>
        public abstract Vector3 GetLocalInertiaTensor(in float mass);

#region Utils

        /// <summary>
        /// Вычисляет и кэширует Quaternion поворота коллайдера относительно тела
        /// </summary>
        private void UpdateToBodyRotation() {
            _toBodyRotation = _bodyTransform.rotation.Difference(_colliderTransform.rotation);
        }

        /// <summary>
        /// Вычисляет и кэширует позицию коллайдера относительно тела
        /// </summary>
        private void UpdateToBodyPosition() {
            //ToDo: без скейла
            var globalDiffPositions = _colliderTransform.position - _bodyTransform.position;
            var diffRotation = _bodyTransform.rotation.Difference(_colliderTransform.rotation);
            _centerToBodyPosition = globalDiffPositions + diffRotation * _colliderLocalCenterOffsetPosition;
        }

        /// <summary>
        /// Вычисляет объем коллайдера
        /// </summary>
        private protected abstract void UpdateVolume();

#endregion
    }
}
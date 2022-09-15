using UnityEngine;

namespace NuclearGames.Physics_LE.Colliders.Colliders3 {
    /// <summary>
    /// Коллайдер сферы
    /// </summary>
    public sealed class SimpleSphere3 : BaseSimpleCollider {

        private readonly SphereCollider _collider;
        
        public SimpleSphere3(Transform bodyTransform, SphereCollider collider, bool baked = false) :
            base(bodyTransform, collider.transform, baked) {
            _collider = collider;
        }
        
        /// <summary>
        /// Возвращает локальный тензор инерции коллайдера
        /// </summary>
        private protected override Vector3 GetNoMassLocalInertiaTensor() {
            var r = _collider.radius;
            return new Vector3(0, 0, 0.4f * r * r);
        }

        /// <summary>
        /// Вычисляет объем коллайдера
        /// </summary>
        private protected override float GetVolumeInternal() {
            var r = _collider.radius;
            return 4 * Mathf.PI * r * r * r;
        }
    }
}
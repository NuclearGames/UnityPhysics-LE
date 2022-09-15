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
        public override Vector3 GetLocalInertiaTensor(in float mass) {
            var r = _collider.radius;
            return new Vector3(0, 0, 0.4f * mass * r * r);
        }

        /// <summary>
        /// Вычисляет объем коллайдера
        /// </summary>
        private protected override void UpdateVolume() {
            var r = _collider.radius;
            _volume = 4 * Mathf.PI * r * r * r;
        }
    }
}
using UnityEngine;

namespace NuclearGames.Physics_LE.Colliders.Colliders3 {
    /// <summary>
    /// Коллайдер параллелепипеда
    /// </summary>
    public sealed class SimpleBoxCollider3 : BaseSimpleCollider {
        
        private readonly BoxCollider _collider;

        public SimpleBoxCollider3(Transform bodyTransform, BoxCollider collider, bool baked = false) :
            base(bodyTransform, collider.transform, baked) {
            _collider = collider;
        }
        
        /// <summary>
        /// Возвращает локальный тензор инерции коллайдера
        /// </summary>
        public override Vector3 GetLocalInertiaTensor(in float mass) {
            var size = _collider.size;
            float halfX = size.x / 2, 
                  halfY = size.y / 2, 
                  halfZ = size.z / 2;
            float xSquare = halfX * halfX,
                  ySquare = halfY * halfY,
                  zSquare = halfZ * halfZ;
            var factor = 1f / 3f * mass;

            return new Vector3(factor * (ySquare + zSquare),
                               factor * (xSquare + zSquare),
                               factor * (xSquare + ySquare));
        }
        
        /// <summary>
        /// Вычисляет объем коллайдера
        /// </summary>
        private protected override void UpdateVolume() {
            var size = _collider.size;
            _volume = size.x * size.y * size.z;
        }
    }
}
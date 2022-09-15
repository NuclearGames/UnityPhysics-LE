using System;
using UnityEngine;

namespace NuclearGames.Physics_LE.Colliders.Colliders3 {
    /// <summary>
    /// Коллайдер капсулы
    /// </summary>
    public sealed class SimpleCapsuleCollider3 : BaseSimpleCollider {
        
        /// <summary>
        /// Математическая высота капсулы
        /// </summary>
        private float Height {
            get {
                var value = _collider.height - _collider.radius * 2;
                return value < 0 ? 0 : value;
            }
        }
        
        private readonly CapsuleCollider _collider;

        public SimpleCapsuleCollider3(Transform bodyTransform, CapsuleCollider collider, bool baked = false) :
            base(bodyTransform, collider.transform, baked) {
            _collider = collider;
        }
        
        /// <summary>
        /// Возвращает локальный тензор инерции коллайдера
        /// </summary>
        public override Vector3 GetLocalInertiaTensor(in float mass) {
            var radius = _collider.radius;
            var radiusSquare = radius * radius;
            var height = Height;
            var heightSquare = height * height;
            var radiusSquareDouble = radiusSquare + radiusSquare;

            var factorDivider = 4 * radius + 3 * height;
            var semiSphereFactor = 2f * radius / factorDivider;
            var cylinderFactor = 3 * height / factorDivider;
            
            float sphereMoment = 0.4f * radiusSquareDouble;
            float sphereOffset = 0.75f * height * radius + 
                                 0.5f * heightSquare;
            float cylinderMoment = 0.25f * radiusSquare +
                                   (1.0f / 12.0f) * heightSquare;

            float semiSphereCross = semiSphereFactor * mass * (sphereMoment + sphereOffset);
            float cylinderCross = cylinderFactor * mass * cylinderMoment;
            float cross = cylinderCross + semiSphereCross;

            float semiSphereAlong = semiSphereFactor * mass * sphereMoment;
            float cylinderAlong = cylinderFactor * mass * 0.25f * radiusSquareDouble;
            float along = semiSphereAlong + cylinderAlong;

            return _collider.direction switch {
                0 => new Vector3(along, cross, cross),
                1 => new Vector3(cross, along, cross),
                2 => new Vector3(cross, cross, along),
                _ => throw new ArgumentOutOfRangeException(nameof(_collider.direction))
            };
        }

#region Utils

        /// <summary>
        /// Вычисляет объем коллайдера
        /// </summary>
        private protected override void UpdateVolume() {
            var radius = _collider.radius;
            var radius2 = radius * radius;

            _volume = Mathf.PI * radius2 * (4f / 3f * radius + Height);
        }

#endregion
    }
}
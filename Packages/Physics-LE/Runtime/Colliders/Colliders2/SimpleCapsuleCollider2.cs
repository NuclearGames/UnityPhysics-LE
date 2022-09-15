using System;
using NuclearGames.Physics_LE.Utils.Extensions;
using UnityEngine;

namespace NuclearGames.Physics_LE.Colliders.Colliders2 {
    /// <summary>
    /// Коллайдеры двумерной капсулы.
    /// <para>Представляет собой комбинацию параллелепипеда и двух полуцилинров с очень маленькой глубиной</para>
    /// </summary>
    internal sealed class SimpleCapsuleCollider2 : BaseSimpleCollider {

        /// <summary>
        /// Математическая высота капсулы
        /// </summary>
        private float HalfWidth {
            get {
                var size = _collider.size;

                var value = _collider.direction switch {
                    CapsuleDirection2D.Horizontal => size.x - size.y / 2,
                    CapsuleDirection2D.Vertical => size.y - size.x / 2,
                    _ => throw new ArgumentOutOfRangeException()
                };

                return value < 0 ? 0 : value;
            }
        }

        /// <summary>
        /// Радиус окружности капсулы
        /// </summary>
        private float Radius {
            get {
                return _collider.direction switch {
                    CapsuleDirection2D.Horizontal => _collider.size.y,
                    CapsuleDirection2D.Vertical => _collider.size.x,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        
        private readonly CapsuleCollider2D _collider;
        
        public SimpleCapsuleCollider2(Transform bodyTransform, CapsuleCollider2D collider, bool baked = false) :
            base(bodyTransform, collider.transform, baked) {
            _collider = collider;
        }
        
        /// <summary>
        /// Возвращает локальный тензор инерции коллайдера
        /// <para>В силу того, что двумерное тело умеет вращаться только вокруг оси Z, остальные мы не считаем</para>
        /// </summary>
        private protected override Vector3 GetNoMassLocalInertiaTensor() {
            const float oneDiv3 = 1f / 3f;
            const float eightDivThreePi = 8f / Mathf.PI * oneDiv3;
            
            float x = HalfWidth;
            float r = Radius;
            
            float x4 = 4 * x;
            float piR = Mathf.PI * r;

            float xSquare = x * x;
            float rSquare = r * r;

            float dividerFactor = x4 + piR;
            float boxFactor = x4 / dividerFactor;
            float cylinderFactor = piR / dividerFactor;

            float boxSquareZ = oneDiv3 * (xSquare + rSquare);
            float cylinderSquareZ = (rSquare / 2) + (eightDivThreePi * x * r) + xSquare;

            float inertiaZ = boxFactor * boxSquareZ + cylinderFactor * cylinderSquareZ;
            return new Vector3(0, 0, inertiaZ);
        }

        private protected override float GetVolumeInternal() {
            float radius = Radius,
                  width = HalfWidth * 2,
                  height = radius * 2,
                  deep = FloatExtensions.ZERO;

            return width * height * deep +
                   radius * radius * Mathf.PI * deep;
        }
    }
}
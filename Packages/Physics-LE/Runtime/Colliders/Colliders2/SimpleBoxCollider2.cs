using NuclearGames.Physics_LE.Utils.Extensions;
using UnityEngine;

namespace NuclearGames.Physics_LE.Colliders.Colliders2 {
    /// <summary>
    /// Коллайдер параллелепипеда с очень маленькой глубиной.
    /// </summary>
    public sealed class SimpleBoxCollider2 : BaseSimpleCollider {
        
        private readonly BoxCollider2D _collider;

        public SimpleBoxCollider2(Transform bodyTransform, BoxCollider2D collider, bool baked = false) :
            base(bodyTransform, collider.transform, baked) {
            _collider = collider;
        }
        
        /// <summary>
        /// Возвращает локальный тензор инерции коллайдера
        /// <para>В силу того, что двумерное тело умеет вращаться только вокруг оси Z, остальные мы не считаем</para>
        /// </summary>
        public override Vector3 GetLocalInertiaTensor(in float mass) {
            var size = _collider.size;
            float halfX = size.x / 2,
                  halfY = size.y / 2;
            float xSquare = halfX * halfX,
                  ySquare = halfY * halfY;
            var factor = 1f / 3f * mass;

            return new Vector3(0, 0, factor * (xSquare + ySquare));
        }
        
        /// <summary>
        /// Вычисляет объем коллайдера
        /// </summary>
        private protected override void UpdateVolume() {
            var size = _collider.size;
            _volume = size.x * size.y * FloatExtensions.ZERO;
        }
    }
}
using NuclearGames.Physics_LE.Utils.Extensions;
using UnityEngine;

namespace NuclearGames.Physics_LE.Colliders.Colliders2 {
    /// <summary>
    /// Коллайдер двумерной сферы (круга).
    /// <para>Представляет собой цилиндр с очень маленькой глубиной</para>
    /// </summary>
    public sealed class SimpleSphere2 : BaseSimpleCollider {

        private readonly CircleCollider2D _collider;

        public SimpleSphere2(Transform bodyTransform, CircleCollider2D collider, bool baked = false) :
            base(bodyTransform, collider.transform, baked) {
            _collider = collider;
        }
        
        /// <summary>
        /// Возвращает локальный тензор инерции коллайдера
        /// <para>В силу того, что двумерное тело умеет вращаться только вокруг оси Z, остальные мы не считаем</para>
        /// </summary>
        public override Vector3 GetLocalInertiaTensor(in float mass) {
            var r = _collider.radius;
            return new Vector3(0, 0, 0.5f * mass * r * r);
        }

        /// <summary>
        /// Вычисляет объем коллайдера
        /// </summary>
        private protected override void UpdateVolume() {
            var r = _collider.radius;
            _volume = Mathf.PI * r * r * FloatExtensions.ZERO;
        }
    }
}
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
        private protected override Vector3 GetNoMassLocalInertiaTensor() {
            var r = _collider.radius;
            return new Vector3(0, 0, 0.5f * r * r);
        }

        /// <summary>
        /// Вычисляет объем коллайдера
        /// </summary>
        private protected override float GetVolumeInternal() {
            var r = _collider.radius;
            return Mathf.PI * r * r * FloatExtensions.ZERO;
        }
    }
}
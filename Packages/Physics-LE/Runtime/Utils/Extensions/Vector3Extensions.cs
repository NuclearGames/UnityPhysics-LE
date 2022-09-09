using UnityEngine;

namespace NuclearGames.Physics_LE.Utils.Extensions {
    public static class Vector3Extensions {
        /// <summary>
        /// Умножает компоненты вектора на величину
        /// </summary>
        public static void Scale(ref Vector3 value, in float multiplier) {
            value.x *= multiplier;
            value.y *= multiplier;
            value.z *= multiplier;
        }

        /// <summary>
        /// Добавляет к компонентам текущего вектора соответсвующие комопоненты другого вектора
        /// </summary>
        public static void Add(ref Vector3 value, in Vector3 other) {
            value.x += other.x;
            value.y += other.y;
            value.z += other.z;
        }
    }
}
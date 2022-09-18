using NuclearGames.Physics_LE.Utils.Mathematics;
using UnityEngine;

namespace NuclearGames.Physics_LE.Utils.Extensions {
    public static class QuaternionExtensions {

        /// <summary>
        /// Добавляет к компонентам исходного кватерниона сответсвующие компоненты другого кватерниона
        /// </summary>
        public static void Add(ref Quaternion value, in Quaternion other) {
            value.x += other.x;
            value.y += other.y;
            value.z += other.z;
            value.w += other.w;
        }
        
        /// <summary>
        /// Умножает компоненты исходного кватерниона на величину
        /// </summary>
        public static void Scale(ref Quaternion value, in float multiplier) {
            value.x *= multiplier;
            value.y *= multiplier;
            value.z *= multiplier;
            value.w *= multiplier;
        }
        
        /// <summary>
        /// Возвращает матрицу поворота
        /// </summary>
        public static Matrix3x3 GetMatrix(this Quaternion value) {
            float nQ = value.x * value.x + value.y * value.y + value.z * value.z + value.w * value.w;
            float s = nQ > 0
                ? 2 / nQ
                : 0;

            // Computations used for optimization (less multiplications)
            float xs = value.x * s;
            float ys = value.y * s;
            float zs = value.z * s;
            float wxs = value.w * xs;
            float wys = value.w * ys;
            float wzs = value.w * zs;
            float xxs = value.x * xs;
            float xys = value.x * ys;
            float xzs = value.x * zs;
            float yys = value.y * ys;
            float yzs = value.y * zs;
            float zzs = value.z * zs;

            return new Matrix3x3(1f - yys - zzs,
                                 xys - wzs,
                                 xzs + wys,
                                 xys + wzs,
                                 1f - xxs - zzs,
                                 yzs - wxs,
                                 xzs - wys,
                                 yzs + wxs,
                                 1f - xxs - yys);
        }

        /// <summary>
        /// Перекладывает компоненты вектора на кватернион
        /// </summary>
        public static Quaternion New(in float w, in Vector3 vector) {
            return new Quaternion(vector.x, vector.y, vector.z, w);
        }

        /// <summary>
        /// Получает кватерниор разницы поротов от источника до цели
        /// <para></para>
        /// <para>M-final = M-rot * M-original</para>
        /// <para>M-final * M-original^-1 = M-rot * (M-original * M-original^-1)</para>
        /// <para>M-final * M-original^-1 = M-rot * E</para>
        /// </summary>
        public static Quaternion Difference(this Quaternion original, in Quaternion final) {
            return final * Quaternion.Inverse(original);
        }
        
        /// <summary>
        /// Получает кватерниор разницы поротов от источника до цели
        /// <para></para>
        /// <para>M-final = M-rot * M-original </para>
        /// <para>M-rot^-1 * M-final = (M-rot^-1 * M-rot) * M-original </para>
        /// <para>M-rot^-1 * M-final = E * M-original </para>
        /// </summary>
        public static Quaternion RestoreFromDifference(this Quaternion final, in Quaternion difference) {
            return Quaternion.Inverse(difference) * final;
        }

        /// <summary>
        /// Проверяет со степень сравнения float, что кватернионы одинаковы
        /// </summary>
        public static bool Approximately(in Quaternion q1, in Quaternion q2) {
            return Mathf.Approximately(q1.w, q2.w) &&
                   Mathf.Approximately(q1.x, q2.x) &&
                   Mathf.Approximately(q1.y, q2.y) &&
                   Mathf.Approximately(q1.z, q2.z);
        }
    }
}
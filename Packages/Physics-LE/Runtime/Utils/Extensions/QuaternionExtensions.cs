using NuclearGames.Physics_LE.Utils.Mathematics;
using UnityEngine;

namespace NuclearGames.Physics_LE.Utils.Extensions {
    internal static class QuaternionExtensions {
        internal static Matrix3x3 GetMatrix(this Quaternion value) {
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
    }
}
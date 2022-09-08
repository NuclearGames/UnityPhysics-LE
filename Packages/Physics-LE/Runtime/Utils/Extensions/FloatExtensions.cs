using UnityEngine;

namespace NuclearGames.Physics_LE.Utils.Extensions {
    internal static class FloatExtensions {
        internal static bool IsEqual(this float value, in float to) {
            return Mathf.Approximately(value, to);
        }
    }
}
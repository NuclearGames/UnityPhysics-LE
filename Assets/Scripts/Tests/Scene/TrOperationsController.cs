using NuclearGames.Physics_LE.Utils.Extensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Tests.Scene {
    internal sealed class TrOperationsController : MonoBehaviour {
        [SerializeField]
        private Transform root;

        [SerializeField]
        private Vector3 colliderOffsetPosition;

        private Transform Transform => _transform ??= transform;
        
        private Transform _transform;


        [ContextMenu("Check local rotation")]
        private void CheckLocalRotation() {
            var diffRotation = root.rotation.Difference(Transform.rotation);
            Assert.IsTrue(QuaternionExtensions.Approximately(Transform.localRotation, in diffRotation));
        }
        
        [ContextMenu("Check difference rotation")]
        private void CheckDifferenceRotation() {
            var diffRotation = root.rotation.Difference(Transform.rotation);
            Assert.IsTrue(QuaternionExtensions.Approximately(root.rotation, Transform.rotation.RestoreFromDifference(in diffRotation)));
        }


        [ContextMenu("Check offset position")]
        private void CheckOffsetPosition() {
            var diffRotation = root.rotation.Difference(Transform.rotation);
            var globalDifference = Transform.position - root.position;
            Debug.Log($"[Wc2]: {Transform.TransformPoint(colliderOffsetPosition)}"); 
            Debug.Log($"[Wr]: {globalDifference + diffRotation * colliderOffsetPosition}"); 
        }

        [ContextMenu("Log Capsule direction")]
        private void LogDirection() {
            Debug.Log($"Direction: {GetComponent<CapsuleCollider>().direction}");
        }
    }
}
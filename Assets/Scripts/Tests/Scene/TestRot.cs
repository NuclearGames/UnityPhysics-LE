using NuclearGames.Physics_LE.Utils.Extensions;
using UnityEngine;

namespace Tests.Scene {
    internal sealed class TestRot : MonoBehaviour {
        [SerializeField] private Transform _colliderTransform;
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private Vector3 _colliderLocalCenterOffsetPosition;

        [Header("")]
        [SerializeField] private Vector3 vec;

        [ContextMenu("Test")]
        private void Test() {
            var globalDiffPositions = _colliderTransform.position - _bodyTransform.position;
            var diffRotation = _bodyTransform.rotation.Difference(_colliderTransform.rotation);
            var centerToBodyPosition = globalDiffPositions + diffRotation * _colliderLocalCenterOffsetPosition;
            // Позиция центральной точки коллайдера относительно тела
            Debug.Log(centerToBodyPosition);


            var toBodyRotation = _bodyTransform.rotation.Difference(_colliderTransform.rotation);
            Debug.Log(toBodyRotation.eulerAngles);
        }

        [ContextMenu("TestVec")]
        private void TestVec() {
            var rot = _bodyTransform.rotation;

            var resultM = rot * vec;
            var resultT = _bodyTransform.TransformVector(vec);
            Debug.Log($"M = {resultM} | T = {resultT}");

            var pointResM = rot * vec + _bodyTransform.position;
            var pointResT = _bodyTransform.TransformPoint(vec);
            Debug.Log($"M = {pointResM} | T = {pointResT}");
        }
    }
}

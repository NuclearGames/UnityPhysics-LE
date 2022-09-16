using NuclearGames.Physics_LE.Bodies;
using UnityEngine;
using NuclearGames.Physics_LE.Utils.Extensions;

namespace Tests.Scene {
    internal sealed class PhysicsTestEnvironment : MonoBehaviour {
        [SerializeField] private Rigidbody unityRigidbody;
        [SerializeField] private PhysicsTestBody simpleBody;

        [Header("Assertions")]
        [SerializeField] private bool assertPosition = true;
        [SerializeField] private bool assertRotation = true;

        [Header("")]
        [SerializeField] private Vector3 vec3;

        internal Rigidbody UnityRigidbody => unityRigidbody;
        internal SimpleRigidbody SimpleRigidbody => simpleBody.Rigidbody;

        /// <summary>
        /// Сравнивает позициии и вращения.
        /// </summary>
        private void FixedUpdate() {
            if (assertPosition) {
                Vector3 unityPos = UnityRigidbody.transform.position;
                Vector3 simplePos = simpleBody.Transform.position;
                float distance = Vector3.Distance(unityPos, simplePos);

                Debug.Assert(Mathf.Approximately(distance, 0f),
                    $"DeltaPosition = {distance}");
            }

            if (assertRotation) {
                Quaternion unityRot = UnityRigidbody.transform.rotation;
                Quaternion simpleRot = simpleBody.Transform.rotation;
                float angle = Quaternion.Angle(unityRot, simpleRot);

                Debug.Assert(Mathf.Approximately(angle, 0f),
                    $"DeltaAngle = {angle}");

                //Debug.Assert(QuaternionExtensions.Approximately(unityRot, simpleRot),
                //    $"DeltaAngle = {Quaternion.Angle(unityRot, simpleRot)}");
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SetVelocity();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                AddForce();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                SetAngularVelocity();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                AddTorque();
            }
        }

        [ContextMenu("Set Velocity")]
        private void SetVelocity() {
            UnityRigidbody.velocity = vec3;
            SimpleRigidbody.LinearVelocity = vec3;
        }

        [ContextMenu("Add Force")]
        private void AddForce() {
            UnityRigidbody.AddForce(vec3);
            SimpleRigidbody.AddForce(vec3);
        }

        [ContextMenu("Set Angular Velocity")]
        private void SetAngularVelocity() {
            UnityRigidbody.angularVelocity = vec3;
            SimpleRigidbody.AngularVelocity = vec3;
        }

        [ContextMenu("Add Torque")]
        private void AddTorque() {
            UnityRigidbody.AddTorque(vec3);
            SimpleRigidbody.AddTorque(vec3);
        }
    }
}

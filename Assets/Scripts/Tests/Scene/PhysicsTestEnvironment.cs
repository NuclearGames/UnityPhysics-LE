using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests.Scene {
    internal sealed class PhysicsTestEnvironment : MonoBehaviour {
        [SerializeField] private SimulationSceneLoader simulationSceneLoader;
        [Space]
        
        [SerializeField] private Rigidbody unityRigidbody;

        [Header("Assertions")]
        [SerializeField] private bool assertPosition = true;
        [SerializeField] private bool assertRotation = true;

        [Header("")]
        [SerializeField] private Vector3 vec3;

        internal Rigidbody AutoRigidbody => unityRigidbody;
        internal Rigidbody ManualRigidbody { get; private set; }


        private float _initialDeltaDistance;
        private float _initialDeltaAngle;

        private void Awake() {
            simulationSceneLoader.CreateScene();
        }

        private void Start() {
            ManualRigidbody = simulationSceneLoader.InstantiateUnityRigidbody(unityRigidbody.transform);
            
            unityRigidbody.transform.Translate(Vector3.left * 2);
            unityRigidbody.gameObject.SetActive(true);
            
            _initialDeltaDistance = Vector3.Distance(ManualRigidbody.transform.position, AutoRigidbody.transform.position);
            _initialDeltaAngle = Quaternion.Angle(ManualRigidbody.transform.rotation, AutoRigidbody.transform.rotation);
        }


        /// <summary>
        /// Сравнивает позициии и вращения.
        /// </summary>
        private void FixedUpdate() {
            const int bufferCapacity = 100;
            
            simulationSceneLoader.UpdatePhysics(Time.fixedDeltaTime);
            
            if (assertPosition) {
                Vector3 unityPos = ManualRigidbody.transform.position;
                Vector3 simplePos = AutoRigidbody.transform.position;
                float distance = Vector3.Distance(unityPos, simplePos);

                Debug.Assert(Approximately(distance, _initialDeltaDistance),
                             $"DeltaPosition = {distance}");
            }

            if (assertRotation) {
                Quaternion unityRot = ManualRigidbody.transform.rotation;
                Quaternion simpleRot = AutoRigidbody.transform.rotation;
                float angle = Quaternion.Angle(unityRot, simpleRot);

                Debug.Assert(Approximately(angle, _initialDeltaAngle),
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
            ManualRigidbody.velocity = vec3;
            AutoRigidbody.velocity = vec3;
        }

        [ContextMenu("Add Force")]
        private void AddForce() {
            ManualRigidbody.AddForce(vec3);
            AutoRigidbody.AddForce(vec3);
        }

        [ContextMenu("Set Angular Velocity")]
        private void SetAngularVelocity() {
            unityRigidbody.angularVelocity = vec3;
            //
            ManualRigidbody.angularVelocity = vec3;
            AutoRigidbody.angularVelocity = vec3;
        }

        [ContextMenu("Add Torque")]
        private void AddTorque() {
            ManualRigidbody.AddTorque(vec3);
            AutoRigidbody.AddTorque(vec3);
        }

        private bool Approximately(float value1, float value2) {
            const float scaler = 1E-03f; // 1E-06f;

            return Mathf.Abs(value1 - value2) < scaler;
        }
    }
}

using System.Collections.Generic;
using NuclearGames.Physics_LE.Bodies;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests.Scene {
    internal sealed class SimulationSceneLoader : MonoBehaviour {
        private UnityEngine.SceneManagement.Scene _simulationScene;
        private PhysicsScene _physicsScene;

        private readonly List<Rigidbody> _unityRigidbodies = new List<Rigidbody>();
        private readonly List<SimpleRigidbody> _reactRigidbodies = new List<SimpleRigidbody>();

        internal void CreateScene() {
            _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
            _physicsScene = _simulationScene.GetPhysicsScene();
        }

        internal Rigidbody InstantiateUnityRigidbody(Transform obj)  {
            var go = Instantiate(obj.gameObject, obj.position, obj.rotation);
            go.SetActive(true);
            
            var requestedComponent = go.GetComponentInChildren<Rigidbody>();
            
            SceneManager.MoveGameObjectToScene(go, _simulationScene);
            
            _unityRigidbodies.Add(requestedComponent);

            return requestedComponent;
        }
        
        internal PhysicsTestBody InstantiateSimpleRigidbody(Transform simpleObj, Rigidbody sourceBody) {
            var go = Instantiate(simpleObj.gameObject, simpleObj.position, simpleObj.rotation);
            go.SetActive(true);
            
            var requestedComponent = go.GetComponentInChildren<PhysicsTestBody>();
            requestedComponent.Initialize(sourceBody);
            
            SceneManager.MoveGameObjectToScene(go, _simulationScene);

            _reactRigidbodies.Add(requestedComponent.Rigidbody);

            return requestedComponent;
        }

        internal void UpdatePhysics(float deltaTime) {
            _physicsScene.Simulate(deltaTime);
            _reactRigidbodies.ForEach(r => r.Update(deltaTime));
        }
    }
}
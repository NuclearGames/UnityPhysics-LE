using NuclearGames.Physics_LE.Bodies;
using NuclearGames.Physics_LE.Colliders.Colliders3;
using NuclearGames.Physics_LE.Colliders.Interfaces;
using UnityEngine;

namespace Tests.Scene {
    internal sealed class PhysicsTestBody : MonoBehaviour {
        [SerializeField] private Rigidbody externalRigidbody;
        [SerializeField] private SphereCollider externalCollider;

        internal Transform Transform => transform;
        internal SimpleRigidbody Rigidbody { get; private set; }
        private ISimpleCollider _collider;

        private void Awake() {
            _collider = new SimpleSphere3(transform, externalCollider);

            Rigidbody = new SimpleRigidbody(transform, new ISimpleCollider[] { _collider });
            Rigidbody.LinearLockAxisFactors = Vector3.one;
            Rigidbody.AngularLockAxisFactors = Vector3.one;
            Rigidbody.Mass = externalRigidbody.mass;
            Rigidbody.LocalCenterOfMass = externalRigidbody.centerOfMass;
        }

        private void FixedUpdate() {
            Rigidbody.Update(Time.fixedDeltaTime);
        }
    }
}

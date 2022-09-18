using NuclearGames.Physics_LE.Bodies;
using NuclearGames.Physics_LE.Colliders.Colliders3;
using NuclearGames.Physics_LE.Colliders.Interfaces;
using UnityEngine;

namespace Tests.Scene {
    internal sealed class PhysicsTestBody : MonoBehaviour {
        [SerializeField] private SphereCollider externalCollider;

        internal SimpleRigidbody Rigidbody { get; private set; }
        private ISimpleCollider _collider;

        internal void Initialize(Rigidbody source) {
            var cachedTransform = transform;
            _collider = new SimpleSphere3(cachedTransform, externalCollider);

            Rigidbody = new SimpleRigidbody(cachedTransform, new ISimpleCollider[] { _collider }) {
                LinearLockAxisFactors = Vector3.one,
                AngularLockAxisFactors = Vector3.one,
                Mass = source.mass,
                LocalCenterOfMass = source.centerOfMass
            };
        }
    }
}

using NuclearGames.Physics_LE.Utils.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace NuclearGames.Physics_LE.Tests.Tests.Runtime.Utils {
    public sealed class Vector3ExtensionsTests {

        [Test]
        public void SetUpTest() {
            var source = CreateRandomVector();
            Vector3 destination;
            destination = source;
            
            Assert.AreEqual(source, destination);
            Assert.AreNotSame(source, destination);
        }
        
        [Test]
        public void AddTest() {
            Vector3 v1 = CreateRandomVector();
            Vector3 v1Copy = Copy(in v1);
            Vector3 v2 = CreateRandomVector();
            
            Vector3Extensions.Add(ref v1, in v2);

            for (int i = 0; i < 3; i++) {
                Assert.IsTrue(Mathf.Approximately(v1[i], v1Copy[i] + v2[i]));
            }
        }
        
        [Test]
        public void ScaleTest() {
            Vector3 v1 = CreateRandomVector();
            Vector3 v1Copy = Copy(in v1);
            float multiplier = Random.Range(0, 15);
            
            Vector3Extensions.Scale(ref v1, in multiplier);

            for (int i = 0; i < 3; i++) {
                Assert.IsTrue(Mathf.Approximately(v1[i], v1Copy[i] * multiplier));
            }
        }
        
        
        private Vector3 CreateRandomVector(float module = 1f) {
            return new Vector3(
                Random.Range(-module, module),
                Random.Range(-module, module),
                Random.Range(-module, module)
            );
        }
        
        private Vector3 Copy(in Vector3 value) {
            return new Vector3(
                value.x,
                value.y,
                value.z
            );
        }
    }
}
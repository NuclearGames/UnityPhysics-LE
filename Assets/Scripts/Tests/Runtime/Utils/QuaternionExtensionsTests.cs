using NuclearGames.Physics_LE.Utils.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace NuclearGames.Physics_LE.Tests.Tests.Runtime.Utils {
    public sealed class QuaternionExtensionsTests {
        [Test]
        public void AddTest() {
            Quaternion q1 = CreateRandom();
            Quaternion q1Copy = Copy(in q1);
            Quaternion q2 = CreateRandom();
            
            QuaternionExtensions.Add(ref q1, in q2);

            for (int i = 0; i < 4; i++) {
                Assert.IsTrue(Mathf.Approximately(q1[i], q1Copy[i] + q2[i]));
            }
        }
        
        [Test]
        public void ScaleTest() {
            Quaternion q1 = CreateRandom();
            Quaternion q1Copy = Copy(in q1);
            float multiplier = Random.Range(0, 15);
            
            QuaternionExtensions.Scale(ref q1, in multiplier);

            for (int i = 0; i < 4; i++) {
                Assert.IsTrue(Mathf.Approximately(q1[i], q1Copy[i] * multiplier));
            }
        }

        [Test]
        public void CtorParseTest() {
            var vector = CreateRandomVector();
            var w = 0.5123f;
            
            var q = QuaternionExtensions.New(in w, in vector);
            
            for (int i = 0; i < 3; i++) {
                Assert.IsTrue(Mathf.Approximately(q[i], vector[i]));
            }
            Assert.IsTrue(Mathf.Approximately(q.w, w));
        }


        private Vector3 CreateRandomVector(float module = 1f) {
            return new Vector3(
                Random.Range(-module, module),
                Random.Range(-module, module),
                Random.Range(-module, module)
            );
        }
        
        private Quaternion CreateRandom(float module = 1f) {
            return new Quaternion(
                Random.Range(-module, module),
                Random.Range(-module, module),
                Random.Range(-module, module),
                Random.Range(-module, module)
            );
        }

        private Quaternion Copy(in Quaternion value) {
            return new Quaternion(
                value.x,
                value.y,
                value.z,
                value.w
            );
        }
    }
}
using NuclearGames.Physics_LE.Utils.Mathematics;
using NUnit.Framework;
using UnityEngine;

namespace NuclearGames.Physics_LE.Tests.Tests.Runtime.Utils {
    public class MatrixTests {

#region Ctors

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(10.0015f)]
        public void CtorTest1(float value) {
            var matrix = new Matrix3x3(value);
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Assert.IsTrue(Mathf.Approximately(value, matrix[i, j]));
                }
            }
        }
        
        [TestCase(new float[]{0, 1, 2, 3, 4, 5, 6, 7, 8})]
        [TestCase(new float[]{10, 12, 23, 34, 45, 56, 67, 78, 89})]
        [TestCase(new float[]{10.001f, 12.002f, 23.003f, 34.004f, 45.005f, 56.006f, 67.007f, 78.008f, 89.009f})]
        public void CtorTest2(float[] values) {
            var matrix = new Matrix3x3(values[0],values[1],values[2],values[3],values[4],values[5],values[6],values[7], values[8]);
            
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Assert.IsTrue(Mathf.Approximately(values[i * 3 + j], matrix[i, j]));
                }
            }
        }

#endregion

        [Test]
        public void TestTranspose() {
            var matrix = CreateRandom(15);

            var matrixT = matrix.GetTranspose();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Assert.IsTrue(Mathf.Approximately(matrix[i, j], matrixT[j, i]));
                }
            }
        }


#region Operators
        
        [TestCase(new float[]{0, 1, 2, 3, 4, 5, 6, 7, 8})]
        [TestCase(new float[]{10, 12, 23, 34, 45, 56, 67, 78, 89})]
        [TestCase(new float[]{10.001f, 12.002f, 23.003f, 34.004f, 45.005f, 56.006f, 67.007f, 78.008f, 89.009f})]
        public void AddToRowTest(float[] values) {
            Vector3 additive = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));

            // заполняем матрицу
            var matrix = CreateFromBuffer(values);
            
            //Тестируем
            for (int row = 0; row < 3; row++) {
                matrix.AddToRow(row, additive);
                for (int col = 0; col < 3; col++) {
                    Assert.IsTrue(Mathf.Approximately(values[row * 3 + col] + additive[col], matrix[row, col]));
                }
            }
        }
        
        [TestCase(new float[]{0, 1, 2, 3, 4, 5, 6, 7, 8})]
        [TestCase(new float[]{10, 12, 23, 34, 45, 56, 67, 78, 89})]
        [TestCase(new float[]{10.001f, 12.002f, 23.003f, 34.004f, 45.005f, 56.006f, 67.007f, 78.008f, 89.009f})]
        public void AddToColumnTest(float[] values) {
            Vector3 additive = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));

            // заполняем матрицу
            var matrix = CreateFromBuffer(values);
            
            //Тестируем
            for (int col = 0; col < 3; col++) {
                matrix.AddToColumn(col, additive);
                for (int row = 0; row < 3; row++) {
                    Assert.IsTrue(Mathf.Approximately(values[row * 3 + col] + additive[row], matrix[row, col]));
                }
            }
        }

        [TestCase(new float[]{0, 1, 2, 3, 4, 5, 6, 7, 8})]
        [TestCase(new float[]{10, 12, 23, 34, 45, 56, 67, 78, 89})]
        [TestCase(new float[]{10.001f, 12.002f, 23.003f, 34.004f, 45.005f, 56.006f, 67.007f, 78.008f, 89.009f})]
        public void MultiplyRowTest(float[] values) {
            float multiplier = Random.Range(0, 100f);

            // заполняем матрицу
            var matrix = CreateFromBuffer(values);
            
            //Тестируем
            for (int i = 0; i < 3; i++) {
                matrix.MultiplyRow(i, multiplier);
                for (int j = 0; j < 3; j++) {
                    Assert.IsTrue(Mathf.Approximately(values[i * 3 + j] * multiplier, matrix[i, j]));
                }
            }
        }
        
        [TestCase(new float[]{0, 1, 2, 3, 4, 5, 6, 7, 8})]
        [TestCase(new float[]{10, 12, 23, 34, 45, 56, 67, 78, 89})]
        [TestCase(new float[]{10.001f, 12.002f, 23.003f, 34.004f, 45.005f, 56.006f, 67.007f, 78.008f, 89.009f})]
        public void MultiplyColumnTest(float[] values) {
            float multiplier = Random.Range(0, 100f);

            // заполняем матрицу
            var matrix = CreateFromBuffer(values);
            
            //Тестируем
            for (int j = 0; j < 3; j++) {
                matrix.MultiplyColumn(j, multiplier);
                for (int i = 0; i < 3; i++) {
                    Assert.IsTrue(Mathf.Approximately(values[i * 3 + j] * multiplier, matrix[i, j]));
                }
            }
        }

        [TestCase(new float[]{0, 1, 2, 3, 4, 5, 6, 7, 8})]
        [TestCase(new float[]{10, 12, 23, 34, 45, 56, 67, 78, 89})]
        [TestCase(new float[]{10.001f, 12.002f, 23.003f, 34.004f, 45.005f, 56.006f, 67.007f, 78.008f, 89.009f})]
        public void IncreaseMatrix(float[] values) {
            float multiplier = Random.Range(0, 100f);
            
            // заполняем матрицу
            var matrix = CreateFromBuffer(values);
            
            //Умножаем
            matrix.Increase(multiplier);
            
            //Тестируем
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Assert.IsTrue(Mathf.Approximately(values[i * 3 + j] * multiplier, matrix[i, j]));
                }
            }
        }

        [Test]
        public void TestMultiplyMatrix1() {
            var matrix1 = CreateRandom(100);
            var matrix2 = CreateRandom(100);

            var resultMatrix = matrix1 * matrix2;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Assert.IsTrue(Mathf.Approximately(matrix1[i,0] * matrix2[0,j] + matrix1[i,1] * matrix2[1,j] + matrix1[i,2] * matrix2[2,j], resultMatrix[i, j]));
                }
            }
        }
        
        [Test]
        public void TestMultiplyMatrix2() {
            var matrix1 = CreateRandom(100);
            var matrix2 = CreateRandom(100);
            var resultMatrix = matrix1 * matrix2;
            var resultMatrixT = resultMatrix.GetTranspose();


            var matrix1T = matrix1.GetTranspose();
            var matrix2T = matrix2.GetTranspose();
            var transposeResultMatrixT = matrix2T * matrix1T;
            
            
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Assert.IsTrue(Mathf.Approximately(resultMatrixT[i, j], transposeResultMatrixT[i, j]));
                }
            }
        }

#endregion


#region Utils

        private Matrix3x3 CreateFromBuffer(float[] values) {
            var matrix = new Matrix3x3(); 
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    matrix[i, j] = values[i * 3 + j];
                }
            }

            return matrix;
        }
        
        private Matrix3x3 CreateRandom(float maxModule) {
            var matrix = new Matrix3x3(); 
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    matrix[i, j] = Random.Range(0, maxModule);
                }
            }

            return matrix;
        }

#endregion
    }
}
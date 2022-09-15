using System;
using UnityEngine;

namespace NuclearGames.Physics_LE.Utils.Mathematics {
    public struct Matrix3x3 {
        private const int ROWS_CONT = 3;
        private const int COLUMNS_CONT = 3;
        private const int SIZE = ROWS_CONT * COLUMNS_CONT;

        private float _a0;
        private float _a1;
        private float _a2;
        private float _b0;
        private float _b1;
        private float _b2;
        private float _c0;
        private float _c1;
        private float _c2;

        public Matrix3x3(float value = 0) :
            this(value, value, value, value, value, value, value, value, value) { }

        public Matrix3x3(float a0, float a1, float a2, 
            float b0, float b1, float b2, 
            float c0, float c1, float c2) {
            _a0 = a0;
            _a1 = a1;
            _a2 = a2;
            _b0 = b0;
            _b1 = b1;
            _b2 = b2;
            _c0 = c0;
            _c1 = c1;
            _c2 = c2;
        }

        /// <summary>
        /// Возвращает/Устанавливает значение матрицы по индексу его строки и столбца
        /// </summary>
        public float this[int row, int col] {
            get => GetValue(row * COLUMNS_CONT + col);
            set => SetValue(row * COLUMNS_CONT + col, value);
        }

        /// <summary>
        /// Возвращает/Устанавливает значение матрицы по итоговому индексу
        /// </summary>
        public float this[int index] {
            get => GetValue(in index);
            set => SetValue(in index, value);
        }

        /// <summary>
        /// Возвращает транспонированную матрицу
        /// </summary>
        public Matrix3x3 GetTranspose() {
            return new Matrix3x3(_a0, _b0, _c0, 
                                 _a1, _b1, _c1, 
                                 _a2, _b2, _c2);
        }

#region Operators
        
        /// <summary>
        /// Добавляет к строке матрицы значение
        /// </summary>
        /// <param name="row">Индекс строки матрицы</param>
        /// <param name="value">Значение</param>
        public void AddToRow(in int row, in Vector3 value) {
            if (row < 0 || row > ROWS_CONT - 1) {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            for (int col = 0; col < COLUMNS_CONT; col++) {
                this[row, col] += value[col];
            }
        }
        
        /// <summary>
        /// Добавляет к колонке матрицы значение
        /// </summary>
        /// <param name="col">Индекс колонки матрицы</param>
        /// <param name="value">Значение</param>
        public void AddToColumn(in int col, in Vector3 value) {
            if (col < 0 || col > COLUMNS_CONT - 1) {
                throw new ArgumentOutOfRangeException(nameof(col));
            }

            for (int row = 0; row < ROWS_CONT; row++) {
                this[row, col] += value[row];
            }
        }

        /// <summary>
        /// Добавляет матрицу к текущей
        /// </summary>
        public void AddMatrix(Matrix3x3 matrix) {
            _a0 += matrix._a0;
            _a1 += matrix._a1;
            _a2 += matrix._a2;
            _b0 += matrix._b0;
            _b1 += matrix._b1;
            _b2 += matrix._b2;
            _c0 += matrix._c0;
            _c1 += matrix._c1;
            _c2 += matrix._c2;
        }

        /// <summary>
        /// Умножает строку матрицы на значение
        /// </summary>
        /// <param name="row">Индекс строки матрицы</param>
        /// <param name="value">Значение</param>
        public void MultiplyRow(in int row, in float value) {
            if (row < 0 || row > ROWS_CONT - 1) {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            for (int col = 0; col < COLUMNS_CONT; col++) {
                this[row, col] *= value;
            }
        }
        
        /// <summary>
        /// Умножает колонку матрицы на значение
        /// </summary>
        /// <param name="col">Индекс колонки матрицы</param>
        /// <param name="value">Значение</param>
        public void MultiplyColumn(in int col, in float value) {
            if (col < 0 || col > COLUMNS_CONT - 1) {
                throw new ArgumentOutOfRangeException(nameof(col));
            }

            for (int row = 0; row < ROWS_CONT; row++) {
                this[row, col] *= value;
            }
        }

        /// <summary>
        /// Умножает матрицу на число
        /// </summary>
        /// <param name="value"></param>
        public void Increase(in float value) {
            _a0 *= value;
            _a1 *= value;
            _a2 *= value;
            _b0 *= value;
            _b1 *= value;
            _b2 *= value;
            _c0 *= value;
            _c1 *= value;
            _c2 *= value;
        }

        public static Vector3 operator *(Matrix3x3 matrix, Vector3 vector) {
            return new Vector3(
                matrix._a0 * vector.x + matrix._a1 * vector.y + matrix._a2 * vector.z,
                matrix._b0 * vector.x + matrix._b1 * vector.y + matrix._b2 * vector.z,
                matrix._c0 * vector.x + matrix._c1 * vector.y + matrix._c2 * vector.z
            );
        }
        
        public static Matrix3x3 operator *(Matrix3x3 matrix1, Matrix3x3 matrix2) {
            return new Matrix3x3(
                (matrix1._a0 * matrix2._a0) + (matrix1._a1 * matrix2._b0) + (matrix1._a2 * matrix2._c0),
                (matrix1._a0 * matrix2._a1) + (matrix1._a1 * matrix2._b1) + (matrix1._a2 * matrix2._c1),
                (matrix1._a0 * matrix2._a2) + (matrix1._a1 * matrix2._b2) + (matrix1._a2 * matrix2._c2),

                (matrix1._b0 * matrix2._a0) + (matrix1._b1 * matrix2._b0) + (matrix1._b2 * matrix2._c0),
                (matrix1._b0 * matrix2._a1) + (matrix1._b1 * matrix2._b1) + (matrix1._b2 * matrix2._c1),
                (matrix1._b0 * matrix2._a2) + (matrix1._b1 * matrix2._b2) + (matrix1._b2 * matrix2._c2),

                (matrix1._c0 * matrix2._a0) + (matrix1._c1 * matrix2._b0) + (matrix1._c2 * matrix2._c0),
                (matrix1._c0 * matrix2._a1) + (matrix1._c1 * matrix2._b1) + (matrix1._c2 * matrix2._c1),
                (matrix1._c0 * matrix2._a2) + (matrix1._c1 * matrix2._b2) + (matrix1._c2 * matrix2._c2)
            );
        }
        
        public static Matrix3x3 operator + (Matrix3x3 matrix1, Matrix3x3 matrix2) {
            return new Matrix3x3(
                (matrix1._a0 * matrix2._a0) + (matrix1._a1 * matrix2._b0) + (matrix1._a2 * matrix2._c0),
                (matrix1._a0 * matrix2._a1) + (matrix1._a1 * matrix2._b1) + (matrix1._a2 * matrix2._c1),
                (matrix1._a0 * matrix2._a2) + (matrix1._a1 * matrix2._b2) + (matrix1._a2 * matrix2._c2),

                (matrix1._b0 * matrix2._a0) + (matrix1._b1 * matrix2._b0) + (matrix1._b2 * matrix2._c0),
                (matrix1._b0 * matrix2._a1) + (matrix1._b1 * matrix2._b1) + (matrix1._b2 * matrix2._c1),
                (matrix1._b0 * matrix2._a2) + (matrix1._b1 * matrix2._b2) + (matrix1._b2 * matrix2._c2),

                (matrix1._c0 * matrix2._a0) + (matrix1._c1 * matrix2._b0) + (matrix1._c2 * matrix2._c0),
                (matrix1._c0 * matrix2._a1) + (matrix1._c1 * matrix2._b1) + (matrix1._c2 * matrix2._c1),
                (matrix1._c0 * matrix2._a2) + (matrix1._c1 * matrix2._b2) + (matrix1._c2 * matrix2._c2)
            );
        }


#endregion

#region Utils

        /// <summary>
        /// Возвращает значение по итоговому индексу 
        /// </summary>
        private float GetValue(in int index) {
            return index switch {
                0 => _a0,
                1 => _a1,
                2 => _a2,
                3 => _b0,
                4 => _b1,
                5 => _b2,
                6 => _c0,
                7 => _c1,
                8 => _c2,
                _ => throw new ArgumentException(nameof(index))
            };
        }

        /// <summary>
        /// Устанавливает значение по итоговому индексу 
        /// </summary>
        private void SetValue(in int index, in float value) {
            switch (index) {
                case 0: _a0 = value; break;
                case 1: _a1 = value; break;
                case 2: _a2 = value; break;
                case 3: _b0 = value; break;
                case 4: _b1 = value;break;
                case 5: _b2 = value; break;
                case 6: _c0 = value; break;
                case 7: _c1 = value; break;
                case 8: _c2 = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

#endregion
    }
}
using OpenTK;


namespace FirimenticEngine.Math
{
    /// <summary>Represents a 3 row by 3 column Matrix.</summary>
    public struct Matrix3
    {
        /// <summary>Holds the values in the 1st row of the Matrix.</summary>
        public Vector3 Row1;
        /// <summary>Holds the values in the 2nd row of the Matrix.</summary>
        public Vector3 Row2;
        /// <summary>Holds the values in the 3rd row of the Matrix.</summary>
        public Vector3 Row3;


        /// <summary>
        /// Crates a new Matrix3.
        /// </summary>
        /// <param name="m11">The value for row 1 column 1 of the Matrix.</param>
        /// <param name="m12">The value for row 1 column 2 of the Matrix.</param>
        /// <param name="m13">The value for row 1 column 3 of the Matrix.</param>
        /// <param name="m21">The value for row 2 column 1 of the Matrix.</param>
        /// <param name="m22">The value for row 2 column 2 of the Matrix.</param>
        /// <param name="m23">The value for row 2 column 3 of the Matrix.</param>
        /// <param name="m31">The value for row 3 column 1 of the Matrix.</param>
        /// <param name="m32">The value for row 3 column 2 of the Matrix.</param>
        /// <param name="m33">The value for row 3 column 3 of the Matrix.</param>
        public Matrix3(float m11, float m12, float m13,
                       float m21, float m22, float m23,
                       float m31, float m32, float m33) {
            this.Row1 = new Vector3(m11, m12, m13);
            this.Row2 = new Vector3(m21, m22, m23);
            this.Row3 = new Vector3(m31, m32, m33);
        }

        /// <summary>
        /// Crates a new Matrix3.
        /// </summary>
        /// <param name="row1">The values for row 1 of the Matrix.</param>
        /// <param name="row2">The values for row 2 of the Matrix.</param>
        /// <param name="row3">The values for row 3 of the Matrix.</param>
        public Matrix3(Vector3 row1, Vector3 row2, Vector3 row3) {
            this.Row1 = row1;
            this.Row2 = row2;
            this.Row3 = row3;
        }


        public static readonly Matrix3 Zero = new Matrix3();
        public static readonly Matrix3 Identity = new Matrix3(1.0f, 0.0f, 0.0f,
                                                              0.0f, 1.0f, 0.0f,
                                                              0.0f, 0.0f, 1.0f
        );


        public static Matrix3 operator *(Matrix3 transform, Matrix3 transformBy) {
            Mult(ref transform, ref transformBy, out Matrix3 result);
            return result;
        }


        /// <summary>
        /// Multiplies two matrices from right to left so that left hand transformations are applied before right hand ones.
        /// </summary>
        /// <param name="trans">The Matrix to be transformed (multiplied by).</param>
        /// <param name="transBy">The matrix to tranform by (be multiplied).</param>
        /// <param name="result">The matrix to be the result of the multiplication.</param>
        public static void Mult(ref Matrix3 trans, ref Matrix3 transBy, out Matrix3 result) {
            result.Row1.X = transBy.Row1.X * trans.Row1.X + transBy.Row1.Y * trans.Row2.X + transBy.Row1.Z * trans.Row3.X;
            result.Row1.Y = transBy.Row1.X * trans.Row1.Y + transBy.Row1.Y * trans.Row2.Y + transBy.Row1.Z * trans.Row3.Y;
            result.Row1.Z = transBy.Row1.X * trans.Row1.Z + transBy.Row1.Y * trans.Row2.Z + transBy.Row1.Z * trans.Row3.Z;
            
            result.Row2.X = transBy.Row2.X * trans.Row1.X + transBy.Row2.Y * trans.Row2.X + transBy.Row2.Z * trans.Row3.X;
            result.Row2.Y = transBy.Row2.X * trans.Row1.Y + transBy.Row2.Y * trans.Row2.Y + transBy.Row2.Z * trans.Row3.Y;
            result.Row2.Z = transBy.Row2.X * trans.Row1.Z + transBy.Row2.Y * trans.Row2.Z + transBy.Row2.Z * trans.Row3.Z;

            result.Row3.X = transBy.Row3.X * trans.Row1.X + transBy.Row3.Y * trans.Row2.X + transBy.Row3.Z * trans.Row3.X;
            result.Row3.Y = transBy.Row3.X * trans.Row1.Y + transBy.Row3.Y * trans.Row2.Y + transBy.Row3.Z * trans.Row3.Y;
            result.Row3.Z = transBy.Row3.X * trans.Row1.Z + transBy.Row3.Y * trans.Row2.Z + transBy.Row3.Z * trans.Row3.Z;
        }
    }
}

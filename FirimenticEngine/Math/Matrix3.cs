

using OpenTK;


namespace FirimenticEngine.Math
{
    /// <summary>
    /// Represents a 3 row by 3 column Matrix.
    /// </summary>
    public struct Matrix3
    {
        /// <summary>
        /// Holds the values in the 1st row of the matrix
        /// </summary>
        public Vector3 Row1;
        /// <summary>
        /// Holds the values in the 2nd row of the matrix
        /// </summary>
        public Vector3 Row2;
        /// <summary>
        /// Holds the values in the 3rd row of the matrix
        /// </summary>
        public Vector3 Row3;
        

        public Matrix3(float m11, float m12, float m13,
                       float m21, float m22, float m23,
                       float m31, float m32, float m33) {
            this.Row1 = new Vector3(m11, m12, m13);
            this.Row2 = new Vector3(m21, m22, m23);
            this.Row3 = new Vector3(m31, m32, m33);
        }

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

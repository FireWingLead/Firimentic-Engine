using OpenTK;

namespace FirimenticEngine.Math
{
    /// <summary>Represents a 1 row by 4 column Matrix.</summary>
    public class Matrix1x4
    {
        /// <summary>Holds the values in the first row of the Matrix.</summary>
        Vector4 Row1;


        /// <summary>
        /// Creates a new Matrix1x4.
        /// </summary>
        /// <param name="m11">The value for row 1 column 1 of the matrix.</param>
        /// <param name="m12">The value for row 1 column 2 of the matrix.</param>
        /// <param name="m13">The value for row 1 column 3 of the matrix.</param>
        /// <param name="m14">The value for row 1 column 4 of the matrix.</param>
        public Matrix1x4(float m11, float m12, float m13, float m14) {
            Row1 = new Vector4(m11, m12, m13, m14);
        }


        public static implicit operator Matrix1x4(Vector4 from) { return new Matrix1x4(from.X, from.Y, from.Z, from.W); }
        public static implicit operator Matrix1x4(Vector3 from) { return new Matrix1x4(from.X, from.Y, from.Z, 0.0f); }

        public static implicit operator Vector4(Matrix1x4 from) { return new Vector4(from.Row1.X, from.Row1.Y, from.Row1.Z, from.Row1.W); }
        public static implicit operator Vector3(Matrix1x4 from) { return new Vector3(from.Row1.X, from.Row1.Y, from.Row1.Z); }
    }
}

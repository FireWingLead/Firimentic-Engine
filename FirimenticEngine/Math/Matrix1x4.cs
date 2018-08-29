using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirimenticEngine.Math
{
    /// <summary>
    /// Represents a 4 row by 1 column Matrix.
    /// </summary>
    public struct Matrix4x1
    {
        /// <summary>
        /// 
        /// </summary>
        public float M11;
        public float M21;
        public float M31;
        public float M41;


        public Matrix4x1(float M11, float M21, float M31, float M41) {
            this.M11 = M11;
            this.M21 = M21;
            this.M31 = M31;
            this.M41 = M41;
        }


        public static implicit operator Matrix4x1(Vector4 from) { return new Matrix4x1(from.X, from.Y, from.Z, from.W); }
        public static implicit operator Matrix4x1(Vector3 from) { return new Matrix4x1(from.X, from.Y, from.Z, 1.0f); }

        public static implicit operator Vector4(Matrix4x1 from) { return new Vector4(from.M11, from.M21, from.M31, from.M41); }
        public static implicit operator Vector3(Matrix4x1 from) { return new Vector3(from.M11, from.M21, from.M31); }


        public static Matrix4x1 operator *(Matrix4x1 target, Matrix4 transformBy) {
            return new Matrix4x1(
                target.M11 * transformBy.M11 + target.M21 * transformBy.M12 + target.M31 * transformBy.M13 + target.M41 * transformBy.M14,
                target.M11 * transformBy.M21 + target.M21 * transformBy.M22 + target.M31 * transformBy.M23 + target.M41 * transformBy.M24,
                target.M11 * transformBy.M31 + target.M21 * transformBy.M32 + target.M31 * transformBy.M33 + target.M41 * transformBy.M34,
                target.M11 * transformBy.M41 + target.M21 * transformBy.M42 + target.M31 * transformBy.M43 + target.M41 * transformBy.M44
            );
        }
    }
}

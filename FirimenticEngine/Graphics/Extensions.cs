
using OpenTK;

namespace FirimenticEngine.Graphics
{
    public static class Extensions
    {
        public static string CamelCase(this string str) {
            if (string.IsNullOrEmpty(str)) return str;
            if (str.Length > 1)
                return char.ToLower(str[0]) + str.Substring(1);
            else
                return str.ToLower();
        }

        //public static Vector3 Transform(this Vector3 vec, ref Matrix4 transformationMatrix) {
        //    return (transformationMatrix * new Vector4(vec.X, vec.Y, vec.Z, 1.0f)).Xyz;
        //}
    }
}

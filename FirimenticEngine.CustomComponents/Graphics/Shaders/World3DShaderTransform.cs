
using OpenTK;

using FirimenticEngine.Graphics.Shaders;


namespace FirimenticEngine.CustomComponents.Graphics.Shaders
{
    public class World3DShaderTransform : ShaderTransform
    {
        public World3DShaderTransform(Matrix4 world) {
            this.world = world;
        }

        public World3DShaderTransform() : this(Matrix4.Identity) { }

        
        private Matrix4 world;
        public Matrix4 World {
            get { return world; }
            set { world = value; }
        }

        public void SetWorld(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44
            ) {
            world.Row0 = new Vector4(m11, m12, m13, m14);
            world.Row0 = new Vector4(m21, m22, m23, m24);
            world.Row0 = new Vector4(m31, m32, m33, m34);
            world.Row0 = new Vector4(m41, m42, m43, m44);
        }

        public void MultWorld(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44
            ) {
            world *= new Matrix4(
                new Vector4(m11, m12, m13, m14),
                new Vector4(m21, m22, m23, m24),
                new Vector4(m31, m32, m33, m34),
                new Vector4(m41, m42, m43, m44)
            );
        }


        public override void ApplyToShader(ShaderProgram shader) {
            shader.Transform3D.World = world;
        }
    }
}

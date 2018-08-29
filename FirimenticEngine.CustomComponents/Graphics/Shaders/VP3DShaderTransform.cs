
using OpenTK;

using FirimenticEngine.Graphics.Shaders;


namespace FirimenticEngine.CustomComponents.Graphics.Shaders
{
     public class VP3DShaderTransform : ShaderTransform
    {
        public VP3DShaderTransform(Matrix4 view, Matrix4 proj) {
            this.view = view;
            this.projection = proj;
        }

        public VP3DShaderTransform() : this(Matrix4.Identity, Matrix4.Identity) { }


        private Matrix4 view;
        public Matrix4 View {
            get { return view; }
            set { view = value; }
        }

        public void SetView(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44
            ) {
            view.Row0 = new Vector4(m11, m12, m13, m14);
            view.Row0 = new Vector4(m21, m22, m23, m24);
            view.Row0 = new Vector4(m31, m32, m33, m34);
            view.Row0 = new Vector4(m41, m42, m43, m44);
        }

        private Matrix4 projection;
        public Matrix4 Projection {
            get { return projection; }
            set { projection = value; }
        }

        public void SetProjection(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44
            ) {
            projection.Row0 = new Vector4(m11, m12, m13, m14);
            projection.Row0 = new Vector4(m21, m22, m23, m24);
            projection.Row0 = new Vector4(m31, m32, m33, m34);
            projection.Row0 = new Vector4(m41, m42, m43, m44);
        }


        public override void ApplyToShader(ShaderProgram shader) {
            shader.Transform3D.View = view;
            shader.Transform3D.Projection = projection;
        }
    }
}

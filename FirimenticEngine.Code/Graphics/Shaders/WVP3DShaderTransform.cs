
using OpenTK;


namespace FirimenticEngine.Graphics.Shaders
{
    public class WVP3DShaderTransform : ShaderTransform
    {
        private bool recalculate;

        public WVP3DShaderTransform(Matrix4 world, Matrix4 view, Matrix4 proj) {
            this.world = world;
            this.view = view;
            this.projection = proj;
            recalculate = true;
        }

        public WVP3DShaderTransform() : this(Matrix4.Identity, Matrix4.Identity, Matrix4.Identity) { }


        private Matrix4 world;
        public Matrix4 World {
            get { return world; }
            set { world = value; recalculate = true; }
        }

        private Matrix4 view;
        public Matrix4 View {
            get { return view; }
            set { view = value; recalculate = true; }
        }

        private Matrix4 projection;
        public Matrix4 Projection {
            get { return projection; }
            set { projection = value; recalculate = true; }
        }

        public Matrix4 WorldViewProjection;


        public void Recalculate() {
            if (recalculate) {
                Matrix4.Mult(ref world, ref view, out Matrix4 worldView);
                Matrix4.Mult(ref worldView, ref projection, out WorldViewProjection);
                recalculate = false;
            }
        }

        public override void ApplyToShader(ShaderProgram shader) {
            shader.Transform3D.world = world;
            shader.Transform3D.view = view;
            shader.Transform3D.projection = projection;
            shader.Transform3D.recalculate = true;
        }
    }
}

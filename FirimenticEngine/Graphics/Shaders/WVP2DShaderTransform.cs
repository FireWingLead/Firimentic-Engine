
using OpenTK;

namespace FirimenticEngine.Graphics.Shaders
{
    public class WVP2DShaderTransform : ShaderTransform
    {
        private bool recalculate;

        public WVP2DShaderTransform(Matrix3 world, Matrix3 view, Matrix3 proj) {
            this.world = world;
            this.view = view;
            this.projection = proj;
            recalculate = true;
        }

        public WVP2DShaderTransform() : this(Matrix3.Identity, Matrix3.Identity, Matrix3.Identity) { }


        public Matrix3 world;
        public Matrix3 World {
            get { return world; }
            set { world = value; recalculate = true; }
        }

        public Matrix3 view;
        public Matrix3 View {
            get { return view; }
            set { view = value; recalculate = true; }
        }

        public Matrix3 projection;
        public Matrix3 Projection {
            get { return projection; }
            set { projection = value; recalculate = true; }
        }

        internal Matrix3 WorldViewProjection;


        public void Recalculate() {
            if (recalculate) {
                Matrix3.Mult(ref world, ref view, out Matrix3 worldView);
                Matrix3.Mult(ref worldView, ref projection, out WorldViewProjection);
                recalculate = false;
            }
        }
        
        public override void ApplyToShader(ShaderProgram shader) {
            shader.Transform2D.world = world;
            shader.Transform2D.view = view;
            shader.Transform2D.projection = projection;
            shader.Transform2D.recalculate = true;
        }
    }
}

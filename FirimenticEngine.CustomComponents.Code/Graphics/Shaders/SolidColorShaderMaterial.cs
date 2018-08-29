using OpenTK;

using FirimenticEngine.Graphics.Shaders;


namespace FirimenticEngine.CustomComponents.Graphics.Shaders
{
    public class SolidColorShaderMaterial : ShaderMaterial
    {
        const int COLOR_INDEX = 0;


        public SolidColorShaderMaterial(Vector4 color) : base(1) {
            parts[COLOR_INDEX] = new MaterialColor(color);
        }

        public SolidColorShaderMaterial() : this(new Vector4()) { }


        public Vector4 Color {
            get { return ((MaterialColor)parts[COLOR_INDEX]).Color; }
            set { ((MaterialColor)parts[COLOR_INDEX]).Color = value; }
        }

        public void SetColor(float r, float g, float b, float a) { Color = new Vector4(r, g, b, a); }
    }
}

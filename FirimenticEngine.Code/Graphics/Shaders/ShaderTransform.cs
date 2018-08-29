
namespace FirimenticEngine.Graphics.Shaders
{
    public class ShaderTransform
    {
        public static readonly ShaderTransform Empty = new ShaderTransform();

        protected ShaderTransform() { }

        public virtual void ApplyToShader(ShaderProgram shader) { }
    }
}

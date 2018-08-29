namespace FirimenticEngine.Graphics.Shaders
{
    public enum UniformParts
    {
        Color = 0, Texture, BumpMap, SpecularColor, SpecularPower, SpecularMap, AmbientColor,
        World, View, Projection, WorldView, WorldViewProjection
    }



    public abstract class UniformPart
    {
        protected UniformPart(UniformParts partCode) {
            PartCode = partCode;
        }

        public UniformParts PartCode { get; private set; }

        public abstract void ApplyToShader(ShaderProgram shader);
    }
}

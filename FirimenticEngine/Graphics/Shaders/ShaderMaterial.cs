
using System.Collections;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.ES30;

namespace FirimenticEngine.Graphics.Shaders
{
    public enum MaterialParts
    {
        Color = UniformParts.Color,
        Texture = UniformParts.Texture,
        BumpMap = UniformParts.BumpMap,
        SpecularColor = UniformParts.SpecularColor,
        SpecularPower = UniformParts.SpecularPower,
        SpecularMap = UniformParts.SpecularMap,
    }



    public abstract class MaterialPart : UniformPart
    {
        protected MaterialPart(MaterialParts partCode) : base((UniformParts)partCode) { }
    }



    public class MaterialColor : MaterialPart
    {
        public MaterialColor(Vector4 color) : base(MaterialParts.Color) {
            Color = color;
        }

        public Vector4 Color;

        public override void ApplyToShader(ShaderProgram shader) {
            GL.Uniform4(shader.UniformLocations[(int)PartCode], Color.X, Color.Y, Color.Z, Color.W);
        }
    }



    public class ShaderMaterial : IEnumerable<MaterialPart>
    {
        public static readonly ShaderMaterial Empty = new ShaderMaterial(0);

        protected MaterialPart[] parts;

        protected ShaderMaterial(int numParts) {
            parts = new MaterialPart[numParts];
        }

        public IEnumerator<MaterialPart> GetEnumerator() { return ((IEnumerable<MaterialPart>)parts).GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return ((IEnumerable<MaterialPart>)parts).GetEnumerator(); }
    }
}

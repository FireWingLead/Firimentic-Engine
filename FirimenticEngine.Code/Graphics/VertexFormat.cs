using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.ES30;


namespace FirimenticEngine.Graphics
{
    public enum VertexParts
    {
        Position, Normal, Color, TexCoords, Tex2Coords
    }



    public interface IVertex { }
    public interface IVertexPositioned : IVertex { Vector3 Position { get; set; } }
    public interface IVertexNormaled : IVertex { Vector3 Normal { get; set; } }
    public interface IVertexColored : IVertex { Vector4 Color { get; set; } }
    public interface IVertexTextured : IVertex { Vector2 TexCoords { get; set; } }
    public interface IVertexDoubleTextured : IVertex { Vector2 Tex2Coords { get; set; } }



    public sealed class VertexPart {
        //Position, Normal, Color
        public static readonly VertexPart Position = new VertexPart(VertexAttribPointerType.Float, 3, 3 * sizeof(float), VertexParts.Position, Enum.GetName(typeof(VertexParts), VertexParts.Position).CamelCase());
        public static readonly VertexPart Normal = new VertexPart(VertexAttribPointerType.Float, 3, 3 * sizeof(float), VertexParts.Normal, Enum.GetName(typeof(VertexParts), VertexParts.Normal).CamelCase());
        public static readonly VertexPart Color = new VertexPart(VertexAttribPointerType.Float, 4, 4 * sizeof(float), VertexParts.Color, Enum.GetName(typeof(VertexParts), VertexParts.Color).CamelCase());
        public static readonly VertexPart TexCoords = new VertexPart(VertexAttribPointerType.Float, 2, 2 * sizeof(float), VertexParts.TexCoords, Enum.GetName(typeof(VertexParts), VertexParts.TexCoords).CamelCase());
        public static readonly VertexPart Tex2Coords = new VertexPart(VertexAttribPointerType.Float, 2, 2 * sizeof(float), VertexParts.Tex2Coords, Enum.GetName(typeof(VertexParts), VertexParts.Tex2Coords).CamelCase());

        public static readonly IReadOnlyDictionary<string, VertexPart> PartTypessByAttributeName = new Dictionary<string, VertexPart> {
            { Position.AttributeName, Position },
            { Normal.AttributeName, Normal },
            { Color.AttributeName, Color },
            { TexCoords.AttributeName, TexCoords },
            { Tex2Coords.AttributeName, Tex2Coords }
        };



        public VertexAttribPointerType ElementType { get; private set; }
        public int SizeInElements { get; private set; }
        public int SizeInBytes { get; private set; }
        public VertexParts PartCode { get; private set; }
        public string AttributeName { get; private set; }

        public int OffsetIntoVertexInBytes { get; private set; }

        private VertexPart(VertexAttribPointerType elementType, int sizeInElements, int sizeInBytes, VertexParts partCode, string attributeName) {
            ElementType = elementType;
            SizeInElements = sizeInElements;
            SizeInBytes = sizeInBytes;
            PartCode = partCode;
            AttributeName = attributeName;
        }

        internal VertexPart(VertexPart partType, int offsetIntoVertexInBytes)
            : this(partType.ElementType, partType.SizeInElements, partType.SizeInBytes, partType.PartCode, partType.AttributeName) {
            OffsetIntoVertexInBytes = offsetIntoVertexInBytes;
        }
    }



    public sealed class VertexFormat
    {
        public static readonly VertexFormat Position = new VertexFormat(new VertexPart[] {
            new VertexPart(VertexPart.Position, 0)
        });
        public static readonly VertexFormat PositionColor = new VertexFormat(new VertexPart[] {
            new VertexPart(VertexPart.Position, 0),
            new VertexPart(VertexPart.Color, VertexPart.Position.SizeInBytes)
        });
        public static readonly VertexFormat PositionNormal = new VertexFormat(new VertexPart[] {
            new VertexPart(VertexPart.Position, 0),
            new VertexPart(VertexPart.Normal, VertexPart.Position.SizeInBytes)
        });
        public static readonly VertexFormat PositionNormalColor = new VertexFormat(new VertexPart[] {
            new VertexPart(VertexPart.Position, 0),
            new VertexPart(VertexPart.Normal, VertexPart.Position.SizeInBytes),
            new VertexPart(VertexPart.Color, VertexPart.Normal.OffsetIntoVertexInBytes + VertexPart.Normal.SizeInBytes)
        });
        public static readonly VertexFormat PositionTexture = new VertexFormat(new VertexPart[] {
            new VertexPart(VertexPart.Position, 0),
            new VertexPart(VertexPart.TexCoords, VertexPart.Position.SizeInBytes)
        });



        public IReadOnlyList<VertexPart> Parts { get; private set; }
        public int BytesPerVertex { get; private set; }

        internal VertexFormat(VertexPart[] parts) {
            Parts = new List<VertexPart>(parts);
            BytesPerVertex = 0;
            foreach (VertexPart part in parts)
                BytesPerVertex += part.SizeInBytes;
        }
    }
}

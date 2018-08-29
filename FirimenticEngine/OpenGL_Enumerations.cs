using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirimenticEngine
{
    public enum PrimitiveType
    {
        Triangles = All.Triangles,
        TriangleStrip = All.TriangleStrip,
        TriangleFan = All.TriangleFan,
        Lines = All.Lines,
        LineStrip = All.LineStrip,
        LineLoop = All.LineLoop,
        Points = All.Points
    }

    public enum BufferTarget
    {
        ArrayBuffer = All.ArrayBuffer,
        ElementArrayBuffer = All.ElementArrayBuffer
    }

    public enum BufferUsageHint
    {
        StaticDraw = All.StaticDraw
    }

    public enum ShaderType
    {
        VertexShader = All.VertexShader,
        FragmentShader = All.FragmentShader
    }

    public enum GetProgramParameterName
    {
        LinkStatus = All.LinkStatus,
        ActiveAttributes = All.ActiveAttributes,
        ActiveUniforms = All.ActiveUniforms
    }
}

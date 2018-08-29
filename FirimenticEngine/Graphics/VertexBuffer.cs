
using System;

using OpenTK.Graphics.ES30;
using FirimenticEngine.Graphics.Shaders;


namespace FirimenticEngine.Graphics
{
    public class VertexBuffer : IDisposable
    {
        public int Handle { get; private set; }

        public VertexFormat VertexFormat { get; private set; }

        public ItemDisposalLevel DisposalLevel { get; set; }



        public VertexBuffer() {
            Handle = GL.GenBuffer();
        }

        public void Dispose() {
            GL.DeleteBuffer(Handle);
            Handle = Constants.NULL;
            if (ShaderProgram.CurrentBoundVBuffer == this)
                ShaderProgram.CurrentBoundVBuffer = null;
        }



        public void SetData<VertexType>(VertexFormat vertFmt, VertexType[] vertices) where VertexType : struct, IVertex {
            VertexFormat = vertFmt;
            VertexBuffer lastBoundVBuffer = ShaderProgram.CurrentBoundVBuffer;
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * vertFmt.BytesPerVertex, vertices, BufferUsageHint.StaticDraw);
            if (lastBoundVBuffer != this)
                ShaderProgram.UseVertexBuffer(this);
        }
    }
}

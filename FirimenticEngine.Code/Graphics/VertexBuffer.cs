
using System;

using OpenTK.Graphics.ES30;
using FirimenticEngine.Graphics.Shaders;


namespace FirimenticEngine.Graphics
{
    public class VertexBuffer : IDisposable
    {
        int handle;
        public int Handle { get { return handle; } private set { handle = value; } }

        public VertexFormat VertexFormat { get; private set; }

        public ItemDisposalLevel DisposalLevel { get; set; }



        public VertexBuffer() {
            GL.GenBuffers(1, out handle);
        }

        public void Dispose() {
            GL.DeleteBuffers(1, ref handle);
            handle = Constants.NULL;
            if (ShaderProgram.CurrentBoundVBuffer == this)
                ShaderProgram.CurrentBoundVBuffer = null;
        }



        public void SetData<VertexType>(VertexFormat vertFmt, VertexType[] vertices) where VertexType : struct, IVertex {
            VertexFormat = vertFmt;
            VertexBuffer lastBoundVBuffer = ShaderProgram.CurrentBoundVBuffer;
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * vertFmt.BytesPerVertex), vertices, BufferUsage.StaticDraw);
            if (lastBoundVBuffer != this)
                ShaderProgram.UseVertexBuffer(this);
        }
    }
}

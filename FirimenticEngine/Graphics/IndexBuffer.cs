
using System;

using OpenTK.Graphics.ES30;

using FirimenticEngine.Graphics.Shaders;


namespace FirimenticEngine.Graphics
{
    public class IndexBuffer : IDisposable
    {
        private int handle;
        public int Handle { get; private set; }

        public ItemDisposalLevel DisposalLevel { get; set; }



        public IndexBuffer() {
            Handle = GL.GenBuffer();
        }

        public void Dispose() {
            GL.DeleteBuffer(Handle);
            if (ShaderProgram.CurrentBoundIBuffer == this)
                ShaderProgram.CurrentBoundIBuffer = null;
        }



        public void SetData(ushort[] indices) {
            IndexBuffer lastBoundIBuffer = ShaderProgram.CurrentBoundIBuffer;
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(ushort), indices, BufferUsageHint.StaticDraw);
            if (lastBoundIBuffer != this)
                ShaderProgram.UseIndexBuffer(lastBoundIBuffer);
        }
    }
}

using System;

using OpenTK.Graphics.ES30;

using FirimenticEngine.Graphics.Shaders;


namespace FirimenticEngine.Graphics
{
    public class IndexBuffer : IDisposable
    {
        private int handle;
        public int Handle { get { return handle; } private set { handle = value; } }

        public ItemDisposalLevel DisposalLevel { get; set; }



        public IndexBuffer() {
            GL.GenBuffers(1, out handle);
        }

        public void Dispose() {
            GL.DeleteBuffers(1, ref handle);
            handle = Constants.NULL;
            if (ShaderProgram.CurrentBoundIBuffer == this)
                ShaderProgram.CurrentBoundIBuffer = null;
        }



        public void SetData(ushort[] indices) {
            IndexBuffer lastBoundIBuffer = ShaderProgram.CurrentBoundIBuffer;
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(ushort)), indices, BufferUsage.StaticDraw);
            if (lastBoundIBuffer != this)
                ShaderProgram.UseIndexBuffer(lastBoundIBuffer);
        }
    }
}

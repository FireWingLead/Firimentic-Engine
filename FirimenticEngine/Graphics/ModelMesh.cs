
using System;
using System.Collections.Generic;

using FirimenticEngine.Graphics.Shaders;


namespace FirimenticEngine.Graphics
{
    public class ModelMesh : IDisposable
    {
        public List<ModelMeshPart> Parts { get; } = new List<ModelMeshPart>();

        public ShaderTransform Transform { get; set; }

        public ShaderProgram DefaultShader { get; set; }
        public VertexBuffer DefaultVertexBuffer { get; set; }
        public ShaderMaterial DefaultMaterial { get; set; }
        public IndexBuffer IndexBuffer { get; set; }



        public void Dispose() {
            if (DefaultVertexBuffer?.DisposalLevel == ItemDisposalLevel.ModelMesh)
                DefaultVertexBuffer.Dispose();
            DefaultVertexBuffer = null;
            if (IndexBuffer?.DisposalLevel == ItemDisposalLevel.ModelMesh)
                IndexBuffer.Dispose();
            IndexBuffer = null;

            foreach (ModelMeshPart part in Parts)
                part.Dispose();
            Parts.Clear();
        }



        public void Draw() {
            if (DefaultShader != null && DefaultShader.Use()) {
                ShaderProgram.UseVertexBuffer(DefaultVertexBuffer);
            }
            else {
                if (ShaderProgram.CurrentBoundVBuffer != DefaultVertexBuffer)
                    ShaderProgram.UseVertexBuffer(DefaultVertexBuffer);
            }
            ShaderProgram.UseIndexBuffer(IndexBuffer);
            Transform.ApplyToShader(ShaderProgram.CurrentInUseShader);
            ShaderProgram.CurrentInUseShader.FinalizeTransformations();

            foreach (ModelMeshPart part in Parts) {
                part.Draw();
            }
        }
    }
}

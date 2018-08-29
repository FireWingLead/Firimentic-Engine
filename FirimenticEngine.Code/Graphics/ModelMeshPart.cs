
using System;
using System.Collections.Generic;

using OpenTK.Graphics.ES30;

using FirimenticEngine.Graphics.Shaders;


namespace FirimenticEngine.Graphics
{
    public class ModelMeshPart : IDisposable
    {
        public ModelMesh ParentMesh { get; set; }
        
        public ShaderProgram Shader { get; set; }
        
        public VertexBuffer VertexBuffer { get; set; }

        public ShaderMaterial Material { get; set; }
        
        public ModelMeshPartPrimCall[] PrimitiveCalls { get; set; }



        public ModelMeshPart(ModelMesh parentMesh) {
            ParentMesh = parentMesh;
            ParentMesh.Parts.Add(this);
            Shader = parentMesh.DefaultShader;
            VertexBuffer = parentMesh.DefaultVertexBuffer;
            Material = parentMesh.DefaultMaterial;
        }

        public ModelMeshPart(ModelMesh parentMesh, ShaderProgram shader, VertexBuffer vBuffer, ShaderMaterial material, ModelMeshPartPrimCall[] primCalls) : this(parentMesh) {
            Shader = shader ?? Shader;
            VertexBuffer = vBuffer ?? VertexBuffer;
            Material = material ?? Material;
            PrimitiveCalls = primCalls;
        }

        public ModelMeshPart(ModelMesh parentMesh, ShaderProgram shader, VertexBuffer vBuffer, ShaderMaterial material, int primCallCount)
            : this(parentMesh, shader, vBuffer, material, new ModelMeshPartPrimCall[primCallCount]) {
        }

        public void Dispose() {
            if (VertexBuffer?.DisposalLevel == ItemDisposalLevel.ModelMeshPart)
                VertexBuffer.Dispose();
            VertexBuffer = null;
        }


        public void SetPrimitiveCalls(IEnumerable<ModelMeshPartPrimCall> copyFrom) {
            IEnumerator<ModelMeshPartPrimCall> cfEnum = copyFrom.GetEnumerator();
            int i = -1;
            while (++i < PrimitiveCalls.Length && cfEnum.MoveNext())
                PrimitiveCalls[i] = cfEnum.Current;
        }



        public void Draw() {
            if (Shader.Use()) {
                ShaderProgram.UseVertexBuffer(VertexBuffer);
                ShaderProgram.UseIndexBuffer(ParentMesh.IndexBuffer);
                ParentMesh.Transform.ApplyToShader(Shader);
                Shader.FinalizeTransformations();
            }
            else {
                if (ShaderProgram.CurrentBoundVBuffer != VertexBuffer)
                    ShaderProgram.UseVertexBuffer(VertexBuffer);
            }
            Shader.ApplyMaterial(Material);

            for (int i = 0; i < PrimitiveCalls.Length; i++) {
                GL.DrawElements(PrimitiveCalls[i].PrimitiveType, PrimitiveCalls[i].IndexCount, DrawElementsType.UnsignedShort, PrimitiveCalls[i].StartOffset);
            }
        }
    }
}

using System;

using OpenTK.Graphics.ES30;


namespace FirimenticEngine.Graphics
{
    public struct ModelMeshPartPrimCall
    {
        public BeginMode PrimitiveType;
        public int IndexCount;
        public IntPtr StartOffset;

        
        public int StartIndex {
            get { return (int)StartOffset / sizeof(ushort); }
            set { StartOffset = (IntPtr)(value * sizeof(ushort)); }
        }


        public ModelMeshPartPrimCall(BeginMode primType, int indexCount, IntPtr startOffset) {
            PrimitiveType = primType;
            IndexCount = indexCount;
            StartOffset = startOffset;
        }
        public ModelMeshPartPrimCall(BeginMode primType, int indexCount, int startIndex) {
            PrimitiveType = primType;
            IndexCount = indexCount;
            StartOffset = IntPtr.Zero;
            StartIndex = startIndex;
        }
    }
}

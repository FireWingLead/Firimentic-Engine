
using OpenTK;

using FirimenticEngine.Graphics;


namespace FirimenticEngine.CustomComponents.Graphics.Vertices
{
    public struct VertexPositioned : IVertexPositioned
    {
        public Vector3 position;


        public Vector3 Position { get { return position; } set { position = value; } }


        public VertexPositioned(Vector3 pos) {
            position = pos;
        }

        public VertexPositioned(float posX, float posY, float posZ) {
            position = new Vector3(posX, posY, posZ);
        }
    }
}

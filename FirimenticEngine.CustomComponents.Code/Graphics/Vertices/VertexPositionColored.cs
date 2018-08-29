using OpenTK;

using FirimenticEngine.Graphics;


namespace FirimenticEngine.CustomComponents.Graphics.Vertices
{
    public struct VertexPositionColored : IVertexPositioned, IVertexColored
    {
        public Vector3 position;
        public Vector4 color;


        public Vector3 Position { get { return position; } set { position = value; } }
        public Vector4 Color { get { return color; } set { color = value; } }


        public VertexPositionColored(Vector3 pos, Vector4 col) {
            position = pos;
            color = col;
        }

        public VertexPositionColored(float posX, float posY, float posZ, float colR, float colG, float colB, float colA) {
            position = new Vector3(posX, posY, posZ);
            color = new Vector4(colR, colG, colB, colA);
        }

        public VertexPositionColored(float posX, float posY, float posZ, byte colR, byte colG, byte colB, byte colA) {
            position = new Vector3(posX, posY, posZ);
            color = new Vector4(colR, colG, colB, colA);
        }

        public VertexPositionColored(float posX, float posY, float posZ, float colR, float colG, float colB)
            : this(posX, posY, posZ, colR, colG, colB, 1.0f) {
        }

        public VertexPositionColored(float posX, float posY, float posZ, byte colR, byte colG, byte colB)
            : this(posX, posY, posZ, colR, colG, colB, 255) {
        }
    }
}

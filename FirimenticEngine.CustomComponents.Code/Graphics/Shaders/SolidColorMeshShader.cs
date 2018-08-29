using OpenTK;
using OpenTK.Graphics.ES30;

using FirimenticEngine.Graphics.Shaders;
using FirimenticEngine.Services;


namespace FirimenticEngine.CustomComponents.Graphics.Shaders
{
    public class SolidColorMeshShader : ShaderProgram
    {
        private static volatile SolidColorMeshShader sharedInstance;
        private static object initKey = new object();
        
        static SolidColorMeshShader() { }

        private SolidColorMeshShader() : base(
            ServiceManager.GetService<IResourceService>().GetTextFileResource("Resources/Shaders/SolidColorMesh.vsh"),
            ServiceManager.GetService<IResourceService>().GetTextFileResource("Resources/Shaders/SolidColorMesh.fsh")
            ) {
            
        }

        /// <summary>
        /// Gets the singleton instance of this shader type.
        /// </summary>
        public static SolidColorMeshShader SharedInstance { get { return sharedInstance; } }

        /// <summary>
        /// Creates and loads the SharedInstance if it has not already been loaded. This method is thread safe.
        /// </summary>
        public static void Load() {
            if (sharedInstance == null) {
                lock (initKey) {
                    if (sharedInstance == null)
                        sharedInstance = new SolidColorMeshShader();
                }
            }
        }

        /// <summary>
        /// Unloads and disposes of the SharedInstance.
        /// </summary>
        public static void Release() {
            sharedInstance.Dispose();
        }

        public override void Dispose() {
            base.Dispose();
            sharedInstance = null;
        }



        public override void FinalizeTransformations() {
            Transform3D.Recalculate();
            GL.UniformMatrix4(UniformLocations[(int)UniformParts.WorldViewProjection], 1, false, ref Transform3D.WorldViewProjection.Row0.X);
        }

        
        public void SetColor(ref Vector4 color) { GL.Uniform4(UniformLocations[(int)UniformParts.Color], color.X, Color.Y, Color.Z, Color.W); }

        public Vector4 Color {
            get {
                float[] vals = new float[4];
                GL.GetUniform(Handle, UniformLocations[(int)UniformParts.Color], vals);
                return new Vector4(vals[0], vals[1], vals[2], vals[3]);
            }
            set {
                SetColor(ref value);
            }
        }
    }
}

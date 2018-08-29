using System;
using Android.Views;
using Android.Content;
using Android.Util;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using OpenTK.Platform;
using OpenTK.Platform.Android;

using FirimenticEngine.Graphics;
using FirimenticEngine.Graphics.Shaders;
using FirimenticEngine.CustomComponents.Graphics;
using FirimenticEngine.CustomComponents.Graphics.Shaders;
using FirimenticEngine.CustomComponents.Graphics.Vertices;

namespace Firimentic.Droid
{
    class GLView1 : AndroidGameView
    {
        float time = 0.0f;

        ModelMesh sphere;

        Vector4 cameraUp;
        Vector4 cameraRight;
        Vector4 cameraPos;
        VP3DShaderTransform camera = new VP3DShaderTransform();

        Vector3 rotSpeed = new Vector3(0, MathHelper.Pi / 4, 0);


        public GLView1(Context context) : base(context) {
            //ShaderTransform t2 = ShaderTransform.Empty;
            //WVP2DShaderTransform t = new WVP2DShaderTransform();
        }

        // This gets called when the drawing surface is ready
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            SolidColorMeshShader.Load();
            SolidColorMeshShader.SharedInstance.Use();

            GL.Viewport(0, 0, Width, Height);
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(40.0f), (float)Width / (float)Height, 1.0f, 10000.0f);
            camera.SetProjection(
                mat.M11, mat.M12, mat.M13, mat.M14,
                mat.M21, mat.M22, mat.M23, mat.M24,
                mat.M31, mat.M32, mat.M33, mat.M34,
                mat.M41, mat.M42, mat.M43, mat.M44
            );

            cameraUp = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
            cameraRight = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
            cameraPos = new Vector4(0.0f, 10.0f, 40.0f, 1.0f);
            mat = Matrix4.LookAt(cameraPos.Xyz, Vector3.Zero, cameraUp.Xyz);
            camera.SetView(
                mat.M11, mat.M12, mat.M13, mat.M14,
                mat.M21, mat.M22, mat.M23, mat.M24,
                mat.M31, mat.M32, mat.M33, mat.M34,
                mat.M41, mat.M42, mat.M43, mat.M44
            );

            //sphere = ShapeGenerator.MakeCube<VertexPositioned>(SolidColorMeshShader.SharedInstance, VertexFormat.Position);
            sphere = ShapeGenerator.MakeSphere2<VertexPositioned>(
                10, 20, Axii.Y, (int)All.Lines, SolidColorMeshShader.SharedInstance, VertexFormat.Position
            );
            mat = Matrix4.Scale(1.0f);
            World3DShaderTransform worldTrans = new World3DShaderTransform();
            worldTrans.SetWorld(
                mat.M11, mat.M12, mat.M13, mat.M14,
                mat.M21, mat.M22, mat.M23, mat.M24,
                mat.M31, mat.M32, mat.M33, mat.M34,
                mat.M41, mat.M42, mat.M43, mat.M44
            );
            sphere.Transform = worldTrans;
            SolidColorShaderMaterial material = new SolidColorShaderMaterial();
            material.SetColor(0.0f, 1.0f, 1.0f, 1.0f);
            sphere.DefaultMaterial = sphere.Parts[0].Material = material;

            // Run the render loop
            Run();
        }

        // This method is called everytime the context needs
        // to be recreated. Use it to set any egl-specific settings
        // prior to context creation
        //
        // In this particular case, we demonstrate how to set
        // the graphics mode and fallback in case the device doesn't
        // support the defaults
        protected override void CreateFrameBuffer() {
            // the default GraphicsMode that is set consists of (16, 16, 0, 0, 2, false)
            try {
                Log.Verbose("GLCube", "Loading with default settings");

                // if you don't call this, the context won't be created
                base.CreateFrameBuffer();
                //this.GraphicsContext.MakeCurrent(this.WindowInfo);
                return;
            } catch (Exception ex) {
                Log.Verbose("GLCube", "{0}", ex);
            }

            // this is a graphics setting that sets everything to the lowest mode possible so
            // the device returns a reliable graphics setting.
            try {
                Log.Verbose("GLCube", "Loading with custom Android settings (low mode)");
                GraphicsMode = new AndroidGraphicsMode(0, 0, 0, 0, 0, false);

                // if you don't call this, the context won't be created
                base.CreateFrameBuffer();
                //this.GraphicsContext.MakeCurrent(this.WindowInfo);
                return;
            } catch (Exception ex) {
                Log.Verbose("GLCube", "{0}", ex);
            }
            throw new Exception("Can't load egl, aborting");
        }

        // This gets called on each frame render
        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SolidColorMeshShader shader = SolidColorMeshShader.SharedInstance;
            shader.Use();
            camera.ApplyToShader(shader);

            sphere.Draw();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);

            time = (time + (float)e.Time) % float.MaxValue;

            Vector3 mColor = new Vector3((float)Math.Sin(time / 8.0f) / 2.0f + 0.5f, (float)Math.Sin(time / 4.0f) / 2.0f + 0.5f, (float)Math.Sin(time / 2.0f) / 2.0f + 0.5f);
            ((SolidColorShaderMaterial)sphere.DefaultMaterial).SetColor(mColor.X, mColor.Y, mColor.Z, 1.0f);

            //Matrix4 worldRot = Matrix4.CreateRotationY(rotSpeed.Y * (float)e.Time);
            ////Matrix4 worldRot2 = Matrix4.CreateRotationX(rotSpeed.X * (float)e.Time);
            //((World3DShaderTransform)sphere.Transform).MultWorld(
            //    worldRot.M11, worldRot.M12, worldRot.M13, worldRot.M14,
            //    worldRot.M21, worldRot.M22, worldRot.M23, worldRot.M24,
            //    worldRot.M31, worldRot.M32, worldRot.M33, worldRot.M34,
            //    worldRot.M41, worldRot.M42, worldRot.M43, worldRot.M44
            //);
        }

        float[] square_vertices = {
            -0.5f, -0.5f,
            0.5f, -0.5f,
            -0.5f, 0.5f,
            0.5f, 0.5f,
        };

        byte[] square_colors = {
            255, 255,   0, 255,
            0,   255, 255, 255,
            0,     0,    0,  0,
            255,   0,  255, 255,
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using OpenTK.Input;

using FirimenticEngine.Services;
using FirimenticEngine.CustomComponents.Graphics;
using FirimenticEngine.CustomComponents.Graphics.Shaders;
using FirimenticEngine.Math;

using Firimentic.Services;


namespace Firimentic
{
    class Program
    {
        public const int NULL = 0;
        
        static GameWindow window;


        static void RegisterServices() {
            ServiceManager.RegisterSingleton(typeof(IResourceService), new ResourceService());
        }

        static void Main(string[] args)
        {
            RegisterServices();

            window = new GameWindow((int)(DisplayDevice.Default.Width * 0.7f), (int)(DisplayDevice.Default.Height * 0.7f), GraphicsMode.Default, "FirimenticEngine", GameWindowFlags.Default, DisplayDevice.Default);
            //window.WindowState = WindowState.Normal;
            window.Load += Window_Load;
            window.Resize += Window_Resize;
            window.RenderFrame += Window_RenderFrame;
            window.Closed += Window_Closed;
            window.KeyUp += Window_KeyUp;
            window.MouseMove += Window_MouseMove;
            window.UpdateFrame += Window_UpdateFrame;
            window.FocusedChanged += Window_FocusedChanged;
            
            //window.CursorVisible = false;

            window.Run();
        }

        private static void Window_FocusedChanged(object sender, EventArgs e) {
            //if (window.Focused) {
            //    Mouse.SetPosition(window.X + window.Width / 2, window.Y + window.Height / 2);
            //    ignoreMouseMoveEvents = 2;
            //}
        }

        static float time = 0.0f;
        static Vector4 cameraUp;
        static Vector4 cameraRight;
        static Vector4 cameraPos;
        static bool mouseDragged = false;
        static Vector2 mouseDrag = Vector2.Zero;
        static Vector2 rotSpeed = Vector2.Zero;
        static int ignoreMouseMoveEvents = 2;
        private static void Window_UpdateFrame(object sender, FrameEventArgs e) {
            time = (time + (float)e.Time) % float.MaxValue;

            Vector3 mColor = new Vector3((float)Math.Sin(time / 8.0f) / 2.0f + 0.5f, (float)Math.Sin(time / 4.0f) / 2.0f + 0.5f, (float)Math.Sin(time / 2.0f) / 2.0f + 0.5f);
            ((SolidColorShaderMaterial)sphere.DefaultMaterial).Color = new Vector4(mColor, 1.0f);

            Matrix4 worldRot = Matrix4.CreateRotationY(MathHelper.Pi / 4 * (float)e.Time);
            //Matrix4 worldRot2 = Matrix4.CreateRotationX(MathHelper.Pi / 4 * (float)e.Time);
            ((World3DShaderTransform)sphere.Transform).World = worldRot * ((World3DShaderTransform)sphere.Transform).World;

            //if (mouseDragged) {
            //    Matrix4 yawRot = Matrix4.CreateFromAxisAngle(cameraUp.Xyz, mouseDrag.X);
            //    Matrix4 pitRot = Matrix4.CreateFromAxisAngle(cameraRight.Xyz, -mouseDrag.Y);
            //    Matrix4 camRot = pitRot * yawRot;
            //    cameraUp = camRot * cameraUp;
            //    cameraRight = camRot * cameraRight;
            //    cameraPos = camRot * cameraPos;
            //    camera.View = Matrix4.LookAt(cameraPos.Xyz, Vector3.Zero, cameraUp.Xyz);
            //    mouseDrag = Vector2.Zero;
            //    mouseDragged = false;
            //}
        }

        private static void Window_MouseMove(object sender, MouseMoveEventArgs e) {
            //if (ignoreMouseMoveEvents > 0) {
            //    Mouse.SetPosition(window.X + window.Width / 2, window.Y + window.Height / 2);
            //    ignoreMouseMoveEvents--;
            //    return;
            //}
            //if (e.Mouse.LeftButton == ButtonState.Pressed && (e.XDelta > 0 || e.YDelta > 0)) {
            //    mouseDrag.X += e.XDelta;
            //    mouseDrag.Y += e.YDelta;
            //    mouseDragged = true;
            //    Mouse.SetPosition(window.X + window.Width / 2, window.Y + window.Height / 2);
            //    ignoreMouseMoveEvents = 2;
            //}
        }

        private static void Window_Load(object sender, EventArgs e) {
            SolidColorMeshShader.Load();
            SolidColorMeshShader.SharedInstance.Use();

            camera.Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(40.0f), (float)window.Width / (float)window.Height, 1.0f, 10000.0f);
            //camera.Projection = Matrix4.CreateOrthographic(10.0f * (float)window.Width / (float)window.Height, 10.0f, 1.0f, 10000.0f);

            cameraUp = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
            cameraRight = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
            cameraPos = new Vector4(0.0f, 10.0f, 40.0f, 1.0f);
            camera.View = Matrix4.LookAt(cameraPos.Xyz, Vector3.Zero, cameraUp.Xyz);
            //camera.View = Matrix4.Identity;

            /*
            sphere = new ModelMesh() {
                Transform = new World3DShaderTransform(Matrix4.CreateScale(8.0f)),
                DefaultShader = SolidColorMeshShader.SharedInstance,
                DefaultVertexBuffer = new VertexBuffer() { DisposalLevel = ItemDisposalLevel.ModelMesh },
                DefaultMaterial = new SolidColorShaderMaterial(new Vector4(0.0f, 1.0f, 1.0f, 1.0f)),
                IndexBuffer = new IndexBuffer() { DisposalLevel = ItemDisposalLevel.ModelMesh }
            };
            */
            sphere = ShapeGenerator.MakeSphere<FirimenticEngine.CustomComponents.Graphics.Vertices.VertexPositioned>(
                10, 20, FirimenticEngine.Graphics.Axii.Y, PrimitiveType.Lines, SolidColorMeshShader.SharedInstance,
                FirimenticEngine.Graphics.VertexFormat.Position
            );
            sphere.Transform = new World3DShaderTransform(Matrix4.CreateScale(8.0f));
            sphere.DefaultMaterial = sphere.Parts[0].Material = new SolidColorShaderMaterial(new Vector4(0.0f, 1.0f, 1.0f, 1.0f));

            /*
            VertexPositioned[] verts = new VertexPositioned[] {
                new VertexPositioned( 0.0f, 0.0f, 0.0f),
                new VertexPositioned( 1.0f, 0.0f, 0.0f),
                new VertexPositioned(-1.0f, 0.0f, 0.0f),
                new VertexPositioned( 0.0f, 1.0f, 0.0f),
                new VertexPositioned( 0.0f,-1.0f, 0.0f),
                new VertexPositioned( 0.0f, 0.0f, 1.0f),
                new VertexPositioned( 0.0f, 0.0f,-1.0f)
            };

            ushort[] indcs = new ushort[] {
                2, 0, 4
            };
            */


            /*
            VertexPositioned[] verts = new VertexPositioned[] {
                new VertexPositioned(-1.0f, 1.0f, 1.0f),
                new VertexPositioned( 1.0f, 1.0f, 1.0f),
                new VertexPositioned( 1.0f,-1.0f, 1.0f),
                new VertexPositioned(-1.0f,-1.0f, 1.0f),
                new VertexPositioned(-1.0f, 1.0f,-1.0f),
                new VertexPositioned( 1.0f, 1.0f,-1.0f),
                new VertexPositioned( 1.0f,-1.0f,-1.0f),
                new VertexPositioned(-1.0f,-1.0f,-1.0f)
            };
            
            ushort[] indcs = new ushort[] {
                0, 1, 2, 3, 0,
                4, 7, 3, 2,
                6, 7, 6,
                5, 4, 5,
                1,
                3, 2, 0,
                7, 4, 3,
                6, 2, 7,
                5, 6, 4,
                1, 0, 5,
                2, 1, 6
            };
            */


            /*
            VertexPositioned[] verts = new VertexPositioned[19 * 40 + 2];

            float inc = 9.0f * MathHelper.Pi / 180.0f;
            float fullCirc = 2 * MathHelper.Pi - float.Epsilon;
            float halfCirc = MathHelper.Pi - float.Epsilon;
            float scale = 1.0f;
            int ind = 0;

            verts[ind++] = new VertexPositioned(Vector3.UnitY * scale);
            verts[ind++] = new VertexPositioned(Vector3.UnitY * -scale);
            for (float lng = 0; lng < fullCirc; lng += inc) {
                for (float lat = inc; lat < halfCirc; lat += inc) {
                    Matrix4 rot = Matrix4.CreateRotationZ(lng) * Matrix4.CreateRotationX(lat);
                    Vector3 pos = (Vector3.UnitY * scale).Transform(ref rot);
                    verts[ind++] = new VertexPositioned(pos);
                }
            }


            ushort[] indcs = new ushort[20 * (2 + 19 + 19) + 1 + 19 * (40 + 1)];

            ind = 0;
            for (ushort lngInd = 0; lngInd < 20; lngInd++) {
                indcs[ind++] = 0;
                for (ushort latInd = 2; latInd < 21; latInd++) {
                    indcs[ind++] = (ushort)(lngInd * 19 + latInd);
                }
                indcs[ind++] = 1;
                for (ushort latInd = 20; latInd > 1; latInd--) {
                    indcs[ind++] = (ushort)((lngInd + 20) * 19 + latInd);
                }
            }
            indcs[ind++] = 0;
            for (ushort latInd = 2; latInd < 21; latInd++) {
                for (ushort lngInd = 0; lngInd < 40; lngInd++) {
                    indcs[ind++] = (ushort)(lngInd * 19 + latInd);
                }
                indcs[ind++] = latInd;
            }
            */


            //sphere.DefaultVertexBuffer.SetData(VertexFormat.Position, verts);
            //sphere.IndexBuffer.SetData(indcs);

            //new ModelMeshPart(sphere, null, null, null, PrimitiveType.LineStrip, indcs.Length, 0);
        }

        private static void Window_Closed(object sender, EventArgs e) {
            SolidColorMeshShader.Release();
            sphere.Dispose();
            sphere = null;
        }
        
        private static void Window_Resize(object sender, EventArgs e) {
            GL.Viewport(0, 0, window.Width, window.Height);
            camera.Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(40.0f), (float)window.Width / (float)window.Height, 1.0f, 10000.0f);
        }

        
        static FirimenticEngine.Graphics.ModelMesh sphere;
        static VP3DShaderTransform camera = new VP3DShaderTransform();
        private static void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SolidColorMeshShader shader = SolidColorMeshShader.SharedInstance;
            shader.Use();
            camera.ApplyToShader(shader);

            sphere.Draw();
            
            window.SwapBuffers();
        }

        private static void Window_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                window.Exit();
                return;
            }
        }



        
    }
}

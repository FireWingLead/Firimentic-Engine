using System;

using OpenTK;
using OpenTK.Graphics.ES30;

using FirimenticEngine.Graphics;
using FirimenticEngine.Graphics.Shaders;


namespace FirimenticEngine.CustomComponents.Graphics
{
    public static class ShapeGenerator
    {
        public static ModelMesh MakeCube<VertType>(ShaderProgram shader, VertexFormat vertFmt)
            where VertType : struct, IVertexPositioned {

            ModelMesh cube = new ModelMesh() {
                IndexBuffer = new IndexBuffer() { DisposalLevel = ItemDisposalLevel.ModelMesh },
                Transform = ShaderTransform.Empty
            };
            ushort[] indices = new ushort[] {
                0, 1, 2, 3, 0, 4, 7, 3, 1, 5, 6, 2, 5, 4, 6, 7, 2, 0, 5, 7, 0
            };
            cube.IndexBuffer.SetData(indices);

            ModelMeshPart part = new ModelMeshPart(cube) {
                Shader = shader,
                VertexBuffer = new VertexBuffer() { DisposalLevel = ItemDisposalLevel.ModelMeshPart }
            };
            VertType[] verts = new VertType[8];
            verts[0].Position = new Vector3(-1,  1, -1);
            verts[0].Position = new Vector3( 1,  1, -1);
            verts[0].Position = new Vector3( 1, -1, -1);
            verts[0].Position = new Vector3(-1, -1, -1);
            verts[0].Position = new Vector3(-1,  1,  1);
            verts[0].Position = new Vector3( 1,  1,  1);
            verts[0].Position = new Vector3( 1, -1,  1);
            verts[0].Position = new Vector3(-1, -1,  1);
            part.VertexBuffer.SetData(vertFmt, verts);
            part.PrimitiveCalls = new ModelMeshPartPrimCall[] {
                new ModelMeshPartPrimCall(BeginMode.LineStrip, 8, 0)
            };

            return cube;
        }



        public static ModelMesh MakeSphere2<VertType>(int stacks, int sectors, Axii poleAxis, int primType, ShaderProgram shader, VertexFormat vertFmt)
            where VertType : struct, IVertexPositioned {
            return MakeSphere<VertType>(stacks, sectors, poleAxis, (BeginMode)primType, shader, vertFmt);
        }

        public static ModelMesh MakeSphere<VertType>(int stacks, int sectors, Axii poleAxis, BeginMode primType, ShaderProgram shader, VertexFormat vertFmt)
            where VertType : struct, IVertexPositioned {
            //Check for errors.
            if (stacks < 2)
                throw new ArgumentOutOfRangeException(nameof(stacks), stacks, string.Format("Argument '{0}' ({1}) cannot be less than {2}.", nameof(stacks), stacks, 2));
            if (sectors < 3)
                throw new ArgumentOutOfRangeException(nameof(sectors), sectors, string.Format("Argument '{0}' ({1}) cannot be less than {2}.", nameof(sectors), sectors, 3));
            int latLines = stacks - 1;
            int lonLines = sectors;
            int numVerts = latLines * lonLines + 2;
            if (numVerts > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(
                    nameof(latLines) + " * " + nameof(lonLines) + " + 2",
                    numVerts,
                    string.Format("Arguments '{0}' ({1}) specifies too many verticies to be indexed by the graphics engine's index type (ushort), which has a max value of {3}.", nameof(latLines) + " * " + nameof(lonLines) + " + 2", numVerts, ushort.MaxValue)
                );


            ModelMesh sphere = new ModelMesh() {
                IndexBuffer = new IndexBuffer() { DisposalLevel = ItemDisposalLevel.ModelMesh },
                Transform = ShaderTransform.Empty
            };
            sphere.IndexBuffer.SetData(MakeSphereIndices(latLines, lonLines, primType));

            ModelMeshPart part = new ModelMeshPart(sphere) {
                Shader = shader,
                VertexBuffer = new VertexBuffer() { DisposalLevel = ItemDisposalLevel.ModelMeshPart }
            };
            part.VertexBuffer.SetData(vertFmt, MakeShpereVertices<VertType>(latLines, lonLines, poleAxis));
            part.PrimitiveCalls = MakeSpherePrimCalls(latLines, lonLines, primType);

            return sphere;
        }

        private static VertType[] MakeShpereVertices<VertType>(int latLines, int lonLines, Axii poleAxis) where VertType : struct, IVertexPositioned {
            //Create the vertex array.
            VertType[] verts = new VertType[latLines * lonLines + 2];

            //Set up needed vars and values.
            float latRot = MathHelper.Pi / (latLines + 1);
            float lonRot = MathHelper.TwoPi / lonLines;
            Vector3 northPole;
            switch (poleAxis) {
                case Axii.X: northPole = Vector3.UnitX; break;
                case Axii.Z: northPole = Vector3.UnitX; break;
                case Axii.Y:
                default: northPole = Vector3.UnitX; break;
            }
            bool normaledVerts = verts[0] is IVertexNormaled;
            bool texturedVerts = verts[0] is IVertexTextured;
            bool dblTexturedVerts = verts[0] is IVertexDoubleTextured;
            int vertIndex = 0;

            //Create non-pole vertices and calculate their values.
            for (int latIndex = 0; latIndex < latLines; latIndex++) {
                Matrix4 latRotMat;
                switch (poleAxis) {
                    case Axii.X: Matrix4.CreateRotationZ(latRot * latIndex, out latRotMat); break;
                    case Axii.Z: Matrix4.CreateRotationY(latRot * latIndex, out latRotMat); break;
                    case Axii.Y:
                    default: Matrix4.CreateRotationX(latRot * latIndex, out latRotMat); break;
                }
                for (int lonIndex = 0; lonIndex < lonLines; lonIndex++) {
                    Matrix4 lonRotMat;
                    switch (poleAxis) {
                        case Axii.X: Matrix4.CreateRotationX(lonRot * lonIndex, out lonRotMat); break;
                        case Axii.Z: Matrix4.CreateRotationZ(lonRot * lonIndex, out lonRotMat); break;
                        case Axii.Y:
                        default: Matrix4.CreateRotationY(lonRot * lonIndex, out lonRotMat); break;
                    }
                    Matrix4.Mult(ref latRotMat, ref lonRotMat, out Matrix4 rotMat);
                    Vector3 pos = (Math.Matrix4x1)northPole * rotMat;
                    verts[vertIndex] = new VertType() { Position = pos };
                    if (normaledVerts)
                        ((IVertexNormaled)verts[vertIndex]).Normal = pos;
                    if (texturedVerts)
                        ((IVertexTextured)verts[vertIndex]).TexCoords = new Vector2(lonRot * lonIndex / MathHelper.TwoPi, latRot * latIndex / MathHelper.Pi);
                    if (dblTexturedVerts)
                        ((IVertexDoubleTextured)verts[vertIndex]).Tex2Coords = new Vector2(lonRot * lonIndex / MathHelper.TwoPi, latRot * latIndex / MathHelper.Pi);
                    vertIndex++;
                }
            }

            //Create pole vertices and calculate their values.
            verts[vertIndex] = new VertType();
            verts[vertIndex + 1] = new VertType();
            verts[vertIndex].Position = northPole;
            verts[vertIndex + 1].Position = -northPole;
            if (normaledVerts) {
                ((IVertexNormaled)verts[vertIndex]).Normal = northPole;
                ((IVertexNormaled)verts[vertIndex + 1]).Normal = -northPole;
            }
            if (texturedVerts) {
                ((IVertexTextured)verts[vertIndex]).TexCoords = new Vector2(0.5f, 0.0f);
                ((IVertexTextured)verts[vertIndex + 1]).TexCoords = new Vector2(0.5f, 1.0f);
            }
            if (dblTexturedVerts) {
                ((IVertexDoubleTextured)verts[vertIndex]).Tex2Coords = new Vector2(0.5f, 0.0f);
                ((IVertexDoubleTextured)verts[vertIndex + 1]).Tex2Coords = new Vector2(0.5f, 1.0f);
            }

            //Vertex array is finished.
            return verts;
        }

        private static ushort[] MakeSphereIndices(int latLines, int lonLines, BeginMode primType) {
            //Create index array.
            ushort[] indices;

            //Set up needed vars and values.
            int numVerts = latLines * lonLines + 2;
            int ind = 0;

            //Assign indices.
            switch (primType) {
                case BeginMode.Points:
                    indices = new ushort[latLines * lonLines + 2];

                    //Simply list the points by pointing to each vertex in order.
                    for (ushort i = 0; i < indices.Length; i++)
                        indices[i] = i;

                    break;

                case BeginMode.Lines:
                    indices = new ushort[(latLines + 1) * lonLines + lonLines * latLines];

                    //First, all the longitude lines in one LineStrip or LineLoop.
                    for (int lonInd = 0; lonInd < lonLines; lonInd += 2) {
                        indices[ind++] = (ushort)(numVerts - 1);
                        for (int latInd = 0; latInd < latLines; latInd++) {
                            indices[ind++] = (ushort)(latInd * lonLines + lonInd);
                        }
                        indices[ind++] = (ushort)(numVerts - 1);
                        if (lonInd + 1 < lonLines) {
                            for (int latInd = latLines - 1; latInd > -1; latInd--) {
                                indices[ind++] = (ushort)(latInd * lonLines + lonInd);
                            }
                        }
                    }

                    //Next, Each latitude line in a LineLoop
                    for (int i = 0; i < numVerts - 2; i++) {
                        indices[ind++] = (ushort)i;
                    }

                    break;

                case BeginMode.Triangles:
                    //                      pole-tri-fans   +   slice-tri-strip  * (num-slices-between-fans)
                    indices = new ushort[(lonLines + 2) * 2 + (lonLines + 1) * 2 * (latLines - 1)];

                    //Start with a triangle fan around each pole.
                    indices[ind++] = (ushort)(numVerts - 2);//North Pole
                    indices[ind++] = 0;
                    for (int i = lonLines - 1; i > -1; i--) {
                        indices[ind++] = (ushort)i;
                    }
                    indices[ind++] = (ushort)(numVerts - 1);//South Pole
                    for (int i = numVerts - 2 - lonLines; i < numVerts - 2; i++) {
                        indices[ind++] = (ushort)i;
                    }
                    indices[ind++] = (ushort)(numVerts - 2 - lonLines);

                    //Next a triangle strip for each slice between the fans.
                    for (int latInd = 0; latInd < latLines - 1; latInd++) {
                        for (int lonInd = 0; lonInd < lonLines; lonInd++) {
                            indices[ind++] = (ushort)((latInd + 1) * lonLines + lonInd);
                            indices[ind++] = (ushort)(latInd * lonLines + lonInd);
                        }
                        indices[ind++] = (ushort)((latInd + 1) * lonLines);
                        indices[ind++] = (ushort)(latInd * lonLines);
                    }

                    break;
                default:
                    throw new ArgumentException(string.Format("Argument {0} cannot be {1}.{2}. It must be Points, Lines, or Triangles.", nameof(primType), typeof(PrimitiveType).Name, primType), nameof(primType));
            }

            //Index array is finished.
            return indices;
        }

        private static ModelMeshPartPrimCall[] MakeSpherePrimCalls(int latLines, int lonLines, BeginMode primType) {
            ModelMeshPartPrimCall[] primCalls;

            int numVerts = latLines * lonLines + 2;

            switch (primType) {
                case BeginMode.Points:
                    primCalls = new ModelMeshPartPrimCall[] {
                        new ModelMeshPartPrimCall(BeginMode.Points, numVerts, IntPtr.Zero)
                    };
                    break;

                case BeginMode.Lines:
                    primCalls = new ModelMeshPartPrimCall[latLines + 1];
                    primCalls[0] = new ModelMeshPartPrimCall(lonLines % 2 == 0 ? BeginMode.LineLoop : BeginMode.LineStrip, (latLines + 1) * lonLines, IntPtr.Zero);
                    for (int i = 0; i < latLines; i++) {
                        primCalls[i + 1] = new ModelMeshPartPrimCall(BeginMode.LineLoop, lonLines, primCalls[0].IndexCount + i * lonLines);
                    }
                    break;

                case BeginMode.Triangles:
                    primCalls = new ModelMeshPartPrimCall[latLines + 1];
                    primCalls[0] = new ModelMeshPartPrimCall(BeginMode.TriangleFan, lonLines + 1, IntPtr.Zero);
                    primCalls[1] = new ModelMeshPartPrimCall(BeginMode.TriangleFan, lonLines + 1, primCalls[0].IndexCount);
                    int startInd = primCalls[0].IndexCount + primCalls[1].IndexCount;
                    for (int i = 0; i < latLines - 1; i++) {
                        primCalls[i + 2] = new ModelMeshPartPrimCall(BeginMode.TriangleStrip, (lonLines + 1) * 2, startInd);
                        startInd += primCalls[i + 2].IndexCount;
                    }
                    break;

                default:
                    throw new ArgumentException(string.Format("Argument {0} cannot be {1}.{2}. It must be Points, Lines, or Triangles.", nameof(primType), typeof(PrimitiveType).Name, primType), nameof(primType));
            }

            return primCalls;
        }
    }
}

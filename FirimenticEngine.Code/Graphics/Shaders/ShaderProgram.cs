using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK;
using OpenTK.Graphics.ES30;
using System.Text;

namespace FirimenticEngine.Graphics.Shaders
{
    public enum ShaderProgramLoadState
    {
        Created, Linked, Disposed
    }

    public enum ShaderLoadState
    {
        Uncreated, Compiled, Deleted
    }



    public abstract class ShaderProgram : IDisposable {
        public const int UNUSED_PART = -1;
        static readonly IReadOnlyDictionary<string, int> UniformPartCodesByUniformName = ((UniformParts[])Enum.GetValues(typeof(UniformParts))).Cast<int>().ToDictionary(val => Enum.GetName(typeof(UniformParts), (UniformParts)val).CamelCase());

        public static ShaderProgram CurrentInUseShader { get; private set; }
        public static VertexFormat CurrentBoundVFormat { get; private set; }
        public static VertexBuffer CurrentBoundVBuffer { get; internal set; }
        public static IndexBuffer CurrentBoundIBuffer { get; internal set; }



        protected ShaderProgram() {
            Handle = GL.CreateProgram();
            LoadState = ShaderProgramLoadState.Created;
        }

        public ShaderProgram(string vertShaderSource, string fragShaderSource) : this() {
            SetVertexShaderSource(vertShaderSource);
            SetFragmentShaderSource(fragShaderSource);
            Link();
        }

        public virtual void Dispose() {
            if (VertexShaderHandle != Constants.NULL) {
                GL.DetachShader(Handle, VertexShaderHandle);
                GL.DeleteShader(VertexShaderHandle);
                VertexShaderHandle = Constants.NULL;
                VertexShaderLoadState = ShaderLoadState.Deleted;
            }
            if (FragmentShaderHandle != Constants.NULL) {
                GL.DetachShader(Handle, FragmentShaderHandle);
                GL.DeleteShader(FragmentShaderHandle);
                FragmentShaderHandle = Constants.NULL;
                FragmentShaderLoadState = ShaderLoadState.Deleted;
            }

            if (CurrentInUseShader == this) {
                CurrentInUseShader = null;
                GL.BindBuffer(BufferTarget.ArrayBuffer, Constants.NULL);
                CurrentBoundVBuffer = null;
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, Constants.NULL);
                CurrentBoundIBuffer = null;
                CurrentBoundVFormat = null;
            }

            if (Handle != Constants.NULL) {
                GL.DeleteProgram(Handle);
                Handle = Constants.NULL;
            }
            LoadState = ShaderProgramLoadState.Disposed;
        }



        public int Handle { get; private set; } = Constants.NULL;
        public ShaderProgramLoadState LoadState { get; private set; }

        public int VertexShaderHandle { get; private set; } = Constants.NULL;
        public int FragmentShaderHandle { get; private set; } = Constants.NULL;

        public ShaderLoadState VertexShaderLoadState { get; private set; }
        public ShaderLoadState FragmentShaderLoadState { get; private set; }

        public IReadOnlyList<int> VertexPartAttributeIndices { get; private set; }
        public IReadOnlyList<int> UniformLocations { get; private set; }

        public WVP2DShaderTransform Transform2D { get; private set; } = new WVP2DShaderTransform();
        public WVP3DShaderTransform Transform3D { get; private set; } = new WVP3DShaderTransform();



        public void SetVertexShaderSource(string source) {
            if (LoadState > ShaderProgramLoadState.Created)
                throw new InvalidOperationException(string.Format("Cannot set vertex shader source code in a ShaderProgram that is already {0}.", LoadState));
            if (VertexShaderLoadState > ShaderLoadState.Uncreated)
                throw new InvalidOperationException("Cannot set vertex shader source code in a ShaderProgram when it has already been set.");
            
            VertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShaderHandle, source);
            GL.CompileShader(VertexShaderHandle);

            GL.GetShader(VertexShaderHandle, ShaderParameter.CompileStatus, out int compileStatus);
            if (compileStatus == 0) {//There was an error
                StringBuilder logBldr = new StringBuilder(500);
                GL.GetShaderInfoLog(VertexShaderHandle, 500, out int logLen, logBldr);
                //string infoLog = GL.GetShaderInfoLog(VertexShaderHandle);
                string infoLog = logBldr.ToString();
                throw new Exception("Error compiling vertex shader:\nInfo Log:\n------------------------------------\n" + infoLog + "\n------------------------------------\n");
            }

            VertexShaderLoadState = ShaderLoadState.Compiled;
        }

        public void SetFragmentShaderSource(string source) {
            if (LoadState > ShaderProgramLoadState.Created)
                throw new InvalidOperationException(string.Format("Cannot set fragment shader source code in a ShaderProgram that is already {0}.", LoadState));
            if (FragmentShaderLoadState > ShaderLoadState.Uncreated)
                throw new InvalidOperationException("Cannot set fragment shader source code in a ShaderProgram when it has already been set.");

            FragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShaderHandle, source);
            GL.CompileShader(FragmentShaderHandle);

            GL.GetShader(FragmentShaderHandle, ShaderParameter.CompileStatus, out int compileStatus);
            if (compileStatus == 0) {//There was an error
                string infoLog = GL.GetShaderInfoLog(FragmentShaderHandle);
                throw new Exception("Error compiling fragment shader:\nInfo Log:\n------------------------------------\n" + infoLog + "\n------------------------------------\n");
            }

            FragmentShaderLoadState = ShaderLoadState.Compiled;
        }

        public void Link() {
            if (LoadState != ShaderProgramLoadState.Created)
                throw new InvalidOperationException("Cannot link an uncreated ShaderProgram.");
            if (VertexShaderLoadState != ShaderLoadState.Compiled || FragmentShaderLoadState != ShaderLoadState.Compiled)
                throw new InvalidOperationException(string.Format("Cannot link a ShaderProgram without a vertex and fragment shader. {0} = {1}. {2} = {3}",
                    nameof(VertexShaderLoadState), VertexShaderLoadState, nameof(FragmentShaderLoadState), FragmentShaderLoadState));

            //Attach any compiled shaders.
            GL.AttachShader(Handle, VertexShaderHandle);
            GL.AttachShader(Handle, FragmentShaderHandle);
            //Check for other shaders (opional ones) to attach.
            //None currunrly supported.

            //Link the program.
            GL.LinkProgram(Handle);
            //Check if the program was linked successfully.
            GL.GetProgram(Handle, ProgramParameter.LinkStatus, out int linkStatus);
            if (linkStatus == 0) {//There was an error.
                //Get the ShaderProgram InfoLog and use it in the message of an exception.
                string infoLog = GL.GetProgramInfoLog(Handle);
                throw new Exception("Error linking shader:\nInfo Log:\n------------------------------------\n" + infoLog + "\n------------------------------------\n");
            }
            //Linking was successsful. Continue with setup.
            LoadState = ShaderProgramLoadState.Linked;
            //string progLog = GL.GetProgramInfoLog(Handle);
            //Console.WriteLine(progLog);

            //We don't need the shaders anymore. Detach and delete any that were loaded.
            GL.DetachShader(Handle, VertexShaderHandle);
            GL.DeleteShader(VertexShaderHandle);
            VertexShaderHandle = Constants.NULL;
            VertexShaderLoadState = ShaderLoadState.Deleted;

            GL.DetachShader(Handle, FragmentShaderHandle);
            GL.DeleteShader(FragmentShaderHandle);
            FragmentShaderHandle = Constants.NULL;
            FragmentShaderLoadState = ShaderLoadState.Deleted;

            //Get attribute locations and set up the optimal VertexFormat that buffers should use.
            GL.GetProgram(Handle, ProgramParameter.ActiveAttributes, out int numAttrs);
            int[] partIndices = new int[VertexPart.PartTypessByAttributeName.Count];
            for (int i = 0; i < partIndices.Length; i++)
                partIndices[i] = UNUSED_PART;
            for (int i = 0; i < numAttrs; i++) {
                string attrName = GL.GetActiveAttrib(Handle, i, out int attrSizeInElements, out ActiveAttribType attrType);
                int partCode = (int)VertexPart.PartTypessByAttributeName[attrName].PartCode;
                partIndices[partCode] = GL.GetAttribLocation(Handle, attrName);
            }
            VertexPartAttributeIndices = new List<int>(partIndices);

            //Get Uniform locations and set up the array(s) of which ones are usable and how.
            GL.GetProgram(Handle, ProgramParameter.ActiveUniforms, out numAttrs);
            partIndices = new int[UniformPartCodesByUniformName.Count];
            for (int i = 0; i < partIndices.Length; i++)
                partIndices[i] = UNUSED_PART;
            for (int i = 0; i < numAttrs; i++) {
                string unifName = GL.GetActiveUniform(Handle, i, out int unifSizeInElements,out ActiveUniformType unifType);
                int partCode = UniformPartCodesByUniformName[unifName];
                partIndices[partCode] = GL.GetUniformLocation(Handle, unifName);
            }
            UniformLocations = new List<int>(partIndices);
        }



        public bool Use() {
            if (CurrentInUseShader == this)
                return false;
            GL.UseProgram(Handle);
            CurrentInUseShader = this;
            CurrentBoundVFormat = null;
            return true;
        }

        public static void UseVertexBuffer(VertexBuffer buffer) {
            if (buffer == null) {
                GL.BindBuffer(BufferTarget.ArrayBuffer, Constants.NULL);
                CurrentBoundVBuffer = null;
                return;
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.Handle);
            CurrentBoundVBuffer = buffer;
            BindAttributesForVertexFormat(buffer.VertexFormat);
        }

        public static void BindAttributesForVertexFormat(VertexFormat fmt) {
            if (fmt == CurrentBoundVFormat)
                return;

            foreach (VertexPart bufferPart in fmt.Parts) {
                int shaderPartLoc = CurrentInUseShader.VertexPartAttributeIndices[(int)bufferPart.PartCode];
                if (shaderPartLoc == UNUSED_PART)
                    continue;
                GL.VertexAttribPointer(
                    shaderPartLoc, //AttributeLocation in the Shader
                    bufferPart.SizeInElements, //Number of floats (or ints or whatever) in the attribute
                    bufferPart.ElementType, //What the attribute is made of (floats? ints? etc.)
                    false, //Should ints in the buffer be normalized to [-1.0,1.0] when they correspond to floats in the shader?
                    fmt.BytesPerVertex, //Dist from one vertex to the next in the buffer.
                    bufferPart.OffsetIntoVertexInBytes //Offset of the attribute into each vertex.
                );
                GL.EnableVertexAttribArray(shaderPartLoc);
            }

            CurrentBoundVFormat = fmt;
        }

        public static void UseIndexBuffer(IndexBuffer buffer) {
            if (buffer == null) {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, Constants.NULL);
                CurrentBoundIBuffer = null;
                return;
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer.Handle);
            CurrentBoundIBuffer = buffer;
        }

        public abstract void FinalizeTransformations();

        public void ApplyMaterial(ShaderMaterial material) {
            if (material == null)
                return;
            foreach (MaterialPart part in material) {
                int matPartCode = UniformLocations[(int)part.PartCode];
                if (matPartCode == UNUSED_PART)
                    continue;
                part.ApplyToShader(this);
            }
        }
    }
}

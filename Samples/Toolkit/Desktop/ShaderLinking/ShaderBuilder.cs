namespace ShaderLinking
{
    using System;
    using System.Text;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D;
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Rebuilds shader data and creates an effect data
    /// </summary>
    internal sealed class ShaderBuilder
    {
        private readonly ShaderData _data; // provides effect data and configuration switches
        private readonly EffectCompiler _effectCompiler; // assembles separate shader bytecodes into an effect data

        private bool _isRebuildingAfterReset; // on compilation error, we try to rebuild it again with the original data

        public ShaderBuilder(ShaderData data)
        {
            if (data == null) throw new ArgumentNullException("data");

            _data = data;
            _effectCompiler = new EffectCompiler();
        }

        public EffectData Rebuild()
        {
            // assemble the source text
            var source = GetSource();

            // compile the library
            var compilationResult = ShaderBytecode.Compile(source, "lib_5_0", ShaderFlags.OptimizationLevel3, EffectFlags.None);

            // if compilation failed - try to reset to the initial data and rebuild again
            if (compilationResult.HasErrors)
            {
                if (_isRebuildingAfterReset)
                    // something is messed up, we alraedy tried to rebuild once
                    throw new InvalidOperationException();

                try
                {
                    _isRebuildingAfterReset = true;

                    // reset the data
                    _data.Reset();
                    // rebuild
                    return Rebuild();
                }
                finally
                {
                    _isRebuildingAfterReset = false;
                }
            }

            var bytecode = compilationResult.Bytecode;

            // create the shader library module
            var shaderLibrary = new Module(bytecode);
            // create the shader library module instance
            var shaderLibraryInstance = new ModuleInstance(shaderLibrary);

            // mark the implicit constant buffer (for single parameter) as bindable
            shaderLibraryInstance.BindConstantBuffer(0, 0, 0);

            // assemble vertex shader
            var vertexShaderBytecode = AssembleVertexShader(shaderLibrary, shaderLibraryInstance);
            // assemble pixel shader
            var pixelShaderBytecode = AssemblePixelShader(shaderLibrary, shaderLibraryInstance);

            try
            {
                // assemble the effect data from the bytecodes
                return _effectCompiler.Compile(vertexShaderBytecode, pixelShaderBytecode);
            }
            finally
            {
                _data.IsDirty = false;
            }
        }

        private string GetSource()
        {
            var sb = new StringBuilder();

            sb.AppendLine(_data.Header);
            sb.AppendLine(_data.Hidden);
            // for any compilation errors - this will be the starting line
            sb.AppendLine("#line 0");
            sb.AppendLine(_data.Source);

            return sb.ToString();
        }

        private ShaderBytecode AssembleVertexShader(Module shaderLibrary, ModuleInstance shaderLibraryInstance)
        {
            var vertexShaderGraph = new FunctionLinkingGraph();

            // create input node - parameter name, semantic and size in bytes
            var vertexShaderInputNode = vertexShaderGraph.SetInputSignature(CreateInParam("inputPos", "SV_POSITION", 4),
                                                                            CreateInParam("inputTex", "COLOR", 4));

            // create the function call node
            var vertexFunctionCallNode = vertexShaderGraph.CallFunction(shaderLibrary, "VertexFunction");

            // bind input parameters to the function call
            vertexShaderGraph.PassValue(vertexShaderInputNode, 0, vertexFunctionCallNode, 0);
            vertexShaderGraph.PassValue(vertexShaderInputNode, 1, vertexFunctionCallNode, 1);

            // create the output parameters node
            var vertexShaderOutputNode = vertexShaderGraph.SetOutputSignature(CreateOutParam("outputTex", "SV_POSITION", 4),
                                                                              CreateOutParam("outputNorm", "COLOR", 4));

            // bind function call parameters to the output node
            vertexShaderGraph.PassValue(vertexFunctionCallNode, 0, vertexShaderOutputNode, 0);
            vertexShaderGraph.PassValue(vertexFunctionCallNode, 1, vertexShaderOutputNode, 1);

            // create the library instance for the shader
            var vertexShaderGraphInstance = vertexShaderGraph.CreateModuleInstance();

            var linker = new Linker();
            // bind linker to the function library
            linker.UseLibrary(shaderLibraryInstance);
            // link the shader
            return linker.Link(vertexShaderGraphInstance, "main", "vs_5_0", 0);
        }

        private ShaderBytecode AssemblePixelShader(Module shaderLibrary, ModuleInstance shaderLibraryInstance)
        {
            var pixelShaderGraph = new FunctionLinkingGraph();

            var pixelShaderInputNode = pixelShaderGraph.SetInputSignature(CreateInParam("inputPos", "SV_POSITION", 4, InterpolationMode.Undefined),
                                                                          CreateInParam("inputTex", "COLOR", 4, InterpolationMode.Undefined));

            var colorValueNode = pixelShaderGraph.CallFunction(shaderLibrary, "ColorFunction");

            pixelShaderGraph.PassValue(pixelShaderInputNode, 0, colorValueNode, 0);
            pixelShaderGraph.PassValue(pixelShaderInputNode, 1, colorValueNode, 1);

            // link additional nodes based on configuration
            if (_data.EnableInvertColor)
            {
                var tempNode = pixelShaderGraph.CallFunction(shaderLibrary, "InvertColor");
                pixelShaderGraph.PassValue(colorValueNode, tempNode, 0);
                colorValueNode = tempNode;
            }

            if (_data.EnableGrayscale)
            {
                var tempNode = pixelShaderGraph.CallFunction(shaderLibrary, "Grayscale");
                pixelShaderGraph.PassValue(colorValueNode, tempNode, 0);
                colorValueNode = tempNode;
            }

            var pixelShaderOutputNode = pixelShaderGraph.SetOutputSignature(CreateOutParam("outputColor", "SV_TARGET", 4));
            pixelShaderGraph.PassValue(colorValueNode, pixelShaderOutputNode, 0);

            var pixelShaderGraphInstance = pixelShaderGraph.CreateModuleInstance();

            var linker = new Linker();
            linker.UseLibrary(shaderLibraryInstance);
            return linker.Link(pixelShaderGraphInstance, "main", "ps_5_0", 0);
        }

        // helper methods to create shader parameters
        private ParameterDescription CreateInParam(string name, string semanticName, int columns, InterpolationMode interpolationMode = InterpolationMode.Linear)
        {
            return CreateParam(name, semanticName, columns, interpolationMode, ParameterFlags.In);
        }

        private ParameterDescription CreateOutParam(string name, string semanticName, int columns, InterpolationMode interpolationMode = InterpolationMode.Undefined)
        {

            return CreateParam(name, semanticName, columns, interpolationMode, ParameterFlags.Out);
        }

        private ParameterDescription CreateParam(string name, string semanticName, int columns, InterpolationMode interpolationMode, ParameterFlags parameterFlags)
        {
            return new ParameterDescription
                   {
                       Name = name,
                       SemanticName = semanticName,
                       Type = ShaderVariableType.Float,
                       Class = ShaderVariableClass.Vector,
                       Rows = 1,
                       Columns = columns,
                       InterpolationMode = interpolationMode,
                       Flags = parameterFlags,
                       FirstInRegister = 0,
                       FirstInComponent = 0,
                       FirstOutRegister = 0,
                       FirstOutComponent = 0
                   };
        }
    }
}
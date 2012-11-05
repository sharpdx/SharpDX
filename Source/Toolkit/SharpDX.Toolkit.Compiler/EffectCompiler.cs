// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.IO;
using SharpDX.Toolkit.Diagnostics;

namespace SharpDX.Toolkit.Graphics
{
    public delegate Stream IncludeFileDelegate(bool isSystemInclude, string file);

    /// <summary>
    /// Main class used to compile a Toolkit FX file.
    /// </summary>
    public class EffectCompiler
    {
        private static readonly Dictionary<string, ValueConverter> ValueConverters = new Dictionary<string, ValueConverter>()
                                                                           {
                                                                               {
                                                                                   "float4",
                                                                                   new ValueConverter("float4", 4, ToFloat,
                                                                                                      (compiler, parameters) => new Vector4((float) parameters[0], (float) parameters[1], (float) parameters[2], (float) parameters[3]))
                                                                               },
                                                                               {
                                                                                   "float3",
                                                                                   new ValueConverter("float3", 3, ToFloat, (compiler, parameters) => new Vector3((float) parameters[0], (float) parameters[1], (float) parameters[2]))
                                                                               },
                                                                               {"float2", new ValueConverter("float2", 2, ToFloat, (compiler, parameters) => new Vector2((float) parameters[0], (float) parameters[1]))},
                                                                               {"float", new ValueConverter("float", 1, ToFloat, (compiler, parameters) => (float) parameters[0])},
                                                                           };

        private readonly List<string> currentExports = new List<string>();
        private EffectCompilerFlags compilerFlags;
        private EffectData.Effect effect;
        private EffectData effectData;

        private List<string> includeDirectoryList;
        private IncludeFileDelegate includeFileDelegate;
        private Include includeHandler;
        private FeatureLevel level;
        private EffectCompilerLogger logger;
        private List<ShaderMacro> macros;
        private EffectParserResult parserResult;
        private EffectData.Pass pass;
        private string preprocessorText;
        private EffectData.Technique technique;
        private int nextSubPassCount;

        private EffectCompiler()
        {
        }

        /// <summary>
        /// Compiles an effect from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="macros">The macros.</param>
        /// <param name="includeDirectoryList">The include directory list.</param>
        /// <param name="includeFileDelegate">The include file delegate.</param>
        /// <returns>The result of compilation.</returns>
        public static EffectCompilerResult CompileFromFile(string filePath, EffectCompilerFlags flags = EffectCompilerFlags.None, List<ShaderMacro> macros = null, List<string> includeDirectoryList = null,
                                                   IncludeFileDelegate includeFileDelegate = null)
        {
            return Compile(NativeFile.ReadAllText(filePath), filePath, flags, macros, includeDirectoryList, includeFileDelegate);
        }

        /// <summary>
        /// Compiles an effect from the specified source code and filepath.
        /// </summary>
        /// <param name="sourceCode">The source code.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="macros">The macros.</param>
        /// <param name="includeDirectoryList">The include directory list.</param>
        /// <param name="includeFileDelegate">The include file delegate.</param>
        /// <returns>The result of compilation.</returns>
        public static EffectCompilerResult Compile(string sourceCode, string filePath, EffectCompilerFlags flags = EffectCompilerFlags.None, List<ShaderMacro> macros = null, List<string> includeDirectoryList = null,
                                                   IncludeFileDelegate includeFileDelegate = null)
        {
            var compiler = new EffectCompiler
                               {
                                   compilerFlags = flags,
                                   macros = macros ?? new List<ShaderMacro>(),
                                   includeDirectoryList = includeDirectoryList ?? new List<string>(),
                                   includeFileDelegate = includeFileDelegate
                               };
            return new EffectCompilerResult(compiler.InternalCompile(sourceCode, filePath), compiler.logger);
        }

        /// <summary>
        /// Disassembles a shader HLSL bytecode to asm code.
        /// </summary>
        /// <param name="shader">The shader.</param>
        /// <returns>A string containing asm code decoded from HLSL bytecode.</returns>
        public static string DisassembleShader(EffectData.Shader shader)
        {
            return new ShaderBytecode(shader.Bytecode).Disassemble(DisassemblyFlags.EnableColorCode | DisassemblyFlags.EnableInstructionNumbering);
        }

        private EffectData InternalCompile(string sourceCode, string fileName)
        {
            fileName = fileName.Replace(@"\\", @"\");

            logger = new EffectCompilerLogger();
            var parser = new EffectParser { Logger = logger, IncludeFileCallback = includeFileDelegate};
            parser.Macros.AddRange(macros);
            parser.IncludeDirectoryList.AddRange(includeDirectoryList);

            parserResult = parser.Parse(sourceCode, fileName);

            // Get back include handler.
            includeHandler = parser.includeHandler;

            if (logger.HasErrors)
                return null;

            effectData = new EffectData
                                 {
                                     Effects = new List<EffectData.Effect>(),
                                     Shaders = new List<EffectData.Shader>()
                                 };

            effect = new EffectData.Effect()
                         {
                             Name = Path.GetFileNameWithoutExtension(fileName),
                             Techniques = new List<EffectData.Technique>()
                         };

            effectData.Effects.Add(effect);

            if (parserResult.Shader != null)
            {
                foreach (var techniqueAst in parserResult.Shader.Techniques)
                {
                    HandleTechnique(techniqueAst);
                }
            }
            return effectData;
        }

        private void HandleTechnique(Ast.Technique techniqueAst)
        {
            technique = new EffectData.Technique()
                            {
                                Name = techniqueAst.Name,
                                Passes = new List<EffectData.Pass>()
                            };

            // Check that the name of this technique is not already used.
            if (technique.Name != null)
            {
                foreach (var registeredTechnique in effect.Techniques)
                {
                    if (registeredTechnique.Name == technique.Name)
                    {
                        logger.Error("A technique with the same name [{0}] already exist", techniqueAst.Span, technique.Name);
                        break;
                    }
                }
            }

            // Adds the technique to the list of technique for the current effect
            effect.Techniques.Add(technique);

            // Process passes for this technique
            foreach (var passAst in techniqueAst.Passes)
            {
                HandlePass(passAst);
            }
        }

        private void HandlePass(Ast.Pass passAst)
        {
            pass = new EffectData.Pass()
                       {
                           Name = passAst.Name,
                           Pipeline = new EffectData.Pipeline(),
                           Attributes = new List<EffectData.Attribute>()
                       };

            // Clear current exports
            currentExports.Clear();

            // Check that the name of this pass is not already used.
            if (pass.Name != null)
            {
                foreach (var registeredPass in technique.Passes)
                {
                    if (registeredPass.Name == pass.Name)
                    {
                        logger.Error("A pass with the same name [{0}] already exist", passAst.Span, pass.Name);
                        break;
                    }
                }
            }

            // Adds the pass to the list of pass for the current technique
            technique.Passes.Add(pass);

            if (nextSubPassCount > 0)
            {
                pass.IsSubPass = true;
                nextSubPassCount--;
            }

            // Process all statements inside this pass.
            foreach (var statement in passAst.Statements)
            {
                var expressionStatement = statement as Ast.ExpressionStatement;
                if (expressionStatement == null)
                    continue;
                HandleExpression(expressionStatement.Expression);
            }
        }

        private void HandleExpression(Ast.Expression expression)
        {
            if (expression is Ast.AssignExpression)
            {
                HandleAssignExpression((Ast.AssignExpression) expression);
            }
            else if (expression is Ast.MethodExpression)
            {
                HandleMethodExpression((Ast.MethodExpression) expression);
            }
            else
            {
                logger.Error("Unsupported expression type [{0}]", expression.Span, expression.GetType().Name);
            }
        }

        private void HandleAssignExpression(Ast.AssignExpression expression)
        {
            switch (expression.Name.Text)
            {
                case "Export":
                    HandleExport(expression.Value);
                    break;

                case EffectData.Attribute.Blending:
                case EffectData.Attribute.DepthStencil:
                case EffectData.Attribute.Rasterizer:
                    HandleAttribute<string>(expression);
                    break;

                case EffectData.Attribute.BlendingColor:
                    HandleAttribute<Vector4>(expression);
                    break;

                case EffectData.Attribute.BlendingSampleMask:
                    HandleAttribute<uint>(expression);
                    break;

                case EffectData.Attribute.DepthStencilReference:
                    HandleAttribute<int>(expression);
                    break;
                case "ShareConstantBuffers":
                    HandleShareConstantBuffers(expression.Value);
                    break;
                case "EffectName":
                    HandleEffectName(expression.Value);
                    break;
                case "SubPassCount":
                    HandleSubPassCount(expression.Value);
                    break;
                case "Preprocessor":
                    HandlePreprocessor(expression.Value);
                    break;
                case "Profile":
                    HandleProfile(expression.Value);
                    break;
                case "VertexShader":
                    CompileShader(EffectShaderType.Vertex, expression.Value);
                    break;
                case "PixelShader":
                    CompileShader(EffectShaderType.Pixel, expression.Value);
                    break;
                case "GeometryShader":
                    CompileShader(EffectShaderType.Geometry, expression.Value);
                    break;
                case "DomainShader":
                    CompileShader(EffectShaderType.Domain, expression.Value);
                    break;
                case "HullShader":
                    CompileShader(EffectShaderType.Hull, expression.Value);
                    break;
                case "ComputeShader":
                    CompileShader(EffectShaderType.Compute, expression.Value);
                    break;
                default:
                    HandleAttribute(expression);
                    break;
            }
        }

        private void HandleExport(Ast.Expression expression)
        {
            object value;
            if (!ExtractValue(expression, out value))
                return;

            if (value is string)
            {
                currentExports.Add((string) value);
            }
            else if (value is object[])
            {
                var arrayValue = (object[]) value;
                foreach (var exportItem in arrayValue)
                {
                    if (!(exportItem is string))
                    {
                        logger.Error("Unexpected value [{0}]. Expecting a string.", expression.Span, exportItem);
                        return;
                    }
                    currentExports.Add((string) exportItem);
                }
            }
            else
            {
                logger.Error("Unexpected value. Expecting a identifier/string or an array of identifier/string.", expression.Span);
            }
        }

        private void HandleShareConstantBuffers(Ast.Expression expression)
        {
            object value;
            if (!ExtractValue(expression, out value))
                return;

            if (!(value is bool))
            {
                logger.Error("ShareConstantBuffers must be a bool", expression.Span);
            }
            else
            {
                effect.ShareConstantBuffers = (bool)value;
            }
        }

        private void HandleEffectName(Ast.Expression expression)
        {
            object value;
            if (!ExtractValue(expression, out value))
                return;

            if (!(value is string))
            {
                logger.Error("Effect name must be a string/identifier", expression.Span);
            }
            else
            {
                effect.Name = (string) value;
            }
        }

        private void HandleSubPassCount(Ast.Expression expression)
        {
            object value;
            if (!ExtractValue(expression, out value))
                return;

            if (!(value is int))
            {
                logger.Error("SubPassCount must be an integer", expression.Span);
            }
            else
            {
                nextSubPassCount = (int)value;
            }
        }

        private void HandlePreprocessor(Ast.Expression expression)
        {
            object value;
            if (!ExtractValue(expression, out value))
                return;

            // If null, then preprocessor is resetted
            if (value == null)
                preprocessorText = null;

            // Else parse preprocessor directive
            var builder = new StringBuilder();
            if (value is string)
            {
                builder.AppendLine((string) value);
            }
            else if (value is object[])
            {
                var arrayValue = (object[]) value;
                foreach (var stringItem in arrayValue)
                {
                    if (!(stringItem is string))
                    {
                        logger.Error("Unexpected type. Preprocessor only support strings in array declaration", expression.Span);
                    }
                    builder.AppendLine((string) stringItem);
                }
            }
            preprocessorText = builder.ToString();
        }

        private void HandleAttribute(Ast.AssignExpression expression)
        {
            // Extract the value and store it in the attribute
            object value;
            if (ExtractValue(expression.Value, out value))
            {
                var attribute = new EffectData.Attribute() {Name = expression.Name.Text, Value = value};
                pass.Attributes.Add(attribute);
            }
        }

        private void HandleAttribute<T>(Ast.AssignExpression expression)
        {
            // Extract the value and store it in the attribute
            object value;
            if (ExtractValue(expression.Value, out value))
            {
                if (typeof(T) == typeof(uint) && value is int)
                {
                    value = unchecked((uint) (int) value);
                }
                else
                {
                    try
                    {
                        value = Convert.ChangeType(value, typeof (T));
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Invalid type for attribute [{0}]. Expecting [{1}]", expression.Value.Span, expression.Name.Text, typeof (T).Name);
                    }
                }

                var attribute = new EffectData.Attribute() { Name = expression.Name.Text, Value = value };
                pass.Attributes.Add(attribute);
            }
        }

        private bool ExtractValue(Ast.Expression expression, out object value)
        {
            value = null;
            if (expression is Ast.LiteralExpression)
            {
                value = ((Ast.LiteralExpression) expression).Value.Value;
            }
            else if (expression is Ast.IdentifierExpression)
            {
                value = ((Ast.IdentifierExpression) expression).Name.Text;
            }
            else if (expression is Ast.ArrayInitializerExpression)
            {
                var arrayExpression = (Ast.ArrayInitializerExpression) expression;
                var arrayValue = new object[arrayExpression.Values.Count];
                for (int i = 0; i < arrayValue.Length; i++)
                {
                    if (!ExtractValue(arrayExpression.Values[i], out arrayValue[i]))
                        return false;
                }
                value = arrayValue;
            }
            else if (expression is Ast.MethodExpression)
            {
                var methodExpression = (Ast.MethodExpression) expression;
                value = ExtractValueFromMethodExpression(methodExpression);
                if (value == null)
                {
                    logger.Error("Unable to convert method expression to value.", expression.Span);
                    return false;
                }
            }
            else
            {
                logger.Error("Unsupported value type. Only [literal, identifier, array]", expression.Span);
                return false;
            }

            return true;
        }

        private object ExtractValueFromMethodExpression(Ast.MethodExpression methodExpression)
        {
            ValueConverter converter;
            if (!ValueConverters.TryGetValue(methodExpression.Name.Text, out converter))
            {
                logger.Error("Unexpected method [{0}]", methodExpression.Span, methodExpression.Name.Text);
                return null;
            }

            // Check for arguments
            if (converter.ArgumentCount != methodExpression.Arguments.Count)
            {
                logger.Error("Unexpected number of arguments (expecting {0} arguments)", methodExpression.Span, converter.ArgumentCount);
                return null;
            }

            var values = new object[methodExpression.Arguments.Count];
            for (int i = 0; i < methodExpression.Arguments.Count; i++)
            {
                object localValue;
                if (!ExtractValue(methodExpression.Arguments[i], out localValue))
                    return null;
                values[i] = converter.ConvertItem(this, localValue);
                if (localValue == null)
                    return null;
            }

            return converter.ConvertFullItem(this, values);
        }

        private void ProfileToFeatureLevel(string expectedPrefix, string profile, out FeatureLevel level)
        {
            level = 0;
            if (profile.StartsWith(expectedPrefix))
            {
                var shortProfile = profile.Substring(expectedPrefix.Length);

                switch (shortProfile)
                {
                    case "2_0":
                        level = FeatureLevel.Level_9_1;
                        break;
                    case "3_0":
                        level = FeatureLevel.Level_9_3;
                        break;
                    case "4_0_level_9_1":
                        level = FeatureLevel.Level_9_1;
                        break;
                    case "4_0_level_9_2":
                        level = FeatureLevel.Level_9_2;
                        break;
                    case "4_0_level_9_3":
                        level = FeatureLevel.Level_9_3;
                        break;
                    case "4_0":
                        level = FeatureLevel.Level_10_0;
                        break;
                    case "4_1":
                        level = FeatureLevel.Level_10_1;
                        break;
                    case "5_0":
                        level = FeatureLevel.Level_11_0;
                        break;
#if DIRECTX11_1
                    case "5_1":
                        level = FeatureLevel.Level_11_1;
                        break;
#endif
                }
            }

            if (level == 0)
                logger.Error("Unsupported profile [{0}]", profile);
        }

        private void HandleProfile(Ast.Expression expression)
        {
            var identifierExpression = expression as Ast.IdentifierExpression;
            if (identifierExpression != null)
            {
                var profile = identifierExpression.Name.Text;
                ProfileToFeatureLevel("fx_", profile, out level);
            }
            else if (expression is Ast.LiteralExpression)
            {
                var literalValue = ((Ast.LiteralExpression) expression).Value.Value;
                try
                {
                    var rawLevel = (int) (Convert.ToSingle(literalValue, CultureInfo.InvariantCulture) * 10);
                    switch (rawLevel)
                    {
                        case 91:
                            level = FeatureLevel.Level_9_1;
                            break;
                        case 92:
                            level = FeatureLevel.Level_9_2;
                            break;
                        case 93:
                            level = FeatureLevel.Level_9_3;
                            break;
                        case 100:
                            level = FeatureLevel.Level_10_0;
                            break;
                        case 101:
                            level = FeatureLevel.Level_10_1;
                            break;
                        case 110:
                            level = FeatureLevel.Level_11_0;
                            break;
#if DIRECTX11_1
                        case 111:
                            level = FeatureLevel.Level_11_1;
                            break;
#endif
                    }
                }
                catch (Exception ex)
                {
                }
            }

            if (level == 0)
                logger.Error("Unexpected assignement for [Profile] attribute: expecting only [identifier (fx_4_0, fx_4_1... etc.), or number (9.3, 10.0, 11.0... etc.)]", expression.Span);
        }

        private string ExtractShaderName(EffectShaderType effectShaderType, Ast.Expression expression)
        {
            string shaderName = null;

            if (expression is Ast.IdentifierExpression)
            {
                shaderName = ((Ast.IdentifierExpression) expression).Name.Text;
            }
            else if (expression is Ast.LiteralExpression)
            {
                var value = ((Ast.LiteralExpression) expression).Value.Value;
                if (Equals(value, 0) || Equals(value, null))
                {
                    return null;
                }
            }
            else if (expression is Ast.CompileExpression)
            {
                var compileExpression = (Ast.CompileExpression) expression;

                var profileName = compileExpression.Profile.Text;

                ProfileToFeatureLevel(StageTypeToString(effectShaderType) + "_", profileName, out level);

                if (compileExpression.Method is Ast.MethodExpression)
                {
                    var shaderMethod = (Ast.MethodExpression)compileExpression.Method;

                    if (shaderMethod.Arguments.Count > 0)
                    {
                        logger.Error("Default arguments for shader methods are not supported", shaderMethod.Span);
                        return null;
                    }

                    shaderName = shaderMethod.Name.Text;
                }
                else
                {
                    logger.Error("Unsupported expression for compile. Excepting method", expression.Span);
                    return null;
                }
            }
            else if (expression is Ast.MethodExpression)
            {
                // CompileShader( vs_4_0, VS() )
                var compileExpression = (Ast.MethodExpression) expression;

                if (compileExpression.Name.Text == "CompileShader")
                {
                    if (compileExpression.Arguments.Count != 2)
                    {
                        logger.Error("Unexpected number of arguments [{0}] instead of 2", expression.Span, compileExpression.Arguments.Count);
                        return null;
                    }

                    // Extract level (11.0 10.0) from profile name (
                    object profileName;
                    if (!ExtractValue(compileExpression.Arguments[0], out profileName))
                        return null;

                    if (!(profileName is string))
                    {
                        logger.Error("Invalid profile [{0}]. Expecting identifier", compileExpression.Arguments[0].Span, profileName);
                        return null;
                    }

                    ProfileToFeatureLevel(StageTypeToString(effectShaderType) + "_", (string) profileName, out level);


                    var shaderMethod = compileExpression.Arguments[1] as Ast.MethodExpression;
                    if (shaderMethod == null)
                    {
                        logger.Error("Unexpected expression. Only method expression are supported", compileExpression.Arguments[1].Span);
                        return null;
                    }

                    if (shaderMethod.Arguments.Count > 0)
                    {
                        logger.Error("Default arguments for shader methods are not supported", shaderMethod.Span);
                        return null;
                    }

                    shaderName = shaderMethod.Name.Text;
                }
            }

            if (shaderName == null)
            {
                logger.Error("Unexpected compile expression", expression.Span);
            }

            return shaderName;
        }

        private void CompileShader(EffectShaderType type, Ast.Expression assignValue)
        {
            CompileShader(type, ExtractShaderName(type, assignValue), assignValue.Span);
        }

        private void CompileShader(EffectShaderType type, string shaderName, SourceSpan span)
        {
            var level = this.level;
            if (shaderName == null)
            {
                pass.Pipeline[type] = EffectData.ShaderLink.NullShader;
                return;
            }

            //// If the shader has been already compiled, skip it
            //foreach (var shader in EffectData.Shaders)
            //{
            //    if (shader.Name == shaderName)
            //        return;
            //}

            // If the level is not setup, return an error
            if (level == 0)
            {
                logger.Error("Expecting setup of [Profile = fx_4_0/fx_5_0...etc.] before compiling a shader.", span);
                return;
            }

            string profile = StageTypeToString(type) + "_";
            switch (this.level)
            {
                case FeatureLevel.Level_9_1:
                    profile += "4_0_level_9_1";
                    break;
                case FeatureLevel.Level_9_2:
                    profile += "4_0_level_9_2";
                    break;
                case FeatureLevel.Level_9_3:
                    profile += "4_0_level_9_3";
                    break;
                case FeatureLevel.Level_10_0:
                    profile += "4_0";
                    break;
                case FeatureLevel.Level_10_1:
                    profile += "4_1";
                    break;
                case FeatureLevel.Level_11_0:
                    profile += "5_0";
                    break;
#if DIRECTX11_1
                case FeatureLevel.Level_11_1:
                    profile += "5_1";
                    break;
#endif
            }

            var saveState = Configuration.ThrowOnShaderCompileError;
            Configuration.ThrowOnShaderCompileError = false;
            try
            {
                var sourcecodeBuilder = new StringBuilder();
                if (!string.IsNullOrEmpty(preprocessorText))
                    sourcecodeBuilder.Append(preprocessorText);
                sourcecodeBuilder.Append(parserResult.PreprocessedSource);

                var sourcecode = sourcecodeBuilder.ToString();

                var result = ShaderBytecode.Compile(sourcecode, shaderName, profile, (ShaderFlags) compilerFlags, D3DCompiler.EffectFlags.None, null, includeHandler, parserResult.SourceFileName);

                var compilerMessages = result.Message;
                if (compilerMessages != null)
                {
                    compilerMessages = compilerMessages.Replace(@"\\", @"\");
                }

                if (result.HasErrors)
                {
                    logger.LogMessage(new LogMessageRaw(LogMessageType.Error, compilerMessages));
                }
                else
                {
                    if (!string.IsNullOrEmpty(compilerMessages))
                    {
                        logger.LogMessage(new LogMessageRaw(LogMessageType.Warning, compilerMessages));
                    }

                    // Check if this shader is exported
                    if (currentExports.Contains(shaderName))
                    {
                        // the exported name is EffectName::ShaderName
                        shaderName = effect.Name + "::" + shaderName;
                    }
                    else
                    {
                        // If the shader is not exported, set the name to null
                        shaderName = null;
                    }

                    var shader = new EffectData.Shader()
                                     {
                                         Name = shaderName,
                                         Level = this.level,
                                         Bytecode = result.Bytecode,
                                         Type = type,
                                         ConstantBuffers = new List<EffectData.ConstantBuffer>(),
                                         ResourceParameters = new List<EffectData.ResourceParameter>(),
                                         InputSignature = new EffectData.Signature(),
                                         OutputSignature = new EffectData.Signature()
                                     };

                    using (var reflect = new ShaderReflection(shader.Bytecode))
                    {
                        BuiltSemanticInputAndOutput(shader, reflect);
                        BuildParameters(shader, reflect);
                    }

                    // Strip reflection datas, as we are storing them in the toolkit format.
                    var newBytecode = result.Bytecode.Strip(StripFlags.CompilerStripReflectionData);
                    shader.Bytecode = newBytecode;

                    // Compute Hashcode
                    newBytecode = newBytecode.Strip(StripFlags.CompilerStripTestBlobs | StripFlags.CompilerStripReflectionData | StripFlags.CompilerStripDebugInformation);
                    shader.Hashcode = Utilities.ComputeHashFNVModified(newBytecode);

                    // Check that this shader was not already generated
                    int shaderIndex;
                    for (shaderIndex = 0; shaderIndex < effectData.Shaders.Count; shaderIndex++)
                    {
                        var shaderItem = effectData.Shaders[shaderIndex];
                        if (shaderItem.IsSimilar(shader))
                        {
                            break;
                        }
                    }

                    // Check if there is a new shader to store in the archive
                    if (shaderIndex >= effectData.Shaders.Count)
                    {
                        shaderIndex = effectData.Shaders.Count;

                        // If this is a Vertex shader, compute the binary signature for the input layout
                        if (shader.Type == EffectShaderType.Vertex)
                        {
                            // Gets the signature from the stripped bytecode and compute the hashcode.
                            shader.InputSignature.Bytecode = ShaderSignature.GetInputSignature(newBytecode);
                            shader.InputSignature.Hashcode = Utilities.ComputeHashFNVModified(shader.InputSignature.Bytecode);
                        }

                        effectData.Shaders.Add(shader);
                    }

                    pass.Pipeline[type] = new EffectData.ShaderLink(shaderIndex);
                }
            }
            finally
            {
                Configuration.ThrowOnShaderCompileError = saveState;
            }
        }

        private void HandleMethodExpression(Ast.MethodExpression expression)
        {
            if (expression.Arguments.Count == 1)
            {
                var argument = expression.Arguments[0];

                switch (expression.Name.Text)
                {
                    case "SetVertexShader":
                        CompileShader(EffectShaderType.Vertex, argument);
                        break;
                    case "SetPixelShader":
                        CompileShader(EffectShaderType.Pixel, argument);
                        break;
                    case "SetGeometryShader":
                        CompileShader(EffectShaderType.Geometry, argument);
                        break;
                    case "SetDomainShader":
                        CompileShader(EffectShaderType.Domain, argument);
                        break;
                    case "SetHullShader":
                        CompileShader(EffectShaderType.Hull, argument);
                        break;
                    case "SetComputeShader":
                        CompileShader(EffectShaderType.Compute, argument);
                        break;
                    default:
                        logger.Warning("Unhandled method [{0}]", expression.Span, expression.Name);
                        break;
                }
            }
            else
            {
                logger.Warning("Unhandled method [{0}]", expression.Span, expression.Name);
            }
        }

        private static string StageTypeToString(EffectShaderType type)
        {
            string profile = null;
            switch (type)
            {
                case EffectShaderType.Vertex:
                    profile = "vs";
                    break;
                case EffectShaderType.Domain:
                    profile = "ds";
                    break;
                case EffectShaderType.Hull:
                    profile = "hs";
                    break;
                case EffectShaderType.Geometry:
                    profile = "gs";
                    break;
                case EffectShaderType.Pixel:
                    profile = "ps";
                    break;
                case EffectShaderType.Compute:
                    profile = "cs";
                    break;
            }
            return profile;
        }


        private void BuiltSemanticInputAndOutput(EffectData.Shader shader, ShaderReflection reflect)
        {
            var description = reflect.Description;
            shader.InputSignature.Semantics = new EffectData.Semantic[description.InputParameters];
            for (int i = 0; i < description.InputParameters; i++)
                shader.InputSignature.Semantics[i] = ConvertToSemantic(reflect.GetInputParameterDescription(i));

            shader.OutputSignature.Semantics = new EffectData.Semantic[description.OutputParameters];
            for (int i = 0; i < description.OutputParameters; i++)
                shader.OutputSignature.Semantics[i] = ConvertToSemantic(reflect.GetOutputParameterDescription(i));
        }

        private EffectData.Semantic ConvertToSemantic(ShaderParameterDescription shaderParameterDescription)
        {
            return new EffectData.Semantic(
                shaderParameterDescription.SemanticName,
                (byte) shaderParameterDescription.SemanticIndex,
                (byte) shaderParameterDescription.Register,
                (byte) shaderParameterDescription.SystemValueType,
                (byte) shaderParameterDescription.ComponentType,
                (byte) shaderParameterDescription.UsageMask,
                (byte) shaderParameterDescription.ReadWriteMask,
                (byte) shaderParameterDescription.Stream
                );
        }

        /// <summary>
        ///   Builds the parameters for a particular shader.
        /// </summary>
        /// <param name="shader"> The shader to build parameters. </param>
        private void BuildParameters(EffectData.Shader shader, ShaderReflection reflect)
        {
            var description = reflect.Description;


            // Iterate on all Constant buffers used by this shader
            // Build all ParameterBuffers
            for (int i = 0; i < description.ConstantBuffers; i++)
            {
                var reflectConstantBuffer = reflect.GetConstantBuffer(i);
                var reflectConstantBufferDescription = reflectConstantBuffer.Description;

                // Skip non pure constant-buffers and texture buffers
                if (reflectConstantBufferDescription.Type != ConstantBufferType.ConstantBuffer
                    && reflectConstantBufferDescription.Type != ConstantBufferType.TextureBuffer)
                    continue;

                // Create the buffer
                var parameterBuffer = new EffectData.ConstantBuffer()
                                          {
                                              Name = reflectConstantBufferDescription.Name,
                                              Size = reflectConstantBufferDescription.Size,
                                              Parameters = new List<EffectData.ValueTypeParameter>(),
                                          };
                shader.ConstantBuffers.Add(parameterBuffer);

                // Iterate on each variable declared inside this buffer
                for (int j = 0; j < reflectConstantBufferDescription.VariableCount; j++)
                {
                    var variableBuffer = reflectConstantBuffer.GetVariable(j);

                    // Build constant buffer parameter
                    var parameter = BuildConstantBufferParameter(variableBuffer);

                    // Add this parameter to the ConstantBuffer
                    parameterBuffer.Parameters.Add(parameter);
                }
            }

            // Iterate on all resources bound in order to resolve resouce dependencies for this shader.
            // If the shader is dependent from an object variable, then create this variable as well.
            for (int i = 0; i < description.BoundResources; i++)
            {
                var bindingDescription = reflect.GetResourceBindingDescription(i);
                string name = bindingDescription.Name;

                // Build resource parameter
                var parameter = BuildResourceParameter(name, bindingDescription);
                shader.ResourceParameters.Add(parameter);
            }
        }

        /// <summary>
        ///   Builds an effect parameter from a reflection variable.
        /// </summary>
        /// <returns> an EffectParameter, null if not handled </returns>
        private EffectData.ValueTypeParameter BuildConstantBufferParameter(ShaderReflectionVariable variable)
        {
            var variableType = variable.GetVariableType();
            var variableDescription = variable.Description;
            var variableTypeDescription = variableType.Description;

            var parameter = new EffectData.ValueTypeParameter()
                                {
                                    Name = variableDescription.Name,
                                    Offset = variableDescription.StartOffset,
                                    Size = variableDescription.Size,
                                    Count = variableTypeDescription.ElementCount,
                                    Class = (EffectParameterClass) variableTypeDescription.Class,
                                    Type = (EffectParameterType) variableTypeDescription.Type,
                                    RowCount = (byte) variableTypeDescription.RowCount,
                                    ColumnCount = (byte) variableTypeDescription.ColumnCount,
                                };

            if (variableDescription.DefaultValue != IntPtr.Zero)
            {
                parameter.DefaultValue = new byte[variableDescription.Size];
                Utilities.Read(variableDescription.DefaultValue, parameter.DefaultValue, 0, parameter.DefaultValue.Length);
            }

            return parameter;
        }

        /// <summary>
        ///   Builds an effect parameter from a reflection variable.
        /// </summary>
        /// <returns> an EffectParameter, null if not handled </returns>
        private static EffectData.ResourceParameter BuildResourceParameter(string name, InputBindingDescription variableBinding)
        {
            var parameter = new EffectData.ResourceParameter()
                                {
                                    Name = name,
                                    Class = EffectParameterClass.Object,
                                    Slot = (byte) variableBinding.BindPoint,
                                    Count = (byte) variableBinding.BindCount,
                                };

            switch (variableBinding.Type)
            {
                case ShaderInputType.TextureBuffer:
                    parameter.Type = EffectParameterType.TextureBuffer;
                    break;
                case ShaderInputType.ConstantBuffer:
                    parameter.Type = EffectParameterType.ConstantBuffer;
                    break;
                case ShaderInputType.Texture:
                    switch (variableBinding.Dimension)
                    {
                        case ShaderResourceViewDimension.Buffer:
                            parameter.Type = EffectParameterType.Buffer;
                            break;
                        case ShaderResourceViewDimension.Texture1D:
                            parameter.Type = EffectParameterType.Texture1D;
                            break;
                        case ShaderResourceViewDimension.Texture1DArray:
                            parameter.Type = EffectParameterType.Texture1DArray;
                            break;
                        case ShaderResourceViewDimension.Texture2D:
                            parameter.Type = EffectParameterType.Texture2D;
                            break;
                        case ShaderResourceViewDimension.Texture2DArray:
                            parameter.Type = EffectParameterType.Texture2DArray;
                            break;
                        case ShaderResourceViewDimension.Texture2DMultisampled:
                            parameter.Type = EffectParameterType.Texture2DMultisampled;
                            break;
                        case ShaderResourceViewDimension.Texture2DMultisampledArray:
                            parameter.Type = EffectParameterType.Texture2DMultisampledArray;
                            break;
                        case ShaderResourceViewDimension.Texture3D:
                            parameter.Type = EffectParameterType.Texture3D;
                            break;
                        case ShaderResourceViewDimension.TextureCube:
                            parameter.Type = EffectParameterType.TextureCube;
                            break;
                        case ShaderResourceViewDimension.TextureCubeArray:
                            parameter.Type = EffectParameterType.TextureCubeArray;
                            break;
                    }
                    break;
                case ShaderInputType.Structured:
                    parameter.Type = EffectParameterType.StructuredBuffer;
                    break;
                case ShaderInputType.Byteaddress:
                    parameter.Type = EffectParameterType.ByteAddressBuffer;
                    break;
                case ShaderInputType.UnorderedAccessViewRwtyped:
                    switch (variableBinding.Dimension)
                    {
                        case ShaderResourceViewDimension.Buffer:
                            parameter.Type = EffectParameterType.RWBuffer;
                            break;
                        case ShaderResourceViewDimension.Texture1D:
                            parameter.Type = EffectParameterType.RWTexture1D;
                            break;
                        case ShaderResourceViewDimension.Texture1DArray:
                            parameter.Type = EffectParameterType.RWTexture1DArray;
                            break;
                        case ShaderResourceViewDimension.Texture2D:
                            parameter.Type = EffectParameterType.RWTexture2D;
                            break;
                        case ShaderResourceViewDimension.Texture2DArray:
                            parameter.Type = EffectParameterType.RWTexture2DArray;
                            break;
                        case ShaderResourceViewDimension.Texture3D:
                            parameter.Type = EffectParameterType.RWTexture3D;
                            break;
                    }
                    break;
                case ShaderInputType.UnorderedAccessViewRwstructured:
                    parameter.Type = EffectParameterType.RWStructuredBuffer;
                    break;
                case ShaderInputType.UnorderedAccessViewRWByteAddress:
                    parameter.Type = EffectParameterType.RWByteAddressBuffer;
                    break;
                case ShaderInputType.UnorderedAccessViewAppendStructured:
                    parameter.Type = EffectParameterType.AppendStructuredBuffer;
                    break;
                case ShaderInputType.UnorderedAccessViewConsumeStructured:
                    parameter.Type = EffectParameterType.ConsumeStructuredBuffer;
                    break;
                case ShaderInputType.UnorderedAccessViewRwstructuredWithCounter:
                    parameter.Type = EffectParameterType.StructuredBuffer;
                    break;
                case ShaderInputType.Sampler:
                    parameter.Type = EffectParameterType.Sampler;
                    break;
            }
            return parameter;
        }

        private static object ToFloat(EffectCompiler compiler, object value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch (Exception ex)
            {
                compiler.logger.Error("Unable to convert [{0}] to float", new SourceSpan(), value);
            }
            return null;
        }

        #region Nested type: ConvertFullItem

        private delegate object ConvertFullItem(EffectCompiler compiler, object[] parameters);

        #endregion

        #region Nested type: ConvertItem

        private delegate object ConvertItem(EffectCompiler compiler, object value);

        #endregion

        #region Nested type: ValueConverter

        private struct ValueConverter
        {
            public int ArgumentCount;
            public ConvertFullItem ConvertFullItem;
            public ConvertItem ConvertItem;

            public ValueConverter(string name, int argumentCount, ConvertItem convertItem, ConvertFullItem convertFullItem)
            {
                ArgumentCount = argumentCount;
                ConvertItem = convertItem;
                ConvertFullItem = convertFullItem;
            }
        }

        #endregion
    }
}
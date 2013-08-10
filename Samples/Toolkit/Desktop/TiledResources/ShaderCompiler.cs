namespace TiledResources
{
    using System;
    using SharpDX.D3DCompiler;

    /// <summary>
    /// Helper class for dynamic shader compilation
    /// </summary>
    /// <remarks>Assumes that entry points are 'main' and shader profile is 'xx_5_0'.</remarks>
    internal static class ShaderCompiler
    {
        /// <summary>
        /// Compiles a pixel shader 
        /// </summary>
        /// <param name="path">The full path to the shader source code.</param>
        /// <returns>The compiled shader bytecode.</returns>
        public static ShaderBytecode CompilePixelShader(string path)
        {
            return Compile(path, "main", "ps_5_0");
        }

        /// <summary>
        /// Compiles a vertex shader 
        /// </summary>
        /// <param name="path">The full path to the shader source code.</param>
        /// <returns>The compiled shader bytecode.</returns>
        public static ShaderBytecode CompileVertexShader(string path)
        {
            return Compile(path, "main", "vs_5_0");
        }

        private static ShaderBytecode Compile(string path, string entryPoint, string profile)
        {
            var compilationResult = ShaderBytecode.CompileFromFile(path, entryPoint, profile);
            if (compilationResult.HasErrors)
            {
                var message = string.Format("Failed to compile shader {0}: {1}; {2}",
                                            path,
                                            compilationResult.ResultCode,
                                            compilationResult.Message);

                throw new ArgumentException(message);
            }

            return compilationResult.Bytecode;
        }
    }
}
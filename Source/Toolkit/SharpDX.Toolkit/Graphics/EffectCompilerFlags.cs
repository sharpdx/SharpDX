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

namespace SharpDX.Toolkit.Graphics
{
    public enum EffectCompilerFlags
    {
        /// <summary>	
        /// Directs the compiler to insert debug file/line/type/symbol information into the output code.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_DEBUG</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_DEBUG</unmanaged-short>	
        Debug = unchecked((int)1),			
        
        /// <summary>	
        /// Directs the compiler not to validate the generated code against known capabilities and constraints. 
        /// We recommend that you use this constant only with shaders that have been successfully compiled in the past. DirectX always validates shaders before it sets them to a device.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_SKIP_VALIDATION</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_SKIP_VALIDATION</unmanaged-short>	
        SkipValidation = unchecked((int)2),			
        
        /// <summary>	
        /// Directs the compiler to skip optimization steps during code generation. We recommend that you set this constant for debug purposes only.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_SKIP_OPTIMIZATION</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_SKIP_OPTIMIZATION</unmanaged-short>	
        SkipOptimization = unchecked((int)4),			
        
        /// <summary>	
        /// Directs the compiler to pack matrices in row-major order on input and output from the shader.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_PACK_MATRIX_ROW_MAJOR</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_PACK_MATRIX_ROW_MAJOR</unmanaged-short>	
        PackMatrixRowMajor = unchecked((int)8),			
        
        /// <summary>	
        /// Directs the compiler to pack matrices in column-major order on input and output from the shader. This type of packing is generally more efficient because a series of dot-products can then perform vector-matrix multiplication.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_PACK_MATRIX_COLUMN_MAJOR</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_PACK_MATRIX_COLUMN_MAJOR</unmanaged-short>	
        PackMatrixColumnMajor = unchecked((int)16),			
        
        /// <summary>	
        /// Directs the compiler to perform all computations with partial precision. If you set this constant, the compiled code might run faster on some hardware.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_PARTIAL_PRECISION</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_PARTIAL_PRECISION</unmanaged-short>	
        PartialPrecision = unchecked((int)32),			
        
        /// <summary>	
        /// Directs the compiler to not use flow-control constructs where possible.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_AVOID_FLOW_CONTROL</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_AVOID_FLOW_CONTROL</unmanaged-short>	
        AvoidFlowControl = unchecked((int)512),			
        
        /// <summary>	
        /// Directs the compiler to use flow-control constructs where possible.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_PREFER_FLOW_CONTROL</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_PREFER_FLOW_CONTROL</unmanaged-short>	
        PreferFlowControl = unchecked((int)1024),			
        
        /// <summary>	
        /// Forces strict compile, which might not allow for legacy syntax.
        /// </summary>	
        /// <remarks>
        /// By default, the compiler disables strictness on deprecated syntax.
        /// </remarks>
        /// <unmanaged>D3DCOMPILE_ENABLE_STRICTNESS</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_ENABLE_STRICTNESS</unmanaged-short>	
        EnableStrictness = unchecked((int)2048),			
        
        /// <summary>	
        /// Directs the compiler to enable older shaders to compile to 5_0 targets.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_ENABLE_BACKWARDS_COMPATIBILITY</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_ENABLE_BACKWARDS_COMPATIBILITY</unmanaged-short>	
        EnableBackwardsCompatibility = unchecked((int)4096),			
        
        /// <summary>	
        /// Forces the IEEE strict compile.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_IEEE_STRICTNESS</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_IEEE_STRICTNESS</unmanaged-short>	
        IeeeStrictness = unchecked((int)8192),			
        
        /// <summary>	
        /// Directs the compiler to use the lowest optimization level. If you set this constant, the compiler might produce slower code but produces the code more quickly. Set this constant when you develop the shader iteratively.	
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_OPTIMIZATION_LEVEL0</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_OPTIMIZATION_LEVEL0</unmanaged-short>	
        OptimizationLevel0 = unchecked((int)16384),			
        
        /// <summary>	
        /// Directs the compiler to use the second lowest optimization level.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_OPTIMIZATION_LEVEL1</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_OPTIMIZATION_LEVEL1</unmanaged-short>	
        OptimizationLevel1 = unchecked((int)0),			
        
        /// <summary>	
        /// Directs the compiler to use the second highest optimization level.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_OPTIMIZATION_LEVEL2</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_OPTIMIZATION_LEVEL2</unmanaged-short>	
        OptimizationLevel2 = unchecked((int)49152),			
        
        /// <summary>	
        /// Directs the compiler to use the highest optimization level. If you set this constant, the compiler produces the best possible code but might take significantly longer to do so. Set this constant for final builds of an application when performance is the most important factor.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_OPTIMIZATION_LEVEL3</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_OPTIMIZATION_LEVEL3</unmanaged-short>	
        OptimizationLevel3 = unchecked((int)32768),			
        
        /// <summary>	
        /// Directs the compiler to treat all warnings as errors when it compiles the shader code. We recommend that you use this constant for new shader code, so that you can resolve all warnings and lower the number of hard-to-find code defects.
        /// </summary>	
        /// <unmanaged>D3DCOMPILE_WARNINGS_ARE_ERRORS</unmanaged>	
        /// <unmanaged-short>D3DCOMPILE_WARNINGS_ARE_ERRORS</unmanaged-short>	
        WarningsAreErrors = unchecked((int)262144),			
        
        /// <summary>	
        /// None.	
        /// </summary>	
        /// <unmanaged>None</unmanaged>	
        /// <unmanaged-short>None</unmanaged-short>	
        None = unchecked((int)0),		  
    }
}
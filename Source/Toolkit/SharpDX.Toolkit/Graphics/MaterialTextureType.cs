// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
//---------------------------------------------------------------------------
// Assimp model, comments were copied from assimp.
// ---------------------------------------------------------------------------
/*
---------------------------------------------------------------------------
Open Asset Import Library (assimp)
---------------------------------------------------------------------------

Copyright (c) 2006-2012, assimp team

All rights reserved.

Redistribution and use of this software in source and binary forms, 
with or without modification, are permitted provided that the following 
conditions are met:

* Redistributions of source code must retain the above
  copyright notice, this list of conditions and the
  following disclaimer.

* Redistributions in binary form must reproduce the above
  copyright notice, this list of conditions and the
  following disclaimer in the documentation and/or other
  materials provided with the distribution.

* Neither the name of the assimp team, nor the names of its
  contributors may be used to endorse or promote products
  derived from this software without specific prior
  written permission of the assimp team.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
---------------------------------------------------------------------------
*/

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Defines the purpose of a texture. See remarks.
    /// </summary>
    /// <remarks>
    /// This is a very difficult topic. Different 3D packages support different
    /// kinds of textures. For very common texture types, such as bumpmaps, the
    /// rendering results depend on implementation details in the rendering 
    /// pipelines of these applications. Assimp loads all texture references from
    /// the model file and tries to determine which of the predefined texture
    /// types below is the best choice to match the original use of the texture
    /// as closely as possible.<br>
    /// 
    /// In content pipelines you'll usually define how textures have to be handled,
    /// and the artists working on models have to conform to this specification,
    /// regardless which 3D tool they're using.        
    /// </remarks>
    public enum MaterialTextureType : byte
    {
        /// <summary>
        /// Dummy value.
        /// </summary>
        None,
        /// <summary>
        /// The texture is combined with the result of the diffuse
        /// lighting equation.
        /// </summary>
        Diffuse,
        /// <summary>
        /// The texture is combined with the result of the specular
        /// lighting equation.
        /// </summary>
        Specular,
        /// <summary>
        /// The texture is combined with the result of the ambient
        /// lighting equation.
        /// </summary>
        Ambient,
        /// <summary>
        /// The texture is added to the result of the lighting
        /// calculation. It isn't influenced by incoming light.
        /// </summary>
        Emissive,
        /// <summary>
        /// The texture is a height map.
        /// </summary>
        /// <remarks>
        /// By convention, higher gray-scale values stand for
        /// higher elevations from the base height.
        /// </remarks>
        Height,
        /// <summary>
        /// The texture is a (tangent space) normal-map.
        /// </summary>
        /// <remarks>
        /// Again, there are several conventions for tangent-space
        /// normal maps. Assimp does (intentionally) not 
        /// distinguish here.
        /// </remarks>
        Normals,
        /// <summary>
        /// The texture defines the glossiness of the material.
        /// </summary>
        /// <remarks>
        /// The glossiness is in fact the exponent of the specular
        /// (phong) lighting equation. Usually there is a conversion
        /// function defined to map the linear color values in the
        /// texture to a suitable exponent. Have fun.
        /// </remarks>
        Shininess,
        /// <summary>
        /// The texture defines per-pixel opacity.
        /// </summary>
        /// <remarks>
        /// Usually 'white' means opaque and 'black' means 
        /// 'transparency'. Or quite the opposite. Have fun.
        /// </remarks>
        Opacity,
        /// <summary>
        /// Displacement texture
        /// </summary>
        /// <remarks>
        /// The exact purpose and format is application-dependent.
        /// Higher color values stand for higher vertex displacements.
        /// </remarks>
        Displacement,
        /// <summary>
        /// Lightmap texture (aka Ambient Occlusion)
        /// </summary>
        /// <remarks>
        /// Both 'Lightmaps' and dedicated 'ambient occlusion maps' are
        /// covered by this material property. The texture contains a
        /// scaling value for the final color value of a pixel. Its
        /// intensity is not affected by incoming light.
        /// </remarks>
        Lightmap,
        /// <summary>
        /// Reflection texture
        /// </summary>
        /// <remarks>
        /// Contains the color of a perfect mirror reflection.
        /// Rarely used, almost never for real-time applications.
        /// </remarks>
        Reflection,
        /// <summary>
        /// Unknown texture
        /// </summary>
        /// <remarks>
        /// A texture reference that does not match any of the definitions 
        /// above is considered to be 'unknown'. It is still imported,
        /// but is excluded from any further postprocessing.
        /// </remarks>
        Unknown,
    }
}
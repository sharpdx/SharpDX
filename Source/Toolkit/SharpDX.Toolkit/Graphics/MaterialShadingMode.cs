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
    /// Defines all shading models supported by the library.
    /// </summary>
    /// <remarks>
    /// The list of shading modes has been taken from Blender.
    /// See Blender documentation for more information. The API does
    /// not distinguish between "specular" and "diffuse" shaders (thus the
    /// specular term for diffuse shading models like Oren-Nayar remains
    /// undefined). <br>
    /// Again, this value is just a hint. Assimp tries to select the shader whose
    /// most common implementation matches the original rendering results of the
    /// 3D modeller which wrote a particular model as closely as possible.
    /// </remarks>
    public enum MaterialShadingMode
    {
        /// <summary>
        /// No particular shading.
        /// </summary>
        None,

        /// <summary>
        /// Flat shading. Shading is done on per-face base,
        /// diffuse only. Also known as 'faceted shading'.
        /// </summary>
        Flat,

        /// <summary>
        /// Simple Gouraud shading. 
        /// </summary>
        Gouraud,

        /// <summary>
        /// Phong-Shading.
        /// </summary>
        Phong,

        /// <summary>
        /// Phong-Blinn-Shading
        /// </summary>
        Blinn,

        /// <summary>
        /// Toon-Shading per pixel, also known as 'comic' shader.
        /// </summary>
        Toon,

        /// <summary>
        /// OrenNayar-Shading per pixel.
        /// </summary>
        /// <remarks>
        /// Extension to standard Lambertian shading, taking the    
        /// roughness of the material into account
        /// </remarks>
        OrenNayar,

        /// <summary>
        /// Minnaert-Shading per pixel
        /// </summary>
        /// <remarks>
        /// Extension to standard Lambertian shading, taking the
        /// "darkness" of the material into account
        /// </remarks>
        Minnaert,

        /// <summary>
        /// CookTorrance-Shading per pixel
        /// </summary>
        /// <remarks>
        /// Special shader for metallic surfaces.
        /// </remarks>
        CookTorrance,

        /// <summary>
        /// No shading at all. Constant light influence of 1.0.
        /// </summary>
        NoShading,

        /// <summary>
        /// Fresnel shading
        /// </summary>
        Fresnel,
    }
}
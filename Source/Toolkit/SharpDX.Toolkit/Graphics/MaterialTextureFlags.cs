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

using System;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Defines some mixed flags for a particular texture.
    /// </summary>
    /// <remarks>
    /// Usually you'll instruct your cg artists how textures have to look like ...
    /// and how they will be processed in your application. However, if you use
    /// Assimp for completely generic loading purposes you might also need to 
    /// process these flags in order to display as many 'unknown' 3D models as 
    /// possible correctly.
    /// 
    /// This corresponds to the #AI_MATKEY_TEXFLAGS property.
    /// </remarks>
    [Flags]
    public enum MaterialTextureFlags : byte
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,

        /// <summary>
        /// The texture's color values have to be inverted (component-wise 1-n)
        /// </summary>
        /// 
        Invert = 0x1,

        /// <summary>
        /// Explicit request to the application to process the alpha channel
        /// of the texture.
        /// </summary>
        /// <remarks>
	    /// Mutually exclusive with #aiTextureFlags_IgnoreAlpha. These
	    /// flags are set if the library can say for sure that the alpha
	    /// channel is used/is not used. If the model format does not
	    /// define this, it is left to the application to decide whether
	    /// the texture alpha channel - if any - is evaluated or not.
	    /// </remarks>
        aiTextureFlags_UseAlpha = 0x2,

        /// <summary>
        /// Explicit request to the application to ignore the alpha channel of the texture.
        ///  </summary>
        /// <remarks>
        /// Mutually exclusive with #aiTextureFlags_UseAlpha. 
        /// </remarks>
        IgnoreAlpha = 0x4,
    };
}
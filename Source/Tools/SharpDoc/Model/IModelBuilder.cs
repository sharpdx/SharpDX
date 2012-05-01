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

namespace SharpDoc.Model
{
    /// <summary>
    /// Interface for building a model documentation from an <see cref="NAssemblySource"/>.
    /// </summary>
    public interface IModelBuilder
    {
        /// <summary>
        /// Gets or sets the calculate page id.
        /// </summary>
        /// <value>
        /// The calculate page id.
        /// </value>
        Func<IModelReference, string> PageIdFunction { get; set; } 

        /// <summary>
        /// Loads from an assembly source definition all types to document. 
        /// </summary>
        /// <param name="assemblySource">The assembly source definition.</param>
        /// <param name="memberRegistry">The member registry to populate with types.</param>
        /// <returns>An assembly documentator that contains all documented types, methods.</returns>
        NAssembly LoadFrom(NAssemblySource assemblySource, MemberRegistry memberRegistry);
    }
}
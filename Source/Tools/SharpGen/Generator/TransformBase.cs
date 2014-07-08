// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using SharpGen.CppModel;
using SharpGen.Model;

namespace SharpGen.Generator
{
    /// <summary>
    /// Base class to convert a C++ type to a C# type.
    /// </summary>
    public abstract class TransformBase
    {
        /// <summary>
        /// Initializes this instance with the specified <see cref="TransformManager"/>.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public virtual void Init(TransformManager manager)
        {
            Manager = manager;
            NamingRules = manager.NamingRules;
        }

        /// <summary>
        /// Gets the transform manager.
        /// </summary>
        /// <value>The transform manager.</value>
        public TransformManager Manager { get; private set; }

        /// <summary>
        /// Gets the naming rules manager.
        /// </summary>
        /// <value>The naming rules manager.</value>
        public NamingRulesManager NamingRules { get; private set; }

        /// <summary>
        /// Prepares the specified C++ element to a C# element.
        /// </summary>
        /// <param name="cppElement">The C++ element.</param>
        /// <returns>The C# element created and registered to the <see cref="TransformManager"/></returns>
        public abstract CsBase Prepare(CppElement cppElement);

        /// <summary>
        /// Processes the specified C# element to complete the mapping process between the C++ and C# element.
        /// </summary>
        /// <param name="csElement">The C# element.</param>
        public abstract void Process(CsBase csElement);
    }
}
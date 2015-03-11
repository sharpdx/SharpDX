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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpGen.TextTemplating
{
    /// <summary>
    /// Base class for a template. A template needs to implement the <see cref="Process"/> method.
    /// </summary>
    public abstract class Templatizer
    {
        private readonly Stack<string> _indents;

        /// <summary>
        /// Initializes a new instance of the <see cref="Templatizer"/> class.
        /// </summary>
        protected Templatizer()
        {
            IsNewLine = true;
            _indents = new Stack<string>();
            Writer = new StringWriter();
        }

        /// <summary>
        /// Gets or sets the writer to write the processed template to.
        /// </summary>
        /// <value>The writer.</value>
        private TextWriter Writer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a new line
        /// that requires a following indent.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if there is a new line; otherwise, <c>false</c>.
        /// </value>
        private bool IsNewLine { get; set; }

        /// <summary>
        /// Gets or sets the current indent.
        /// </summary>
        /// <value>The current indent.</value>
        private string CurrentIndent { get; set;}

        /// <summary>
        /// Pushes an indentation.
        /// </summary>
        /// <param name="indent">The indent.</param>
        public void PushIndent(string indent)
        {
            _indents.Push(indent);
            UpdateIndent();
        }

        /// <summary>
        /// Pops last indentation.
        /// </summary>
        public void PopIndent()
        {
            if (_indents.Count > 0)
                _indents.Pop();
            UpdateIndent();
        }

        /// <summary>
        /// Updates the current indentation based on the stack of indents.
        /// </summary>
        private void UpdateIndent()
        {
            var indentBuilder = new StringBuilder();
            foreach (var item in _indents)
                indentBuilder.Append(item);
            CurrentIndent = indentBuilder.ToString();            
        }

        /// <summary>
        /// Writes a formatted text to the Writer.
        /// </summary>
        /// <param name="value">The text.</param>
        /// <param name="args">The args.</param>
        /// <returns>this instance</returns>
        public Templatizer Write(object value, params object[] args)
        {
            Write(value==null?null:value.ToString(), args);
            return this;
        }

        /// <summary>
        /// Writes an object value to the Writer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>this instance</returns>
        public Templatizer Write(object value)
        {
            Write(value == null ? null : value.ToString());
            return this;
        }

        /// <summary>
        /// Writes a simple text to the Writer
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>this instance</returns>
        public Templatizer Write(string text)
        {
            if (IsNewLine)
            {
                Writer.Write(CurrentIndent);
                IsNewLine = false;
            }
            if (text != null)
                Writer.Write(text);
            return this;
        }

        /// <summary>
        /// Writes a formatted text to the Writer.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="args">The args.</param>
        /// <returns>this instance</returns>
        public Templatizer Write(string text, params object[] args)
        {
            if (text != null)
            {
                text = text.Replace("{", "{{").Replace("}", "}}");
                text = string.Format(text, args);
            }
            Write(text);
            return this;
        }

        /// <summary>
        /// Writes a simple text with a new line to the Writer.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>this instance</returns>
        public Templatizer WriteLine(string text)
        {
            return Write(text).WriteLine();
        }

        /// <summary>
        /// Writes a formatted text with a new line to the Writer.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="args">The args.</param>
        /// <returns>this instance</returns>
        public Templatizer WriteLine(string text, params object[] args)
        {
            return Write(text, args).WriteLine();
        }

        /// <summary>
        /// Writes a new line.
        /// </summary>
        /// <returns></returns>
        public Templatizer WriteLine()
        {
            Writer.WriteLine();
            IsNewLine = true;
            return this;
        }

        /// <summary>
        /// Main method to be implemented to output a processed template.
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Returns the text processed by this templatizer.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Writer.ToString();
        }
    }
}

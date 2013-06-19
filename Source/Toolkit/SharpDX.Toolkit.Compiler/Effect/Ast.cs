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
using System.Collections.Generic;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A simple ast used to store technique/pass parsing result.
    /// </summary>
    internal class Ast
    {
        /// <summary>
        /// Root node for all ast objects.
        /// </summary>
        public class Node
        {
            public SourceSpan Span;
        }

        /// <summary>
        /// Root node for all expressions.
        /// </summary>
        public class Expression : Node
        {
        }

        /// <summary>
        /// Root node for all statements.
        /// </summary>
        public class Statement : Node
        {
        }

        /// <summary>
        /// An identifier.
        /// </summary>
        public class Identifier : Node
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Identifier" /> class.
            /// </summary>
            public Identifier()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Identifier" /> class.
            /// </summary>
            /// <param name="text">The name.</param>
            public Identifier(string text)
            {
                Text = text;
            }

            /// <summary>
            /// The identifier as a string.
            /// </summary>
            public string Text;

            /// <summary>
            /// Is an indirect reference using &lt;...&gt;.
            /// </summary>
            public bool IsIndirect;

            public override string ToString()
            {
                return string.Format("{0}", IsIndirect ? "<" + Text + ">" :  Text);
            }
        }

        /// <summary>
        /// An indexed identifier.
        /// </summary>
        public class IndexedIdentifier : Identifier
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="IndexedIdentifier" /> class.
            /// </summary>
            public IndexedIdentifier()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="IndexedIdentifier" /> class.
            /// </summary>
            /// <param name="text">The name.</param>
            /// <param name="index">The index.</param>
            public IndexedIdentifier(string text, int index)
            {
                Text = text;
                Index = index;
            }

            /// <summary>
            /// The index
            /// </summary>
            public int Index;

            public override string ToString()
            {
                var str = string.Format("{0}[{1}]", Text, Index);
                return string.Format("{0}", IsIndirect ? "<" + str + ">" : str);
            }
        }

        /// <summary>
        /// A literal value.
        /// </summary>
        public class Literal : Node
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Literal" /> class.
            /// </summary>
            public Literal()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Literal" /> class.
            /// </summary>
            /// <param name="value">The value.</param>
            public Literal(object value)
            {
                Value = value;
            }

            /// <summary>
            /// The literal value.
            /// </summary>
            public object Value;

            public override string ToString()
            {
                if (Value is string)
                {
                    return string.Format("\"{0}\"", Value);
                }

                if (Value is bool)
                {
                    return (bool)Value ? "true" : "false";
                }

                if (Value == null)
                {
                    return "null";
                }

                return string.Format("{0}", Value);
            }
        }

        /// <summary>
        /// An expression statement.
        /// </summary>
        public class ExpressionStatement : Statement
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ExpressionStatement" /> class.
            /// </summary>
            public ExpressionStatement()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ExpressionStatement" /> class.
            /// </summary>
            /// <param name="expression">The expression.</param>
            public ExpressionStatement(Expression expression)
            {
                Expression = expression;
            }

            /// <summary>
            /// The Expression.
            /// </summary>
            public Expression Expression;

            public override string ToString()
            {
                return string.Format("{0};", Expression);
            }
        }

        /// <summary>
        /// An array initializer {...} expression.
        /// </summary>
        public class ArrayInitializerExpression : Expression
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ArrayInitializerExpression" /> class.
            /// </summary>
            public ArrayInitializerExpression()
            {
                Values = new List<Expression>();
            }

            /// <summary>
            /// List of values.
            /// </summary>
            public List<Expression> Values;

            public override string ToString()
            {
                return string.Format("{{{0}}}", Utilities.Join(",", Values));
            }
        }

        /// <summary>
        /// A reference to an identifier.
        /// </summary>
        public class IdentifierExpression : Expression
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="IdentifierExpression" /> class.
            /// </summary>
            public IdentifierExpression()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="IdentifierExpression" /> class.
            /// </summary>
            /// <param name="name">The name.</param>
            public IdentifierExpression(Identifier name)
            {
                Name = name;
            }

            /// <summary>
            /// The identifier referenced by this expression.
            /// </summary>
            public Identifier Name;

            public override string ToString()
            {
                return string.Format("{0}", Name);
            }
        }

        /// <summary>
        /// An assign expression name = value.
        /// </summary>
        public class AssignExpression : Expression
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AssignExpression" /> class.
            /// </summary>
            public AssignExpression()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AssignExpression" /> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            public AssignExpression(Identifier name, Expression value)
            {
                Name = name;
                Value = value;
            }

            /// <summary>
            /// The identifier receiver. 
            /// </summary>
            public Identifier Name;

            /// <summary>
            /// The value to assign.
            /// </summary>
            public Expression Value;

            public override string ToString()
            {
                return string.Format("{0} = {1}", Name, Value);
            }
        }

        /// <summary>
        /// A literal expression.
        /// </summary>
        public class LiteralExpression : Expression
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="LiteralExpression" /> class.
            /// </summary>
            public LiteralExpression()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="LiteralExpression" /> class.
            /// </summary>
            /// <param name="value">The value.</param>
            public LiteralExpression(Literal value)
            {
                Value = value;
            }

            public Literal Value;

            public override string ToString()
            {
                return string.Format("{0}", Value);
            }
        }

        /// <summary>
        /// A compile expression (old style d3d9: compile vx_2_0 VS();).
        /// </summary>
        public class CompileExpression : Expression
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CompileExpression" /> class.
            /// </summary>
            public CompileExpression()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CompileExpression" /> class.
            /// </summary>
            /// <param name="profile"></param>
            /// <param name="method"></param>
            public CompileExpression(Identifier profile, Expression method)
            {
                Profile = profile;
                Method = method;
            }

            public Identifier Profile;

            public Expression Method;

            public override string ToString()
            {
                return string.Format("compile {0} {1}", Profile, Method);
            }
        }

        /// <summary>
        /// A method expression.
        /// </summary>
        public class MethodExpression : Expression
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MethodExpression" /> class.
            /// </summary>
            public MethodExpression()
            {
                Arguments = new List<Expression>();
            }

            /// <summary>
            /// Name of the method.
            /// </summary>
            public Identifier Name;

            /// <summary>
            /// Arguments.
            /// </summary>
            public List<Expression> Arguments;

            public override string ToString()
            {
                return string.Format("{0}({1})", Name, Utilities.Join(",", Arguments));
            }
        }

        /// <summary>
        /// A HLSL 'pass'.
        /// </summary>
        public class Pass : Node
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Pass" /> class.
            /// </summary>
            public Pass()
            {
                Statements = new List<Statement>();
            }

            /// <summary>
            /// Name of the pass.
            /// </summary>
            /// <remarks>
            /// Can be null.
            /// </remarks>
            public string Name;

            /// <summary>
            /// List of statements.
            /// </summary>
            public List<Statement> Statements;

            public override string ToString()
            {
                return string.Format("pass {0}{{...{1} statement...}}", Name == null ? string.Empty : Name + " ", Statements.Count);
            }
        }

        /// <summary>
        /// A HLSL 'technique'.
        /// </summary>
        public class Technique : Node
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Technique" /> class.
            /// </summary>
            public Technique()
            {
                Passes = new List<Pass>();
            }

            /// <summary>
            /// Name of the technique.
            /// </summary>
            /// <remarks>
            /// Can be null.
            /// </remarks>
            public string Name;

            /// <summary>
            /// List of passes.
            /// </summary>
            public List<Pass> Passes;

            public override string ToString()
            {
                return string.Format("technique {0}{{...{1} pass...}}", Name == null ? string.Empty : Name + " ", Passes.Count);
            }
        }

        /// <summary>
        /// Root ast for a shader.
        /// </summary>
        public class Shader : Node
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Shader" /> class.
            /// </summary>
            public Shader()
            {
                Techniques = new List<Technique>();
            }

            /// <summary>
            /// List of techniques.
            /// </summary>
            public List<Technique> Techniques;
        }
    }
}
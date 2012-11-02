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
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;

namespace SharpDX.Toolkit.Graphics
{
    internal class EffectParser
    {
        /// <summary>
        ///   End of file token.
        /// </summary>
        private static readonly Token EndOfFile = new Token(TokenType.EndOfFile, null, new SourceSpan());

        private int bracketCount = 0;
        private int curlyBraceCount = 0;
        private string currentFile;
        private int currentLine;
        private int currentLineAbsolutePos;
        private Ast.Pass currentPass;
        private List<Token> currentPreviewTokens = new List<Token>();
        private Ast.Technique currentTechnique;
        private Token currentToken;

        internal IncludeHandler includeHandler;
        private bool isPreviewToken = false;
        private int parentCount = 0;
        private List<Token> previewTokens = new List<Token>();
        private EffectParserResult result;
        private Token savedPreviewToken;
        private IEnumerator<Token> tokenEnumerator;


        /// <summary>
        /// Initializes a new instance of the <see cref="EffectParser" /> class.
        /// </summary>
        public EffectParser()
        {
            includeHandler = new IncludeHandler();
            Macros = new List<ShaderMacro>();
        }

        /// <summary>
        /// Gets or sets the include file callback.
        /// </summary>
        /// <value>The include file callback.</value>
        public IncludeFileDelegate IncludeFileCallback
        {
            get { return includeHandler.IncludeFileCallback; }
            set { includeHandler.IncludeFileCallback = value; }
        }

        /// <summary>
        /// Gets the macros.
        /// </summary>
        /// <value>The macros.</value>
        public List<ShaderMacro> Macros { get; private set; }

        /// <summary>
        /// Gets the include directory list.
        /// </summary>
        /// <value>The include directory list.</value>
        public List<string> IncludeDirectoryList
        {
            get { return includeHandler.IncludeDirectories; }
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public EffectCompilerLogger Logger { get; set; }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Result of parsing</returns>
        public EffectParserResult Parse(string input, string fileName)
        {
            // Reset count
            parentCount = 0;
            curlyBraceCount = 0;
            bracketCount = 0;
            result = new EffectParserResult {SourceFileName = Path.Combine(Environment.CurrentDirectory, Path.GetFullPath(fileName))};

            includeHandler.Logger = Logger;
            includeHandler.FileResolved.Clear();
            includeHandler.CurrentDirectory.Clear();
            includeHandler.CurrentDirectory.Push(Path.GetDirectoryName(result.SourceFileName));

            string compilationErrors = null;
            var preprocessedInput = ShaderBytecode.Preprocess(input, Macros.ToArray(), includeHandler, out compilationErrors, fileName);
            result.PreprocessedSource = preprocessedInput;

            tokenEnumerator = Tokenizer.Run(preprocessedInput).GetEnumerator();

            do
            {
                var token = NextToken();
                if (token.Type == TokenType.EndOfFile)
                    break;

                switch (token.Type)
                {
                    case TokenType.Identifier:
                        if ((token.EqualString("technique") || token.EqualString("technique10") || token.EqualString("technique11")) && curlyBraceCount == 0)
                            ParseTechnique();
                        break;
                }
            } while (true);

            if (!CheckAllBracketsClosed())
            {
                if (parentCount != 0)
                {
                    Logger.Error("Error matching closing parenthese for '('", lastParentToken.Span);
                }

                if (bracketCount != 0)
                {
                    Logger.Error("Error matching closing bracket for '['", lastBracketToken.Span);
                }

                if (curlyBraceCount != 0)
                {
                    Logger.Error("Error matching closing curly brace for '{'", lastCurlyBraceToken.Span);
                }
            }


            return result;
        }

        private bool CheckAllBracketsClosed()
        {
            return parentCount == 0 && bracketCount == 0 && curlyBraceCount == 0;
        }

        private Token lastParentToken;

        private Token lastBracketToken;

        private Token lastCurlyBraceToken;

        private bool HandleBrackets(Token token)
        {
            bool isBracketOk = true;
            switch (token.Type)
            {
                case TokenType.LeftParent:
                    lastParentToken = token;
                    parentCount++;
                    break;

                case TokenType.RightParent:
                    lastParentToken = token;
                    parentCount--;
                    if (parentCount < 0)
                    {
                        parentCount = 0;
                        Logger.Error("No matching parenthesis '(' for this closing token.", token.Span);
                        isBracketOk = false;
                    }
                    break;

                case TokenType.LeftBracket:
                    lastBracketToken = token;
                    bracketCount++;
                    break;

                case TokenType.RightBracket:
                    lastBracketToken = token;
                    bracketCount--;
                    if (bracketCount < 0)
                    {
                        bracketCount = 0;
                        Logger.Error("No matching bracket '[' for this closing token.", token.Span);
                        isBracketOk = false;
                    }
                    break;

                case TokenType.LeftCurlyBrace:
                    lastCurlyBraceToken = token;
                    curlyBraceCount++;
                    break;

                case TokenType.RightCurlyBrace:
                    lastCurlyBraceToken = token;
                    curlyBraceCount--;
                    if (curlyBraceCount < 0)
                    {
                        curlyBraceCount = 0;
                        Logger.Error("No matching curly brace '{{' for this closing token.", token.Span);
                        isBracketOk = false;
                    }
                    break;
            }

            return isBracketOk;
        }


        private void BeginPreviewToken()
        {
            isPreviewToken = true;
            savedPreviewToken = currentToken;
        }

        private void EndPreviewToken()
        {
            isPreviewToken = false;
            previewTokens = currentPreviewTokens;
            currentPreviewTokens = new List<Token>();
            currentToken = savedPreviewToken;
        }

        private Token NextToken()
        {
            var token = InternalNextToken();

            // Handle preprocessor "line"
            while (token.Type == TokenType.Preprocessor)
            {
                InternalNextToken();
                if (!Expect("line"))
                {
                    Logger.Error("Unsupported preprocessor token [{0}]. Only #line is supported.", token.Span, currentToken.Value);
                    return currentToken;
                }

                if (!ExpectNext(TokenType.Number))
                    return currentToken;

                // Set currentLine - 1 as the NL at the end of the preprocessor line will add +1 
                currentLine = int.Parse(currentToken.Value) - 1;

                // Check next token
                token = InternalNextToken();

                // If this is a string, then this is the end of preprocessor line
                if (token.Type == TokenType.String)
                {
                    currentFile = token.Value.Substring(1, token.Value.Length - 2);

                    // Replace "file" from #line preprocessor with the actual fullpath.
                    for (int i = 0; i < includeHandler.FileResolved.Count; i++)
                    {
                        var fileResolved = includeHandler.FileResolved[i];
                        if (fileResolved.Item1 == currentFile)
                        {
                            currentFile = fileResolved.Item2;
                            includeHandler.FileResolved.RemoveAt(i);
                            break;
                        }
                    }
                    currentFile = currentFile.Replace(@"\\", @"\");

                    token = InternalNextToken();
                }
            }

            // Set correct location for current token
            currentToken.Span.Line = currentLine;
            currentToken.Span.Column = token.Span.Index - currentLineAbsolutePos + 1;
            currentToken.Span.FilePath = currentFile;

            return currentToken;
        }

        private Token InternalNextToken()
        {
            // TODO: token preview is not safe with NewLine count
            if (previewTokens.Count > 0)
            {
                currentToken = previewTokens[0];
                previewTokens.RemoveAt(0);

                if (isPreviewToken)
                    currentPreviewTokens.Add(currentToken);

                return currentToken;
            }

            while (tokenEnumerator.MoveNext())
            {
                currentToken = tokenEnumerator.Current;
                if (currentToken.Type == TokenType.Newline)
                {
                    currentLine++;
                    currentLineAbsolutePos = currentToken.Span.Index + currentToken.Span.Length;
                }
                else
                {
                    currentToken.Span.Line = currentLine;
                    currentToken.Span.Column = currentToken.Span.Index - currentLineAbsolutePos + 1;
                    currentToken.Span.FilePath = currentFile;

                    HandleBrackets(currentToken);
                    if (isPreviewToken)
                        currentPreviewTokens.Add(currentToken);
                    return currentToken;
                }
            }

            currentToken = EndOfFile;
            return currentToken;
        }

        private bool ExpectNext(TokenType tokenType)
        {
            NextToken();
            return Expect(tokenType);
        }

        private bool Expect(TokenType tokenType)
        {
            bool isSameTokenType = currentToken.Type == tokenType;
            if (!isSameTokenType)
                Logger.Error("Error while parsing unexpected token [{0}]. Expecting token [{1}]", currentToken.Span, currentToken.Value, tokenType);
            return isSameTokenType;
        }

        private bool Expect(string keyword, bool isCaseSensitive = false)
        {
            if (Expect(TokenType.Identifier))
            {
                if (!(currentToken.EqualString(keyword, isCaseSensitive)))
                {
                    Logger.Error("Error while parsing unexpected keyword [{0}]. Expecting keyword [{1}]", currentToken.Span, currentToken.Value, keyword);
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private void ParseTechnique()
        {
            var technique = new Ast.Technique() {Span = currentToken.Span};

            // Get Technique name if any.
            var ident = NextToken();
            if (ident.Type == TokenType.Identifier)
            {
                technique.Name = ident.Value;
                NextToken();
            }

            if (!Expect(TokenType.LeftCurlyBrace))
                return;

            // Add the technique being parsed
            currentTechnique = technique;

            bool continueParsingTecnhique = true;
            bool isParseOk = false;

            do
            {
                var token = NextToken();
                switch (token.Type)
                {
                    case TokenType.Identifier:
                        if (token.EqualString("pass"))
                            ParsePass();
                        break;

                    case TokenType.RightCurlyBrace:
                        isParseOk = true;
                        continueParsingTecnhique = false;
                        break;

                    default:
                        Logger.Error("Unexpected token [{0}]. Expecting tokens ['pass','}}']", currentToken.Span, currentToken);
                        continueParsingTecnhique = false;
                        break;
                }
            } while (continueParsingTecnhique);

            if (isParseOk)
            {
                if (result.Shader == null)
                    result.Shader = new Ast.Shader();
                result.Shader.Techniques.Add(technique);
            }
        }

        private void ParsePass()
        {
            var pass = new Ast.Pass() {Span = currentToken.Span};

            // Get Pass name if any.
            var ident = NextToken();
            if (ident.Type == TokenType.Identifier)
            {
                pass.Name = ident.Value;
                NextToken();
            }

            if (!Expect(TokenType.LeftCurlyBrace))
                return;

            // Add the technique being parsed
            currentPass = pass;

            bool continueParsingTecnhique = true;
            bool isParseOk = false;
            do
            {
                var token = NextToken();
                switch (token.Type)
                {
                    case TokenType.Identifier:
                        if (!ParseStatement())
                            continueParsingTecnhique = false;
                        break;

                    case TokenType.RightCurlyBrace:
                        isParseOk = true;
                        continueParsingTecnhique = false;
                        break;

                    // fxc doesn't support empty statements, so we don't support them.
                    //case TokenType.SemiColon:
                    //    // Skip empty statements
                    //    break;

                    default:
                        Logger.Error("Unexpected token [{0}]. Expecting tokens ['identifier','}}']", currentToken.Span, currentToken);
                        continueParsingTecnhique = false;
                        break;
                }
            } while (continueParsingTecnhique);

            if (isParseOk)
                currentTechnique.Passes.Add(pass);
        }

        private bool ParseStatement()
        {
            var expression = ParseExpression();

            if (expression == null)
                return false;

            if (!ExpectNext(TokenType.SemiColon))
                return false;

            currentPass.Statements.Add(new Ast.ExpressionStatement(expression) {Span = expression.Span});
            return true;
        }

        private Ast.Expression ParseExpression(bool allowIndirectIdentifier = false)
        {
            Ast.Expression expression = null;
            var token = currentToken;

            switch (token.Type)
            {
                case TokenType.Identifier:
                    if (token.Value == "true")
                        expression = new Ast.LiteralExpression(new Ast.Literal(true) {Span = token.Span}) {Span = token.Span};
                    else if (token.Value == "false")
                        expression = new Ast.LiteralExpression(new Ast.Literal(false) {Span = token.Span}) {Span = token.Span};
                    else if (string.Compare(token.Value, "null", StringComparison.OrdinalIgnoreCase) == 0)
                        expression = new Ast.LiteralExpression(new Ast.Literal(null) {Span = token.Span}) {Span = token.Span};
                    else if (token.Value == "compile")
                    {
                        expression = ParseCompileExpression();
                    }
                    else
                    {
                        var nextIdentifier = ParseIdentifier(token, true);

                        // check next token
                        BeginPreviewToken();
                        var nextToken = NextToken();
                        EndPreviewToken();

                        switch (nextToken.Type)
                        {
                            case TokenType.LeftParent:
                                var methodExpression = ParseMethodExpression();
                                methodExpression.Name = nextIdentifier;
                                expression = methodExpression;
                                break;
                            case TokenType.Equal:
                                var assignExpression = ParseAssignExpression();
                                assignExpression.Name = nextIdentifier;
                                expression = assignExpression;
                                break;
                            default:
                                expression = new Ast.IdentifierExpression(nextIdentifier) {Span = token.Span};
                                break;
                        }
                    }
                    break;
                case TokenType.LeftParent:
                    expression = ParseArrayInitializerExpression(TokenType.RightParent);
                    //expression = ParseExpression();
                    //ExpectNext(TokenType.RightParent);
                    break;
                case TokenType.String:
                    // TODO doesn't support escaped strings
                    var str = token.Value.Substring(1, token.Value.Length - 2);
                    // Temporary escape
                    str = str.Replace("\\r\\n", "\r\n");
                    str = str.Replace("\\n", "\n");
                    str = str.Replace("\\t", "\t");
                    expression = new Ast.LiteralExpression(new Ast.Literal(str) {Span = token.Span}) {Span = token.Span};
                    break;

                case TokenType.Number:
                    var numberStr = token.Value.Replace(" ", "");
                    object numberValue;
                    if (numberStr.EndsWith("f", false, CultureInfo.InvariantCulture) || numberStr.Contains("e") || numberStr.Contains("E") || numberStr.Contains("."))
                    {
                        var floatStr = numberStr.TrimEnd('f');
                        numberValue = float.Parse(floatStr, NumberStyles.Float, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        numberValue = int.Parse(numberStr, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    }

                    expression = new Ast.LiteralExpression(new Ast.Literal(numberValue) {Span = token.Span}) {Span = token.Span};
                    break;

                case TokenType.Hexa:
                    var intValue = int.Parse(token.Value.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    expression = new Ast.LiteralExpression(new Ast.Literal(intValue) {Span = token.Span}) {Span = token.Span};
                    break;

                case TokenType.LeftCurlyBrace:
                    expression = ParseArrayInitializerExpression(TokenType.RightCurlyBrace);
                    break;

                default:
                    if (token.Type == TokenType.LessThan && allowIndirectIdentifier)
                    {
                        NextToken();
                        var identifierExpression = ParseExpression() as Ast.IdentifierExpression;
                        bool isClosedIndirect = ExpectNext(TokenType.GreaterThan);
                        if (identifierExpression == null)
                        {
                            Logger.Error("Unexpected expression inside indirect reference.", token.Span);
                        }
                        else
                        {
                            identifierExpression.Name.IsIndirect = isClosedIndirect;
                            expression = identifierExpression;
                        }
                    }
                    else
                    {
                        Logger.Error("Unexpected token [{0}]. Expecting tokens ['identifier', 'string', 'number', '{{']", currentToken.Span, currentToken);
                    }
                    break;
            }

            return expression;
        }

        private Ast.Identifier ParseIdentifier(Token token, bool allowIndexed = false)
        {
            var identifierName = new System.Text.StringBuilder(token.Value);

            // ------------------------------------------------------------
            // Parse complex identifier XXXX::AAAA::BBBBB
            // ------------------------------------------------------------
            BeginPreviewToken();
            var nextToken = NextToken();
            EndPreviewToken();

            // Dot are not supported by compiler XXXX.AAAA.BBBB
            // if (nextToken.Type == TokenType.DoubleColon || nextToken.Type == TokenType.Dot)
            if (nextToken.Type == TokenType.DoubleColon)
            {
                var identifierSeparatorType = nextToken.Type;
                var endChar = nextToken.Type == TokenType.DoubleColon ? "::" : ".";

                nextToken = NextToken();
                identifierName.Append(nextToken.Value);

                bool continueParsing = true;
                do
                {
                    BeginPreviewToken();
                    nextToken = NextToken();
                    EndPreviewToken();

                    switch (nextToken.Type)
                    {
                        case TokenType.Identifier:
                            identifierName.Append(nextToken.Value);
                            break;
                        case TokenType.DoubleColon:
                        case TokenType.Dot:
                            if (identifierName[identifierName.Length - 1] == endChar[0])
                            {
                                Logger.Error("Unexpected token [{0}]. Expecting tokens [identifier]", nextToken.Span, endChar);
                            }
                            if (identifierSeparatorType != nextToken.Type)
                            {
                                Logger.Error("Unexpected token [{0}]. Expecting tokens [{1}]", nextToken.Span, nextToken.Value, endChar);
                            }

                            identifierName.Append(nextToken.Value);
                            break;
                        default:
                            continueParsing = false;
                            break;
                    }

                    // If can go to the next token
                    if (continueParsing)
                    {
                        nextToken = NextToken();
                    }
                } while (continueParsing);


                if (identifierName[identifierName.Length - 1] == endChar[0])
                {
                    Logger.Error("Unexpected token [{0}]. Expecting tokens [identifier]", nextToken.Span, endChar);                    
                }
            }

            // ------------------------------------------------------------
            // Parse optional indexer a[xxx]
            // ------------------------------------------------------------
            if (allowIndexed)
            {
                BeginPreviewToken();
                var arrayToken = NextToken();
                EndPreviewToken();

                if (arrayToken.Type == TokenType.LeftBracket)
                {
                    // Skip left bracket [
                    NextToken();

                    // Get first token from inside bracket
                    NextToken();

                    // Parse value inside bracket
                    var expression = ParseExpression() as Ast.LiteralExpression;

                    if (expression == null || !(expression.Value.Value is int))
                    {
                        Logger.Error("Unsupported expression for indexed variable. Only support for integer Literal", currentToken.Span);
                        return null;
                    }
                    ExpectNext(TokenType.RightBracket);

                    return new Ast.IndexedIdentifier(identifierName.ToString(), (int)expression.Value.Value) { Span = token.Span };
                }
            }

            return new Ast.Identifier(identifierName.ToString()) { Span = token.Span };
        }


        private Ast.Expression ParseCompileExpression()
        {
            var compileExp = new Ast.CompileExpression() {Span = currentToken.Span};

            if (!ExpectNext(TokenType.Identifier))
                return compileExp;

            compileExp.Profile = ParseIdentifier(currentToken);

            // Go to expression token
            NextToken();

            compileExp.Method = ParseExpression();

            return compileExp;
        }

        private Ast.MethodExpression ParseMethodExpression()
        {
            if (!ExpectNext(TokenType.LeftParent))
                return null;

            var methodExp = new Ast.MethodExpression {Span = currentToken.Span};

            int expectedArguments = 0;
            bool continueParsing = true;
            do
            {
                var token = NextToken();
                switch (token.Type)
                {
                    case TokenType.RightParent:
                        continueParsing = false;
                        break;

                    default:
                        var nextValue = ParseExpression();
                        if (nextValue == null)
                        {
                            continueParsing = false;
                        }
                        else
                        {
                            methodExp.Arguments.Add(nextValue);

                            NextToken();

                            if (currentToken.Type == TokenType.RightParent)
                            {
                                continueParsing = false;
                            }
                            else if (currentToken.Type == TokenType.Comma)
                            {
                                expectedArguments += (expectedArguments == 0) ? 2 : 1;
                            }
                            else
                            {
                                Logger.Error("Unexpected token [{0}]. Expecting tokens [',', ')']", currentToken.Span, currentToken);
                                continueParsing = false;
                            }
                        }
                        break;
                }
            } while (continueParsing);

            int argCount = methodExp.Arguments.Count;
            if (expectedArguments > 0 && expectedArguments != argCount)
                Logger.Error("Unexpected number of commas.", currentToken.Span);

            return methodExp;
        }

        private Ast.AssignExpression ParseAssignExpression()
        {
            var span = currentToken.Span;
            if (!ExpectNext(TokenType.Equal))
                return null;

            NextToken();

            var value = ParseExpression(true);
            return new Ast.AssignExpression {Value = value, Span = span};
        }

        private Ast.ArrayInitializerExpression ParseArrayInitializerExpression(TokenType rightParent)
        {
            var expression = new Ast.ArrayInitializerExpression() {Span = currentToken.Span};
            var values = expression.Values;

            bool continueParsing = true;
            do
            {
                var token = NextToken();
                if (token.Type == rightParent)
                    break;

                var nextValue = ParseExpression();
                if (nextValue == null)
                {
                    continueParsing = false;
                }
                else
                {
                    values.Add(nextValue);

                    NextToken();

                    if (currentToken.Type == rightParent)
                    {
                        continueParsing = false;
                    }
                    else if (currentToken.Type != TokenType.Comma)
                    {
                        Logger.Error("Unexpected token [{0}]. Expecting tokens [',', '{1}']", currentToken.Span, currentToken.Value, rightParent);
                        continueParsing = false;
                    }
                }
            } while (continueParsing);

            return expression;
        }

        #region Nested type: IncludeHandler

        internal class IncludeHandler : CallbackBase, Include
        {
            public readonly Stack<string> CurrentDirectory;

            public readonly List<Tuple<string, string>> FileResolved;
            public readonly List<string> IncludeDirectories;
            public SourceSpan CurrentSpan;
            public IncludeFileDelegate IncludeFileCallback;
            public EffectCompilerLogger Logger;

            public IncludeHandler()
            {
                IncludeDirectories = new List<string>();
                CurrentDirectory = new Stack<string>();
                FileResolved = new List<Tuple<string, string>>();
            }

            #region Include Members

            public Stream Open(IncludeType type, string fileName, Stream parentStream)
            {
                var currentDirectory = CurrentDirectory.Peek();
                if (currentDirectory == null)
                    currentDirectory = Environment.CurrentDirectory;

                var filePath = fileName;

                if (!Path.IsPathRooted(filePath))
                {
                    var directoryToSearch = new List<string> {currentDirectory};
                    directoryToSearch.AddRange(IncludeDirectories);
                    foreach (var dirPath in directoryToSearch)
                    {
                        var selectedFile = Path.Combine(dirPath, fileName);
                        if (File.Exists(selectedFile))
                        {
                            filePath = selectedFile;
                            break;
                        }
                    }
                }

                Stream stream = null;

                if (filePath == null || !File.Exists(filePath))
                {
                    // Else try to use the include file callback
                    if (IncludeFileCallback != null)
                    {
                        stream = IncludeFileCallback(type == IncludeType.System, fileName);
                        if (stream != null)
                        {
                            FileResolved.Add(new Tuple<string, string>(fileName, fileName));
                            return stream;
                        }
                    }

                    Logger.Error("Unable to find file [{0}]", CurrentSpan, filePath ?? fileName);
                    return null;
                }

                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                CurrentDirectory.Push(Path.GetDirectoryName(Path.GetFullPath(filePath)));
                FileResolved.Add(new Tuple<string, string>(fileName, Path.GetFullPath(filePath)));
                return stream;
            }

            public void Close(Stream stream)
            {
                stream.Close();
                CurrentDirectory.Pop();
            }

            #endregion
        }

        #endregion

        #region Nested type: Tuple

        internal class Tuple<T1, T2>
        {
            public T1 Item1;
            public T2 Item2;

            public Tuple(T1 item1, T2 item2)
            {
                Item1 = item1;
                Item2 = item2;
            }
        }

        #endregion
    }
}
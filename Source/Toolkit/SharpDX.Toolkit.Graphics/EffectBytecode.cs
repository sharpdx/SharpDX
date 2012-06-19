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
using System.Text.RegularExpressions;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;

namespace SharpDX.Toolkit.Graphics
{
    public class EffectBytecode
    {
        private static readonly string[] StageParameters = new string[] { "vs", "ds", "hs", "gs", "ps", "cs" };
        internal ShaderBytecode[] Bytecodes = new ShaderBytecode[StageParameters.Length];

        public static EffectBytecode Compile(string sourceCode, string effectName, FeatureLevel level, string sourceFileName, ShaderFlags flags = ShaderFlags.None, params ShaderMacro[] macros)
        {
            var regexEffect = new Regex(@"<Effect\s+(name=""" + effectName + @""".*?)/>");

            var match = regexEffect.Match(sourceCode);

            if (!match.Success)
                throw new CompilationException(string.Format("Unable to find XML Effect description for effect name [{0}]", effectName));

            var parametersSplit = match.Groups[1].Value.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var parameters = new Dictionary<string, string>();
            foreach (var parameter in parametersSplit)
            {
                var nameValue = parameter.Split('=');
                parameters.Add(nameValue[0].Trim(), nameValue[1].Trim().Trim('"'));
            }

            string profilePostfix;
            switch (level)
            {
                case FeatureLevel.Level_11_0:
                    profilePostfix = "5_0";
                    break;
                case FeatureLevel.Level_10_0:
                    profilePostfix = "4_0";
                    break;
                case FeatureLevel.Level_10_1:
                    profilePostfix = "4_1";
                    break;
                case FeatureLevel.Level_9_3:
                    profilePostfix = "4_0_level_9_3";
                    break;
                case FeatureLevel.Level_9_2:
                    profilePostfix = "4_0_level_9_3";
                    break;
                case FeatureLevel.Level_9_1:
                    profilePostfix = "4_0_level_9_1";
                    break;
                default:
                    throw new CompilationException("Invalid feature level");
            }

            var effectBytecode = new EffectBytecode();

            for (int i = 0; i < StageParameters.Length; i++)
            {
                var stageParameter = StageParameters[i];
                string stageAttr;
                if (parameters.TryGetValue(stageParameter, out stageAttr))
                {
                    var stageProfile = stageParameter + "_" + profilePostfix;

                    effectBytecode.Bytecodes[i] = ShaderBytecode.Compile(sourceCode, stageAttr, stageProfile, flags, EffectFlags.None, macros, null, sourceFileName);
                }
            }

            return effectBytecode;
        }
    }
}
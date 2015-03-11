// Copyright (c) 2010 SharpDX - Alexandre Mutel
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
using Mono.Cecil;
using Mono.Linker;
using Mono.Linker.Steps;

namespace SharpPak
{
    /// <summary>
    /// Custom SharpDX step class for MonoLinker.
    /// </summary>
    /// <remarks>
    /// This class must be appended after the sweep step:
    /// <code>
    /// pipeline.AddStepBefore(typeof (SweepStep), new ComObjectStep());
    /// </code>
    /// </remarks>
    public class ComObjectStep : BaseStep
    {
        protected override void Process()
        {
            foreach (var assembly in Context.GetAssemblies())
                SweepAssembly(assembly);
        }

        void SweepAssembly(AssemblyDefinition assembly)
        {
            if (Annotations.GetAction(assembly) != AssemblyAction.Link)
                return;

            foreach (var type in assembly.MainModule.Types)
            {
                if (Annotations.IsMarked(type))
                {
                    bool isComObject = false;
                    TypeReference parentType = type.BaseType;
                    while (parentType != null && parentType.FullName != "System.Object" && parentType is TypeDefinition)
                    {
                        if (parentType.FullName == "SharpDX.ComObject")
                        {
                            isComObject = true;
                            break;
                        }
                        parentType = ((TypeDefinition)parentType).BaseType;
                    }

                    if (isComObject)
                    {
                        foreach (var methodDefinition in type.Methods)
                        {
                            if (methodDefinition.IsConstructor && methodDefinition.Parameters.Count == 1 && methodDefinition.Parameters[0].ParameterType.FullName == "System.IntPtr")
                            {
                                Context.Annotations.Mark(methodDefinition);
                                Context.Annotations.SetAction(methodDefinition, MethodAction.Parse);
                            }
                        }
                    }
                    continue;
                }
            }
        }
    }
}
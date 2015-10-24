﻿// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Pdb;
using CallSite = Mono.Cecil.CallSite;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace SharpCli
{
    /// <summary>
    /// InteropBuilder is responsible to patch SharpDX assemblies and inject unmanaged interop call.
    /// InteropBuilder is also adding several useful methods:
    /// - memcpy using cpblk
    /// - Read/ReadRange/Write/WriteRange of structured data to a memory location
    /// - SizeOf on generic structures (C# usually doesn't allow this).
    /// </summary>
    public class InteropApp
    {
        private List<TypeDefinition> classToRemoveList = new List<TypeDefinition>();
        AssemblyDefinition assembly;
        private TypeReference voidType;
        private TypeReference voidPointerType;
        private TypeReference intType;

        /// <summary>
        /// Creates a module init for a C# assembly.
        /// </summary>
        /// <param name="method">The method to add to the module init.</param>
        private void CreateModuleInit(MethodDefinition method)
        {
            const MethodAttributes ModuleInitAttributes = MethodAttributes.Static
                                                          | MethodAttributes.Assembly
                                                          | MethodAttributes.SpecialName
                                                          | MethodAttributes.RTSpecialName;

            var moduleType = assembly.MainModule.GetType("<Module>");

            // Get or create ModuleInit method
            var cctor = moduleType.Methods.FirstOrDefault(moduleTypeMethod => moduleTypeMethod.Name == ".cctor");
            if (cctor == null)
            {
                cctor = new MethodDefinition(".cctor", ModuleInitAttributes, method.ReturnType);
                moduleType.Methods.Add(cctor);
            }

            bool isCallAlreadyDone = cctor.Body.Instructions.Any(instruction => instruction.OpCode == OpCodes.Call && instruction.Operand == method);

            // If the method is not called, we can add it
            if (!isCallAlreadyDone)
            {
                var ilProcessor = cctor.Body.GetILProcessor();
                var retInstruction = cctor.Body.Instructions.FirstOrDefault(instruction => instruction.OpCode == OpCodes.Ret);
                var callMethod = ilProcessor.Create(OpCodes.Call, method);

                if (retInstruction == null)
                {
                    // If a ret instruction is not present, add the method call and ret
                    ilProcessor.Append(callMethod);
                    ilProcessor.Emit(OpCodes.Ret);
                }
                else
                {
                    // If a ret instruction is already present, just add the method to call before
                    ilProcessor.InsertBefore(retInstruction, callMethod);
                }
            }
        }

        /// <summary>
        /// Creates the write method with the following signature: 
        /// <code>
        /// public static unsafe void* Write&lt;T&gt;(void* pDest, ref T data) where T : struct
        /// </code>
        /// </summary>
        /// <param name="method">The method to patch</param>
        private void CreateWriteMethod(MethodDefinition method)
        {
            method.Body.Instructions.Clear();
            method.Body.InitLocals = true;

            var gen = method.Body.GetILProcessor();
            var paramT = method.GenericParameters[0];
            // Preparing locals
            // local(0) int
            method.Body.Variables.Add(new VariableDefinition(intType));
            // local(1) T*
            method.Body.Variables.Add(new VariableDefinition(new PinnedType(new ByReferenceType(paramT))));

            // Push (0) pDest for memcpy
            gen.Emit(OpCodes.Ldarg_0);

            // fixed (void* pinnedData = &data[offset])
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Stloc_1);

            // Push (1) pinnedData for memcpy
            gen.Emit(OpCodes.Ldloc_1);

            // totalSize = sizeof(T)
            gen.Emit(OpCodes.Sizeof, paramT);
            gen.Emit(OpCodes.Conv_I4);
            gen.Emit(OpCodes.Stloc_0);

            // Push (2) totalSize
            gen.Emit(OpCodes.Ldloc_0);

            // Emit cpblk
            EmitCpblk(method, gen);

            // Return pDest + totalSize
            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Conv_I);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Add);

            // Ret
            gen.Emit(OpCodes.Ret);
        }

        private void ReplaceFixedStatement(MethodDefinition method, ILProcessor ilProcessor, Instruction fixedtoPatch)
        {
            var paramT = ((GenericInstanceMethod)fixedtoPatch.Operand).GenericArguments[0];
            // Preparing locals
            // local(0) T*
            method.Body.Variables.Add(new VariableDefinition("pin", new PinnedType(new ByReferenceType(paramT))));

            int index = method.Body.Variables.Count - 1;

            Instruction ldlocFixed;
            Instruction stlocFixed;
            switch (index)
            {
                case 0:
                    stlocFixed = ilProcessor.Create(OpCodes.Stloc_0);
                    ldlocFixed = ilProcessor.Create(OpCodes.Ldloc_0);
                    break;
                case 1:
                    stlocFixed = ilProcessor.Create(OpCodes.Stloc_1);
                    ldlocFixed = ilProcessor.Create(OpCodes.Ldloc_1);
                    break;
                case 2:
                    stlocFixed = ilProcessor.Create(OpCodes.Stloc_2);
                    ldlocFixed = ilProcessor.Create(OpCodes.Ldloc_2);
                    break;
                case 3:
                    stlocFixed = ilProcessor.Create(OpCodes.Stloc_3);
                    ldlocFixed = ilProcessor.Create(OpCodes.Ldloc_3);
                    break;
                default:
                    stlocFixed = ilProcessor.Create(OpCodes.Stloc, index);
                    ldlocFixed = ilProcessor.Create(OpCodes.Ldloc, index);
                    break;
            }
            ilProcessor.InsertBefore(fixedtoPatch, stlocFixed);
            ilProcessor.Replace(fixedtoPatch, ldlocFixed);
        }

        private void ReplaceReadInline(MethodDefinition method, ILProcessor ilProcessor, Instruction fixedtoPatch)
        {
            var paramT = ((GenericInstanceMethod)fixedtoPatch.Operand).GenericArguments[0];
            var copyInstruction = ilProcessor.Create(OpCodes.Ldobj, paramT);
            ilProcessor.Replace(fixedtoPatch, copyInstruction);
        }

        private void ReplaceCopyInline(MethodDefinition method, ILProcessor ilProcessor, Instruction fixedtoPatch)
        {
            var paramT = ((GenericInstanceMethod)fixedtoPatch.Operand).GenericArguments[0];
            var copyInstruction = ilProcessor.Create(OpCodes.Cpobj, paramT);
            ilProcessor.Replace(fixedtoPatch, copyInstruction);
        }

        private void ReplaceSizeOfStructGeneric(MethodDefinition method, ILProcessor ilProcessor, Instruction fixedtoPatch)
        {
            var paramT = ((GenericInstanceMethod)fixedtoPatch.Operand).GenericArguments[0];
            var copyInstruction = ilProcessor.Create(OpCodes.Sizeof, paramT);
            ilProcessor.Replace(fixedtoPatch, copyInstruction);
        }

        /// <summary>
        /// Creates the cast  method with the following signature:
        /// <code>
        /// public static unsafe void* Cast&lt;T&gt;(ref T data) where T : struct
        /// </code>
        /// </summary>
        /// <param name="method">The method cast.</param>
        private void CreateCastMethod(MethodDefinition method)
        {
            method.Body.Instructions.Clear();
            method.Body.InitLocals = true;

            var gen = method.Body.GetILProcessor();

            gen.Emit(OpCodes.Ldarg_0);

            // Ret
            gen.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the cast  method with the following signature:
        /// <code>
        /// public static TCAST[] CastArray&lt;TCAST, T&gt;(T[] arrayData) where T : struct where TCAST : struct
        /// </code>
        /// </summary>
        /// <param name="method">The method cast array.</param>
        private void CreateCastArrayMethod(MethodDefinition method)
        {
            method.Body.Instructions.Clear();
            method.Body.InitLocals = true;

            var gen = method.Body.GetILProcessor();

            gen.Emit(OpCodes.Ldarg_0);

            // Ret
            gen.Emit(OpCodes.Ret);
        }

        private void ReplaceFixedArrayStatement(MethodDefinition method, ILProcessor ilProcessor, Instruction fixedtoPatch)
        {
            var paramT = ((GenericInstanceMethod)fixedtoPatch.Operand).GenericArguments[0];
            // Preparing locals
            // local(0) T*
            method.Body.Variables.Add(new VariableDefinition("pin", new PinnedType(new ByReferenceType(paramT))));

            int index = method.Body.Variables.Count - 1;

            Instruction ldlocFixed;
            Instruction stlocFixed;
            switch (index)
            {
                case 0:
                    stlocFixed = ilProcessor.Create(OpCodes.Stloc_0);
                    ldlocFixed = ilProcessor.Create(OpCodes.Ldloc_0);
                    break;
                case 1:
                    stlocFixed = ilProcessor.Create(OpCodes.Stloc_1);
                    ldlocFixed = ilProcessor.Create(OpCodes.Ldloc_1);
                    break;
                case 2:
                    stlocFixed = ilProcessor.Create(OpCodes.Stloc_2);
                    ldlocFixed = ilProcessor.Create(OpCodes.Ldloc_2);
                    break;
                case 3:
                    stlocFixed = ilProcessor.Create(OpCodes.Stloc_3);
                    ldlocFixed = ilProcessor.Create(OpCodes.Ldloc_3);
                    break;
                default:
                    stlocFixed = ilProcessor.Create(OpCodes.Stloc, index);
                    ldlocFixed = ilProcessor.Create(OpCodes.Ldloc, index);
                    break;
            }

            var instructionLdci40 = ilProcessor.Create(OpCodes.Ldc_I4_0);
            ilProcessor.InsertBefore(fixedtoPatch, instructionLdci40);
            var instructionLdElema = ilProcessor.Create(OpCodes.Ldelema, paramT);
            ilProcessor.InsertBefore(fixedtoPatch, instructionLdElema);
            ilProcessor.InsertBefore(fixedtoPatch, stlocFixed);
            ilProcessor.Replace(fixedtoPatch, ldlocFixed);
        }

        /// <summary>
        /// Creates the write range method with the following signature:
        /// <code>
        /// public static unsafe void* Write&lt;T&gt;(void* pDest, T[] data, int offset, int count) where T : struct
        /// </code>
        /// </summary>
        /// <param name="method">The method copy struct.</param>
        private void CreateWriteRangeMethod(MethodDefinition method)
        {
            method.Body.Instructions.Clear();
            method.Body.InitLocals = true;

            var gen = method.Body.GetILProcessor();
            var paramT = method.GenericParameters[0];
            // Preparing locals
            // local(0) int
            method.Body.Variables.Add(new VariableDefinition(intType));
            // local(1) T*
            method.Body.Variables.Add(new VariableDefinition(new PinnedType(new ByReferenceType(paramT))));

            // Push (0) pDest for memcpy
            gen.Emit(OpCodes.Ldarg_0);

            // fixed (void* pinnedData = &data[offset])
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Ldelema, paramT);
            gen.Emit(OpCodes.Stloc_1);

            // Push (1) pinnedData for memcpy
            gen.Emit(OpCodes.Ldloc_1);

            // totalSize = sizeof(T) * count
            gen.Emit(OpCodes.Sizeof, paramT);
            gen.Emit(OpCodes.Conv_I4);
            gen.Emit(OpCodes.Ldarg_3);
            gen.Emit(OpCodes.Mul);
            gen.Emit(OpCodes.Stloc_0);

            // Push (2) totalSize
            gen.Emit(OpCodes.Ldloc_0);

            // Emit cpblk
            EmitCpblk(method, gen);

            // Return pDest + totalSize
            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Conv_I);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Add);

            // Ret
            gen.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the read method with the following signature:
        /// <code>
        /// public static unsafe void* Read&lt;T&gt;(void* pSrc, ref T data) where T : struct
        /// </code>
        /// </summary>
        /// <param name="method">The method copy struct.</param>
        private void CreateReadMethod(MethodDefinition method)
        {
            method.Body.Instructions.Clear();
            method.Body.InitLocals = true;

            var gen = method.Body.GetILProcessor();
            var paramT = method.GenericParameters[0];

            // Preparing locals
            // local(0) int
            method.Body.Variables.Add(new VariableDefinition(intType));
            // local(1) T*

            method.Body.Variables.Add(new VariableDefinition(new PinnedType(new ByReferenceType(paramT))));

            // fixed (void* pinnedData = &data[offset])
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Stloc_1);

            // Push (0) pinnedData for memcpy
            gen.Emit(OpCodes.Ldloc_1);

            // Push (1) pSrc for memcpy
            gen.Emit(OpCodes.Ldarg_0);

            // totalSize = sizeof(T)
            gen.Emit(OpCodes.Sizeof, paramT);
            gen.Emit(OpCodes.Conv_I4);
            gen.Emit(OpCodes.Stloc_0);

            // Push (2) totalSize
            gen.Emit(OpCodes.Ldloc_0);

            // Emit cpblk
            EmitCpblk(method, gen);

            // Return pDest + totalSize
            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Conv_I);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Add);

            // Ret
            gen.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the read method with the following signature:
        /// <code>
        /// public static unsafe void Read&lt;T&gt;(void* pSrc, ref T data) where T : struct
        /// </code>
        /// </summary>
        /// <param name="method">The method copy struct.</param>
        private void CreateReadRawMethod(MethodDefinition method)
        {
            method.Body.Instructions.Clear();
            method.Body.InitLocals = true;

            var gen = method.Body.GetILProcessor();
            var paramT = method.GenericParameters[0];

            // Push (1) pSrc for memcpy
            gen.Emit(OpCodes.Cpobj);

        }

        /// <summary>
        /// Creates the read range method with the following signature:
        /// <code>
        /// public static unsafe void* Read&lt;T&gt;(void* pSrc, T[] data, int offset, int count) where T : struct
        /// </code>
        /// </summary>
        /// <param name="method">The method copy struct.</param>
        private void CreateReadRangeMethod(MethodDefinition method)
        {
            method.Body.Instructions.Clear();
            method.Body.InitLocals = true;

            var gen = method.Body.GetILProcessor();
            var paramT = method.GenericParameters[0];
            // Preparing locals
            // local(0) int
            method.Body.Variables.Add(new VariableDefinition(intType));
            // local(1) T*
            method.Body.Variables.Add(new VariableDefinition(new PinnedType(new ByReferenceType(paramT))));

            // fixed (void* pinnedData = &data[offset])
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Ldelema, paramT);
            gen.Emit(OpCodes.Stloc_1);

            // Push (0) pinnedData for memcpy
            gen.Emit(OpCodes.Ldloc_1);

            // Push (1) pDest for memcpy
            gen.Emit(OpCodes.Ldarg_0);

            // totalSize = sizeof(T) * count
            gen.Emit(OpCodes.Sizeof, paramT);
            gen.Emit(OpCodes.Conv_I4);
            gen.Emit(OpCodes.Ldarg_3);
            gen.Emit(OpCodes.Conv_I4);
            gen.Emit(OpCodes.Mul);
            gen.Emit(OpCodes.Stloc_0);

            // Push (2) totalSize
            gen.Emit(OpCodes.Ldloc_0);

            // Emit cpblk
            EmitCpblk(method, gen);

            // Return pDest + totalSize
            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Conv_I);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Add);

            // Ret
            gen.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the memcpy method with the following signature:
        /// <code>
        /// public static unsafe void memcpy(void* pDest, void* pSrc, int count)
        /// </code>
        /// </summary>
        /// <param name="methodCopyStruct">The method copy struct.</param>
        private void CreateMemcpy(MethodDefinition methodCopyStruct)
        {
            methodCopyStruct.Body.Instructions.Clear();

            var gen = methodCopyStruct.Body.GetILProcessor();

            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Unaligned, (byte)1);       // unaligned to 1
            gen.Emit(OpCodes.Cpblk);

            // Ret
            gen.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the memset method with the following signature:
        /// <code>
        /// public static unsafe void memset(void* pDest, byte value, int count)
        /// </code>
        /// </summary>
        /// <param name="methodSetStruct">The method set struct.</param>
        private void CreateMemset(MethodDefinition methodSetStruct)
        {
            methodSetStruct.Body.Instructions.Clear();

            var gen = methodSetStruct.Body.GetILProcessor();

            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Unaligned, (byte)1);       // unaligned to 1
            gen.Emit(OpCodes.Initblk);

            // Ret
            gen.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Emits the cpblk method, supporting x86 and x64 platform.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="gen">The gen.</param>
        private void EmitCpblk(MethodDefinition method, ILProcessor gen)
        {
            var cpblk = gen.Create(OpCodes.Cpblk);
            //gen.Emit(OpCodes.Sizeof, voidPointerType);
            //gen.Emit(OpCodes.Ldc_I4_8);
            //gen.Emit(OpCodes.Bne_Un_S, cpblk);
            gen.Emit(OpCodes.Unaligned, (byte)1);       // unaligned to 1
            gen.Append(cpblk);
            
        }

        private List<string>  GetSharpDXAttributes(MethodDefinition method)
        {
            var attributes = new List<string>();
            foreach (var customAttribute in method.CustomAttributes)
            {
                if (customAttribute.AttributeType.FullName == "SharpDX.TagAttribute")
                {
                    var value = customAttribute.ConstructorArguments[0].Value;
                    attributes.Add(value == null ? string.Empty : value.ToString());
                }
            }

            return attributes;
        }

        /// <summary>
        /// Patches the method.
        /// </summary>
        /// <param name="method">The method.</param>
        bool PatchMethod(MethodDefinition method)
        {
            bool isSharpJit = false;

            var attributes = this.GetSharpDXAttributes(method);
            if (attributes.Contains("SharpDX.ModuleInit"))
            {
                CreateModuleInit(method);
            }

            if (method.DeclaringType.Name == "Interop")
            {
                if (method.Name == "memcpy")
                {
                    CreateMemcpy(method);
                }
                else if (method.Name == "memset")
                {
                    CreateMemset(method);
                }
                else if ((method.Name == "Cast") || (method.Name == "CastOut"))
                {
                    CreateCastMethod(method);
                }
                else if (method.Name == "CastArray")
                {
                    CreateCastArrayMethod(method);
                }
                else if (method.Name == "Read" || (method.Name == "ReadOut") || (method.Name == "Read2D"))
                {
                    if (method.Parameters.Count == 2)
                        CreateReadMethod(method);
                    else
                        CreateReadRangeMethod(method);
                }
                else if (method.Name == "Write" || (method.Name == "Write2D"))
                {
                    if (method.Parameters.Count == 2)
                        CreateWriteMethod(method);
                    else
                        CreateWriteRangeMethod(method);
                }
            }
            else if (method.HasBody)
            {
                var ilProcessor = method.Body.GetILProcessor();

                var instructions = method.Body.Instructions;
                Instruction instruction = null;
                Instruction previousInstruction;
                for (int i = 0; i < instructions.Count; i++)
                {
                    previousInstruction = instruction;
                    instruction = instructions[i];

                    if (instruction.OpCode == OpCodes.Call && instruction.Operand is MethodReference)
                    {
                        var methodDescription = (MethodReference)instruction.Operand;

                        if (methodDescription is MethodDefinition)
                        {
                            foreach (var customAttribute in ((MethodDefinition)methodDescription).CustomAttributes)
                            {
                                if (customAttribute.AttributeType.FullName == typeof(ObfuscationAttribute).FullName)
                                {
                                    foreach (var arg in customAttribute.Properties)
                                    {
                                        if (arg.Name == "Feature" && arg.Argument.Value != null)
                                        {
                                            var customValue = arg.Argument.Value.ToString();
                                            if (customValue.StartsWith("SharpJit."))
                                            {
                                                isSharpJit = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (isSharpJit) break;
                            }
                        }

                        if (!isSharpJit)
                        {
                            if (methodDescription.Name.StartsWith("Calli") && methodDescription.DeclaringType.Name == "LocalInterop")
                            {
                                var callSite = new CallSite(methodDescription.ReturnType) { CallingConvention = MethodCallingConvention.StdCall };
                                // Last parameter is the function ptr, so we don't add it as a parameter for calli
                                // as it is already an implicit parameter for calli
                                for (int j = 0; j < methodDescription.Parameters.Count - 1; j++)
                                {
                                    var parameterDefinition = methodDescription.Parameters[j];
                                    callSite.Parameters.Add(parameterDefinition);
                                }

                                // Create calli Instruction
                                var callIInstruction = ilProcessor.Create(OpCodes.Calli, callSite);

                                // Replace instruction
                                ilProcessor.Replace(instruction, callIInstruction);
                            } 
                            else if (methodDescription.DeclaringType.Name == "Interop")
                            {
                                if (methodDescription.FullName.Contains("Fixed"))
                                {
                                    if (methodDescription.Parameters[0].ParameterType.IsArray)
                                    {
                                        ReplaceFixedArrayStatement(method, ilProcessor, instruction);
                                    }
                                    else
                                    {
                                        ReplaceFixedStatement(method, ilProcessor, instruction);
                                    }
                                }
                                else if (methodDescription.Name.StartsWith("ReadInline"))
                                {
                                    this.ReplaceReadInline(method, ilProcessor, instruction);
                                }
                                else if (methodDescription.Name.StartsWith("CopyInline") || methodDescription.Name.StartsWith("WriteInline"))
                                {
                                    this.ReplaceCopyInline(method, ilProcessor, instruction);
                                }
                                else if (methodDescription.Name.StartsWith("SizeOf"))
                                {
                                    this.ReplaceSizeOfStructGeneric(method, ilProcessor, instruction);
                                }
                            }
                        }
                    }
                }
            }
            return isSharpJit;
        }

        bool containsSharpJit;

        /// <summary>
        /// Patches the type.
        /// </summary>
        /// <param name="type">The type.</param>
        void PatchType(TypeDefinition type)
        {
            // Patch methods
            foreach (var method in type.Methods)
                if (PatchMethod(method))
                    containsSharpJit = true;

            // LocalInterop will be removed after the patch only for non SharpJit code
            if (!containsSharpJit && type.Name == "LocalInterop")
                classToRemoveList.Add(type);

            // Patch nested types
            foreach (var typeDefinition in type.NestedTypes)
                PatchType(typeDefinition);
        }

        /// <summary>
        /// Determines whether [is file check updated] [the specified file].
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="fromFile">From file.</param>
        /// <returns>
        /// 	<c>true</c> if [is file check updated] [the specified file]; otherwise, <c>false</c>.
        /// </returns>
        static bool IsFileCheckUpdated(string file, string fromFile)
        {
            return File.Exists(file) && File.GetLastWriteTime(file) == File.GetLastWriteTime(fromFile);
        }

        /// <summary>
        /// Get Program Files x86
        /// </summary>
        /// <returns></returns>
        static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))
                || Environment.GetEnvironmentVariable("ProgramFiles(x86)") != null)
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        /// <summary>
        /// Patches the file.
        /// </summary>
        /// <param name="file">The file.</param>
        public bool PatchFile(string file)
        {
            file = Path.Combine(Environment.CurrentDirectory, file);

            var fileTime = new FileTime(file);
            //var fileTimeInteropBuilder = new FileTime(Assembly.GetExecutingAssembly().Location);
            string checkFile = Path.GetFullPath(file) + ".check";
            //string checkInteropBuilderFile = "InteropBuild.check";

            // If checkFile and checkInteropBuilderFile up-to-date, then nothing to do
            if (fileTime.CheckFileUpToDate(checkFile))
            {
                Log("Nothing to do. SharpDX patch was already applied for assembly [{0}]", file);
                return false;
            }

            // Copy PDB from input assembly to output assembly if any
            var readerParameters = new ReaderParameters();
            var resolver = new DefaultAssemblyResolver();
            readerParameters.AssemblyResolver = resolver;
            var writerParameters = new WriterParameters();
            var pdbName = Path.ChangeExtension(file, "pdb");
            if (File.Exists(pdbName))
            {
                var symbolReaderProvider = new PdbReaderProvider();
                readerParameters.SymbolReaderProvider = symbolReaderProvider;
                readerParameters.ReadSymbols = true;
                writerParameters.WriteSymbols = true;
            }

            // Read Assembly
            assembly = AssemblyDefinition.ReadAssembly(file, readerParameters);
            resolver.AddSearchDirectory(Path.GetDirectoryName(file));

            // Query the target framework in order to resolve correct assemblies and type forwarding
            var targetFrameworkAttr = assembly.CustomAttributes.FirstOrDefault(
                attribute => attribute.Constructor.FullName.Contains("System.Runtime.Versioning.TargetFrameworkAttribute"));
            if(targetFrameworkAttr != null && targetFrameworkAttr.ConstructorArguments.Count > 0 &&
                targetFrameworkAttr.ConstructorArguments[0].Value != null)
            {
                string netcoreAssemblyPath;
#if !CORECLR
                var targetFramework = new FrameworkName(targetFrameworkAttr.ConstructorArguments[0].Value.ToString());

                netcoreAssemblyPath = string.Format(@"Reference Assemblies\Microsoft\Framework\{0}\v{1}",
                    targetFramework.Identifier,
                    targetFramework.Version);
                netcoreAssemblyPath = Path.Combine(ProgramFilesx86(), netcoreAssemblyPath);
#else
                    // For the time being that path might need to be updated.
                netcoreAssemblyPath = ".\\deps\\CoreFX\\CoreCLR\\v5.0\\";
#endif
                if(Directory.Exists(netcoreAssemblyPath))
                {
                    resolver.AddSearchDirectory(netcoreAssemblyPath);
                }
            }

            // Import void* and int32 
            voidType = assembly.MainModule.TypeSystem.Void.Resolve();
            voidPointerType = new PointerType(assembly.MainModule.Import(voidType));
            intType = assembly.MainModule.Import( assembly.MainModule.TypeSystem.Int32.Resolve());

            // Remove CompilationRelaxationsAttribute
            for (int i = 0; i < assembly.CustomAttributes.Count; i++)
            {
                var customAttribute = assembly.CustomAttributes[i];
                if (customAttribute.AttributeType.FullName == typeof(CompilationRelaxationsAttribute).FullName)
                {
                    assembly.CustomAttributes.RemoveAt(i);
                    i--;
                }
            }

            Log("SharpDX interop patch for assembly [{0}]", file);
            foreach (var type in assembly.MainModule.Types)
                PatchType(type);

            // Remove All Interop classes
            foreach (var type in classToRemoveList)
                assembly.MainModule.Types.Remove(type);

            var outputFilePath = file;
            assembly.Write(outputFilePath, writerParameters);

            fileTime = new FileTime(file);
            // Update Check file
            fileTime.UpdateCheckFile(checkFile);
            //fileTimeInteropBuilder.UpdateCheckFile(checkInteropBuilderFile);
                                
            Log("SharpDX patch done for assembly [{0}]", file);
            return true;
        }
        /// <summary>
        /// Main of this program.
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    Console.WriteLine("{0} file_path is expecting one file argument",
                                      Assembly.GetExecutingAssembly().GetName().Name);
                    Environment.Exit(1);
                }

                string file = args[0];
                var program = new InteropApp();
                program.PatchFile(file);
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(1);
            }
        }

        public void Log(string message, params object[] parameters)
        {
            Console.WriteLine(message, parameters);
        }

        public void LogError(string message, params object[] parameters)
        {
            Console.WriteLine(message, parameters);
        }

        public void LogError(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        /// <summary>
        /// FileTime.
        /// </summary>
        class FileTime
        {
            private DateTime CreateTime;
            private DateTime LastAccessTime;
            private DateTime LastWriteTime;

            public FileTime(string file)
            {
                CreateTime = File.GetCreationTime(file);
                LastAccessTime = File.GetLastAccessTime(file);
                LastWriteTime = File.GetLastWriteTime(file);
            }

            public void UpdateCheckFile(string checkFile)
            {
                File.WriteAllText(checkFile, "");
                UpdateFile(checkFile);
            }

            /// <summary>
            /// Checks the file.
            /// </summary>
            /// <param name="checkfile">The file to check.</param>
            /// <returns>true if the file exist and has the same LastWriteTime </returns>
            public bool CheckFileUpToDate(string checkfile)
            {
                return File.Exists(checkfile) && File.GetLastWriteTime(checkfile) == LastWriteTime;                
            }

            public void UpdateFile(string file)
            {
                File.SetCreationTime(file, CreateTime);
                File.SetLastWriteTime(file, LastWriteTime);
                File.SetLastAccessTime(file, LastAccessTime);
            }
        }

    }
}

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
using System.Linq;
using System.Text.RegularExpressions;
using SharpGen.Logging;
using SharpGen.CppModel;
using SharpGen.Model;

namespace SharpGen.Generator
{
    /// <summary>
    /// Transforms a C++ struct to a C# struct.
    /// </summary>
    public class StructTransform : TransformBase
    {
        private readonly Dictionary<Regex, string> _mapMoveStructToInner = new Dictionary<Regex, string>();

        /// <summary>
        /// Moves a C++ struct to an inner C# struct.
        /// </summary>
        /// <param name="fromStruct">From C++ struct regex query.</param>
        /// <param name="toStruct">To C# struct.</param>
        public void MoveStructToInner(string fromStruct, string toStruct)
        {
            _mapMoveStructToInner.Add(new Regex("^" + fromStruct + "$"), toStruct);
        }

        /// <summary>
        /// Prepares C++ struct for mapping. This method is creating the associated C# struct.
        /// </summary>
        /// <param name="cppElement">The c++ struct.</param>
        /// <returns></returns>
        public override CsBase Prepare(CppElement cppElement)
        {
            var cppStruct = (CppStruct) cppElement;

            // Create a new C# struct
            var nameSpace = Manager.ResolveNamespace(cppStruct);
            var csStruct = new CsStruct(cppStruct)
                                   {
                                       Name = NamingRules.Rename(cppStruct),
                                       // IsFullyMapped to false => The structure is being mapped
                                       IsFullyMapped = false
                                   };

            // Add the C# struct to its namespace
            nameSpace.Add(csStruct);

            // Map the C++ name to the C# struct
            Manager.BindType(cppStruct.Name, csStruct);
            return csStruct;
        }


        /// <summary>
        /// Processes the specified C# element to complete the mapping process between the C++ and C# element.
        /// </summary>
        /// <param name="csElement">The C# element.</param>
        public override void Process(CsBase csElement)
        {
            Process((CsStruct)csElement);
        }

        /// <summary>
        /// Maps the C++ struct to C# struct.
        /// </summary>
        /// <param name="csStruct">The c sharp struct.</param>
        private void Process(CsStruct csStruct)
        {
            // TODO: this mapping must be robust. Current calculation for field offset is not always accurate for union.
            // TODO: need to handle align/packing correctly.

            // If a struct was already mapped, then return immediately
            // The method MapStruct can be called recursively
            if (csStruct.IsFullyMapped)
                return;

                // Set IsFullyMappy in order to avoid recursive mapping
                csStruct.IsFullyMapped = true;

                // Get the associated CppStruct and CSharpTag
                var cppStruct = (CppStruct)csStruct.CppElement;
                bool hasMarshalType = csStruct.HasMarshalType;

                // If this structure need to me moved to another container, move it now
                foreach (var keyValuePair in _mapMoveStructToInner)
                {
                    if (keyValuePair.Key.Match(csStruct.CppElementName).Success)
                    {
                        string cppName = keyValuePair.Key.Replace(csStruct.CppElementName, keyValuePair.Value);
                        var destSharpStruct = (CsStruct)Manager.FindBindType(cppName);
                        // Remove the struct from his container
                        csStruct.Parent.Remove(csStruct);
                        // Add this struct to the new container struct
                        destSharpStruct.Add(csStruct);
                    }
                }

                // Current offset of a field
                int currentFieldAbsoluteOffset = 0;

                int fieldCount = cppStruct.IsEmpty ? 0 : cppStruct.Items.Count;


                // Last field offset
                int previousFieldOffsetIndex = -1;

                // Size of the last field
                int previousFieldSize = 0;

                // 
                int maxSizeOfField = 0;

                bool isInUnion = false;

                int cumulatedBitOffset = 0;

                // -------------------------------------------------------------------------------
                // Iterate on all fields and perform mapping
                // -------------------------------------------------------------------------------
                for (int fieldIndex = 0; fieldIndex < fieldCount; fieldIndex++)
                {
                    var cppField = (CppField) cppStruct.Items[fieldIndex];
                    Logger.RunInContext(cppField.ToString(), () =>  
                    {
                        var fieldStruct = Manager.GetCsType<CsField>(cppField, true);
                        csStruct.Add(fieldStruct);

                        // Get name
                        fieldStruct.Name = NamingRules.Rename(cppField);

                        // BoolToInt doesn't generate native Marshaling although they have a different marshaller
                        if (!fieldStruct.IsBoolToInt && fieldStruct.HasMarshalType)
                            hasMarshalType = true;


                        // If last field has same offset, then it's a union
                        // CurrentOffset is not moved
                        if (isInUnion && previousFieldOffsetIndex != cppField.Offset)
                        {
                            previousFieldSize = maxSizeOfField;
                            maxSizeOfField = 0;
                            isInUnion = false;
                        }

                        currentFieldAbsoluteOffset += previousFieldSize;
                        var fieldAlignment = (fieldStruct.MarshalType ?? fieldStruct.PublicType).CalculateAlignment();

                        // If field alignment is < 0, then we have a pointer somewhere so we can't align
                        if (fieldAlignment > 0)
                        {
                            // otherwise, align the field on the alignment requirement of the field
                            int delta = (currentFieldAbsoluteOffset % fieldAlignment);
                            if (delta != 0)
                            {
                                currentFieldAbsoluteOffset += fieldAlignment - delta;
                            }
                        }

                        // Get correct offset (for handling union)
                        fieldStruct.Offset = currentFieldAbsoluteOffset;
                        fieldStruct.IsBitField = cppField.IsBitField;

                        // Handle bit fields : calculate BitOffset and BitMask for this field
                        if (previousFieldOffsetIndex != cppField.Offset)
                        {
                            cumulatedBitOffset = 0;
                        }
                        if (cppField.IsBitField)
                        {
                            int lastCumulatedBitOffset = cumulatedBitOffset;
                            cumulatedBitOffset += cppField.BitOffset;
                            fieldStruct.BitMask = ((1 << cppField.BitOffset) - 1); // &~((1 << (lastCumulatedBitOffset + 1)) - 1);
                            fieldStruct.BitOffset = lastCumulatedBitOffset;
                        }


                        var nextFieldIndex = fieldIndex + 1;
                        if ((previousFieldOffsetIndex == cppField.Offset)
                           || (nextFieldIndex < fieldCount && ((CppField)cppStruct.Items[nextFieldIndex]).Offset == cppField.Offset))
                        {
                            if(previousFieldOffsetIndex != cppField.Offset)
                            {
                                maxSizeOfField = 0;
                            }
                            maxSizeOfField = fieldStruct.SizeOf > maxSizeOfField ? fieldStruct.SizeOf : maxSizeOfField;
                            isInUnion = true;
                            csStruct.ExplicitLayout = true;
                            previousFieldSize = 0;
                        }
                        else
                        {
                            previousFieldSize = fieldStruct.SizeOf;
                        }
                        previousFieldOffsetIndex = cppField.Offset;
                    });
                }

            // In case of explicit layout, check that we can safely generate it on both x86 and x64 (in case of an union
            // using pointers, we can't)
            if(csStruct.ExplicitLayout)
            {
                var fieldList = csStruct.Fields.ToList();
                for(int i = 0; i < fieldList.Count; i++)
                {
                    var field = fieldList[i];
                    var fieldAlignment = (field.MarshalType ?? field.PublicType).CalculateAlignment();

                    if(fieldAlignment < 0)
                    {
                        // If pointer field is not the last one, than we can't handle it
                        if ((i + 1) < fieldList.Count)
                        {
                            Logger.Error(
                                "The field [{0}] in structure [{1}] has pointer alignment within a structure that contains an union. An explicit layout cannot be handled on both x86/x64. This structure needs manual layout (remove fields from definition) and write them manually in xml mapping files",
                                field.CppElementName,
                                csStruct.CppElementName);
                            break;
                        }
                    }
                }
            }

            csStruct.SizeOf = currentFieldAbsoluteOffset + previousFieldSize;
            csStruct.HasMarshalType = hasMarshalType;
        }
    }
}
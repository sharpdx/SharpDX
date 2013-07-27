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
using System.Linq;
using SharpGen.Logging;
using SharpGen.Config;
using SharpGen.CppModel;
using SharpGen.Model;

namespace SharpGen.Generator
{
    /// <summary>
    /// Transforms a C++ enum to a C# enum definition.
    /// </summary>
    public class EnumTransform : TransformBase
    {
        /// <summary>
        /// Prepares the specified C++ element to a C# element.
        /// </summary>
        /// <param name="cppElement">The C++ element.</param>
        /// <returns>The C# element created and registered to the <see cref="TransformManager"/></returns>
        public override CsBase Prepare(CppElement cppElement)
        {
            var cppEnum = (CppEnum) cppElement;

            // Create C# enum
            var newEnum = new CsEnum
            {
                Name = NamingRules.Rename(cppEnum),
                CppElement = cppEnum
            };

            // Get the namespace for this particular include and enum
            var nameSpace = Manager.ResolveNamespace(cppEnum);
            nameSpace.Add(newEnum);

            // Bind C++ enum to C# enum
            Manager.BindType(cppEnum.Name, newEnum);

            return newEnum;
        }

        /// <summary>
        /// Processes the specified C# element to complete the mapping process between the C++ and C# element.
        /// </summary>
        /// <param name="csElement">The C# element.</param>
        public override void Process(CsBase csElement)
        {
            Process((CsEnum)csElement);
        }
        
        /// <summary>
        /// Maps a C++ Enum to a C# enum.
        /// </summary>
        /// <param name="cppEnum">the c++ enum.</param>
        private void Process(CsEnum newEnum)
        {
            var cppEnum = (CppEnum) newEnum.CppElement;

            // Get tag from C++ enum
            var tag = cppEnum.GetTagOrDefault<MappingRule>();

            // Determine enum type. Default is int
            string typeName = cppEnum.GetTypeNameWithMapping();
            switch (typeName)
            {
                case "byte":
                    newEnum.Type = typeof(byte);
                    newEnum.SizeOf = 1;
                    break;
                case "short":
                    newEnum.Type = typeof(short);
                    newEnum.SizeOf = 1;
                    break;
                case "int":
                    newEnum.Type = typeof(int);
                    newEnum.SizeOf = 4;
                    break;
                default:
                    Logger.Error("Invalid type [{0}] for enum [{1}]. Types supported are : int, byte, short", typeName, cppEnum);
                    break;
            }

            // Find Root Name of this enum
            // All enum items should start with the same root name and the root name should be at least 4 chars
            string rootName = cppEnum.Name;
            string rootNameFound = null;
            bool isRootNameFound = false;
            for (int i = rootName.Length; i >= 4 && !isRootNameFound; i--)
            {
                rootNameFound = rootName.Substring(0, i);

                isRootNameFound = true;
                foreach (var cppEnumItem in cppEnum.EnumItems)
                {
                    if (!cppEnumItem.Name.StartsWith(rootNameFound))
                    {
                        isRootNameFound = false;
                        break;
                    }
                }
            }
            if (isRootNameFound)
                rootName = rootNameFound;

            // Create enum items for enum
            foreach (var cppEnumItem in cppEnum.EnumItems)
            {
                string enumName = NamingRules.Rename(cppEnumItem, rootName);
                string enumValue = cppEnumItem.Value;

                var csharpEnumItem = new CsEnumItem(enumName, enumValue) { CppElement = cppEnumItem };

                newEnum.Add(csharpEnumItem);

                if (cppEnumItem.Name != "None")
                    Manager.BindType(cppEnumItem.Name, csharpEnumItem);
            }

            bool tryToAddNone = tag.EnumHasNone.HasValue ? tag.EnumHasNone.Value : false;

            // If C++ enum name is ending with FLAG OR FLAGS
            // Then tag this enum as flags and add None if necessary
            if (cppEnum.Name.EndsWith("FLAG") || cppEnum.Name.EndsWith("FLAGS"))
            {
                newEnum.IsFlag = true;

                if (!tag.EnumHasNone.HasValue)
                    tryToAddNone = !newEnum.Items.Cast<CsEnumItem>().Any(item => item.Name == "None");
            }

            // Add None value
            if (tryToAddNone)
            {
                var csharpEnumItem = new CsEnumItem("None", "0");
                csharpEnumItem.CppElement = new CppElement { Description = "None." };
                newEnum.Add(csharpEnumItem);
            }
        }
    }
}
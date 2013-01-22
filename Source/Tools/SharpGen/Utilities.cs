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
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;

namespace SharpGen
{
    /// <summary>
    /// Utility class.
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Escapes the xml/html text in order to use it inside xml.
        /// </summary>
        /// <param name="stringToEscape">The string to escape.</param>
        /// <returns></returns>
        public static string EscapeXml(string stringToEscape)
        {
            return stringToEscape.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");            
        }

        /// <summary>
        /// Gets a resource from this assembly.
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The text of resource</returns>
        public static string GetResourceAsString(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            string val = "";
            //' resources are named using a fully qualified name
            Stream strm = asm.GetManifestResourceStream(typeof (Utilities).Namespace + "." + resourceName);

            //' read the contents of the embedded file
            var reader = new StreamReader(strm);
            val = reader.ReadToEnd();
            reader.Close();

            return val;
        }

        /// <summary>
        /// Imports the XML types.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="mappings">The mappings.</param>
        /// <param name="importedTypes">The imported types.</param>
        /// <param name="importer">The importer.</param>
        private static void ImportXmlTypes(Type type, List<XmlMapping> mappings, List<Type> importedTypes, XmlReflectionImporter importer)
        {
            XmlTypeMapping mapping = null;
            var importer2 = new XmlReflectionImporter();
            try
            {
                mapping = importer2.ImportTypeMapping(type);
            }
            catch (Exception exception)
            {
                if (((exception is ThreadAbortException) || (exception is StackOverflowException)) || (exception is OutOfMemoryException))
                {
                    throw;
                }
                return;
            }
            if (mapping != null)
            {
                mapping = importer.ImportTypeMapping(type);
                mappings.Add(mapping);
                importedTypes.Add(type);
            }
        }

        /// <summary>
        /// Generates XmlSerializers assembly for this assembly, allowing faster startup with xml serialization.
        /// </summary>
        public static void SGenThisAssembly()
        {
            var xmlRootTypes = new List<Type>();
            var assembly = typeof(Utilities).Assembly;
            var mappings = new List<XmlMapping>();
            var allXmlTypeToSerialize = new List<Type>();
            var importer = new XmlReflectionImporter();

            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(XmlRootAttribute), true).Length > 0)
                {
                    xmlRootTypes.Add(type);
                    ImportXmlTypes(type, mappings, allXmlTypeToSerialize, importer);
                }
            }

            if (allXmlTypeToSerialize.Count == 0)
                return;

            string assemblySerializer = XmlSerializer.GetXmlSerializerAssemblyName(allXmlTypeToSerialize[0], null) + ".dll";
            var assemblySerializerTime = File.GetLastWriteTime(assemblySerializer);

            if (!File.Exists(assemblySerializer) || File.GetLastWriteTime(typeof(Utilities).Assembly.Location) > assemblySerializerTime)
            {
                // Delete previous assembly
                File.Delete(assemblySerializer);

                // Regenerate assembly
                var parameters = new CompilerParameters();
                string codePath = Path.GetDirectoryName(assembly.Location);
                var files = new TempFileCollection(codePath, false);
                parameters.TempFiles = files;
                parameters.GenerateInMemory = false;
                parameters.IncludeDebugInformation = true;
                parameters.GenerateInMemory = false;
                XmlSerializer.GenerateSerializer(allXmlTypeToSerialize.ToArray(), mappings.ToArray(), parameters);
                files.Delete();
            }
            else
            {
                Assembly.LoadFrom(assemblySerializer);
            }
        }
    }
}
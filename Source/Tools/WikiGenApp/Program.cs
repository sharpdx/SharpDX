using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Mono.Cecil;

namespace WikiGenApp
{
    class Program : IDisposable
    {
        Dictionary<string, XmlNode> cacheNodes = new Dictionary<string, XmlNode>();
        Dictionary<string, AssemblyDefinition> assemblies = new Dictionary<string, AssemblyDefinition>();
        private DefaultAssemblyResolver defaultResolver;

        private TextWriter writer;
        private TextWriter indexWriter;
        private int currentPageIndex;

        static bool IsComObject(TypeDefinition typeDef)
        {
            while (true)
            {
                if (typeDef.Name == "ComObject")
                {
                    return true;
                }
                if (typeDef.BaseType != null)
                {
                    typeDef = typeDef.BaseType.Resolve();
                }
                else
                {
                    return false;
                }
            }
        }

        private Program()
        {
            defaultResolver = new DefaultAssemblyResolver();
            defaultResolver.AddSearchDirectory(@"..\..\..\..\..\Bin\Desktop\");
            writer = new StringWriter();

            indexWriter = new StreamWriter("index.md");
        }

        private void WriteTableHeader(TextWriter writerArg)
        {
            writerArg.WriteLine(@"Managed | Native
----- | --------------------------------");
        }

        public void Dispose()
        {
            indexWriter.Close();
        }

        public void Run()
        {

            indexWriter.WriteLine("---");
            indexWriter.WriteLine("layout: wiki");
            indexWriter.WriteLine("title: Class Library API");
            indexWriter.WriteLine("---");
            indexWriter.WriteLine();
            indexWriter.WriteLine("The class library reference contains links between the native API and the managed equivalent in `SharpDX` assemblies.");
       
            indexWriter.WriteLine();
            indexWriter.WriteLine("# Graphics 3D APIs");
            indexWriter.WriteLine();
            WriteTableHeader(indexWriter);

            ProcessAssembly("SharpDX.Direct3D9", GetMsdnLink("Direct3D9", "bb219837"),
    "Microsoft Direct3D 9 graphics API (deprecated)");

            ProcessAssembly("SharpDX.Direct3D11", GetMsdnLink("Direct3D11", "ff476080"),
                "Microsoft Direct3D 11 graphics to create 3-D graphics for games and scientific and desktop applications.");

            ProcessAssembly("SharpDX.Direct3D12", GetMsdnLink("Direct3D12", "dn903821"),
                "The Direct3D 12 programming guide contains information about how to use the Direct3D 12 programmable pipeline to create a customized graphics engine.");

            ProcessAssembly("SharpDX.D3DCompiler", GetMsdnLink("D3DCompiler", "dd607340"),
                "The Direct3D shader compiler API.");

            ProcessAssembly("SharpDX.DXGI", GetMsdnLink("DXGI", "hh404534"),
                "Microsoft DirectX Graphics Infrastructure (DXGI) handles enumerating graphics adapters, enumerating display modes, selecting buffer formats, sharing resources between processes (such as, between applications and the Desktop Window Manager (DWM)), and presenting rendered frames to a window or monitor for display.");


            indexWriter.WriteLine();
            indexWriter.WriteLine("# Graphics 2D APIs");
            indexWriter.WriteLine();
            WriteTableHeader(indexWriter);

            ProcessAssembly("SharpDX.Direct2D1", GetMsdnLink("Direct2D1", "dd370990"),
                "Direct2D is a hardware-accelerated, immediate-mode, 2-D graphics API that provides high performance and high-quality rendering for 2-D geometry, bitmaps, and text. The Direct2D API is designed to interoperate well with GDI, GDI+, and Direct3D.", "SharpDX.Direct2D1");

            ProcessAssembly("SharpDX.Direct2D1", GetMsdnLink("DirectWrite", "dd368038"),
                "Today's applications must support high-quality text rendering, resolution-independent outline fonts, and full Unicode text and layout support. DirectWrite, a DirectX API, provides these features and more.", "SharpDX.DirectWrite");

            ProcessAssembly("SharpDX.Direct2D1", GetMsdnLink("WIC", "ee719902"),
                "The Windows Imaging Component (WIC) is an extensible platform that provides low-level API for digital images.  WIC supports the standard web image formats, high dynamic range images, and raw camera data.", "SharpDX.WIC");

            ProcessAssembly("SharpDX.DirectComposition", GetMsdnLink("DirectComposition", "hh437371"),
    "Microsoft DirectComposition is a Windows component that enables high-performance bitmap composition with transforms, effects, and animation");

            indexWriter.WriteLine();
            indexWriter.WriteLine("# Input APIs");
            indexWriter.WriteLine();
            WriteTableHeader(indexWriter);

            ProcessAssembly("SharpDX.DirectInput", GetMsdnLink("DirectInput", "ee416842"),
                "The DirectInput API is used to process data from a joystick, or other game controller. The use of DirectInput for keyboard and mouse input is not recommended. You should use Windows messages instead (deprecated)");

            ProcessAssembly("SharpDX.XInput", GetMsdnLink("XInput", "hh405053"),
                "XInput Game Controller API enables applications to receive input from the Xbox 360 Controller for Windows.");

            ProcessAssembly("SharpDX.RawInput", GetMsdnLink("RawInput", "ms645536"),
                "RawInput API");

            indexWriter.WriteLine();
            indexWriter.WriteLine("# Audio and Media APIs");
            indexWriter.WriteLine();
            WriteTableHeader(indexWriter);

            ProcessAssembly("SharpDX.XAudio2", GetMsdnLink("XAudio2", "hh405049"),
                "XAudio2 is a low-level audio API that provides signal processing and mixing foundation for developing high performance audio engines for games.");

            ProcessAssembly("SharpDX.XAudio2", GetMsdnLink("X3DAudio", "ee415714"),
                "X3DAudio is an API used in conjunction with XAudio2 to create the illusion of a sound coming from a point in 3D space.", "SharpDX.X3DAudio");

            ProcessAssembly("SharpDX.DirectSound", GetMsdnLink("DirectSound", "ee416960"),
                "DirectSound API (deprecated)");

            ProcessAssembly("SharpDX.MediaFoundation", GetMsdnLink("MediaFoundation", "ms694197"),
                "Microsoft Media Foundation enables the development of applications and components for using digital media on Windows Vista and later.");
        }

        static void Main(string[] args)
        {

            //var doc = XDocument.Load(new StreamReader(new FileStream(@"../../../../../Bin/Desktop/SharpDX.Direct3D12.xml", FileMode.Open)));

            //var assembly = AssemblyDefinition.ReadAssembly(@"..\..\..\..\..\Bin\Desktop/SharpDX.Direct3D12.dll",

            using (var program = new Program())
            {
                program.Run();
            }

            //var device = assembly.MainModule.GetType("SharpDX.Direct3D12.Device");

            //var result = IsComObject(device);

            //var stringId = DocIdHelper.GetXmlId(device);
        }


        private AssemblyDefinition LoadAssembly(string assemblyName)
        {
            AssemblyDefinition assembly;
            if (assemblies.TryGetValue(assemblyName, out assembly))
            {
                return assembly;
            }

            var assemblyPath = defaultResolver.GetSearchDirectories()
                .Select(path => Path.Combine(path, assemblyName + ".dll"))
                .FirstOrDefault(path => File.Exists(path));

            assembly = AssemblyDefinition.ReadAssembly(assemblyPath,
                new ReaderParameters()
                {
                    AssemblyResolver = defaultResolver
                });
            assemblies.Add(assemblyName, assembly);

            var doc = new XmlDocument();
            doc.Load(Path.ChangeExtension(assemblyPath, "xml"));

            var items = doc.SelectNodes("/doc/members/member");
            cacheNodes.Clear();
            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var node = items[i];
                    if (node.Attributes != null)
                    {
                        var nameAttr = node.Attributes["name"];
                        if (nameAttr != null)
                        {
                            cacheNodes.Add(nameAttr.Value, node);
                        }
                    }
                }
            }

            return assembly;
        }

        private void ProcessAssembly(string assemblyName, string nativeName, string description, string namespaceName = null)
        {
            var assembly = LoadAssembly(assemblyName);

            currentPageIndex++;

            namespaceName = namespaceName ?? assemblyName;

            var enumTypes = new Dictionary<TypeDefinition, DocEntry>();
            var structTypes = new Dictionary<TypeDefinition, DocEntry>();
            var comTypes = new Dictionary<TypeDefinition, DocEntry>();

            foreach (var type in assembly.MainModule.Types)
            {
                if (type.Namespace != namespaceName)
                {
                    continue;
                }

                if (!type.IsPublic)
                {
                    continue;
                }

                var tuple = GetInfo(type);
                if (tuple == null)
                {
                    continue;
                }

                if (type.IsEnum)
                {
                    enumTypes.Add(type, tuple);
                }
                else if (IsComObject(type))
                {
                    comTypes.Add(type, tuple);
                }
                else
                {
                    structTypes.Add(type, tuple);
                }
            }

            var apiName = Path.GetExtension(namespaceName).Substring(1);
            var docId = apiName.ToLowerInvariant();

            writer = new StreamWriter(docId + ".md");

            writer.WriteLine("---");
            writer.WriteLine("layout: wiki");
            writer.WriteLine($"title: {apiName} API");
            writer.WriteLine("---");

            writer.WriteLine();

            writer.WriteLine("> This page is automatically generated from the assembly documentation.");
            writer.WriteLine("> ");
            writer.WriteLine($"> It provides links between managed types and methods in the `{assemblyName}` assembly and the original documentation of the {nativeName} API on MSDN.");

            writer.WriteLine();

            writer.WriteLine(EscapeHtmlToMarkdown(description));
            writer.WriteLine();

            indexWriter.WriteLine($"[`{namespaceName}`]({docId}.html) | {nativeName} <p>{EscapeHtmlToMarkdown(description)}</p>");

            writer.WriteLine("- <a href='#api-Enumerations'>Enumerations</a>");
            writer.WriteLine("- <a href='#api-Structures'>Structures</a>");
            writer.WriteLine("- <a href='#api-Interfaces'>Interfaces</a>");
            writer.WriteLine();

            ProcessTypes(enumTypes, "Enumerations");
            ProcessTypes(structTypes, "Structures");
            ProcessTypes(comTypes, "Interfaces", true);

            writer.Close();
        }

        private DocEntry GetInfo(MemberReference type)
        {
            var docId = DocIdHelper.GetXmlId(type);
            XmlNode doc2;
            cacheNodes.TryGetValue(docId, out doc2);

            var unManagedApi = GetTag(doc2, "unmanaged");
            if (unManagedApi == null)
            {
                return null;
            }
            var unManagedShortApi = GetTag(doc2, "unmanaged-short");
            var msdnId = GetTag(doc2, "msdn-id");

            var docEntry = new DocEntry(unManagedApi, unManagedShortApi, msdnId) {Summary = GetTag(doc2, "summary")};
            return docEntry;
        }

        private static string EscapeHtmlToMarkdown(string html)
        {
            var doc = HtmlAgilityPack.HtmlNode.CreateNode(html);
            var text = doc.InnerText;
            var firstPoint = text.IndexOf(".");
            if (firstPoint > 0)
            {
                text = text.Substring(0, firstPoint + 1);
            }

            return text?.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty);
        }

        private void ProcessTypes(Dictionary<TypeDefinition, DocEntry> types, string sectionName, bool displayMethods = false)
        {
            writer.WriteLine($"# <a id=\"api-{sectionName}\">{sectionName}</a>");
            writer.WriteLine();

            WriteTableHeader(writer);
            foreach (var type in types.Keys.OrderBy(k => k.Name))
            {
                var value = types[type];

                var managedMehtods = new StringBuilder();
                var nativeMethods = new StringBuilder();
                if (displayMethods)
                {
                    foreach (var method in type.Methods.Where(m => m.IsPublic).Cast<MemberReference>().Concat(type.Properties.Where(m => m.GetMethod?.IsPublic ?? m.SetMethod?.IsPublic ?? false )).OrderBy(m => m.Name))
                    {
                        var tuple = GetInfo(method);
                        if (tuple == null)
                        {
                            continue;
                        }

                        managedMehtods.Append($"<li>`{method.Name}`</li>");
                        nativeMethods.Append($"<li>{GetNativeLink(tuple)}</li>");
                    }
                }

                writer.Write($"`{type.Name}`");

                if (managedMehtods.Length > 0)
                {
                    writer.Write($"<ul>{managedMehtods}</ul>");
                }

                writer.Write(" | ");

                writer.Write(GetNativeLink(value));

                if (nativeMethods.Length > 0)
                {
                    writer.Write($"<ul>{nativeMethods}</ul>");
                }

                if (value.Summary != null)
                {
                    writer.Write("<p>");
                    writer.Write(EscapeHtmlToMarkdown(value.Summary));
                    writer.Write("</p>");
                }

                writer.WriteLine();
            }

            writer.WriteLine();
        }

        private string GetNativeLink(DocEntry value)
        {
            var shortApi = value.NativeShortApi ?? "???";
            var indexOfMethod = shortApi.IndexOf("::");
            if (indexOfMethod > 0)
            {
                shortApi = shortApi.Substring(indexOfMethod + 2);
            }
            return GetMsdnLink(shortApi, value.MsdnId);
        }

        private static string GetMsdnLink(string name, string msdnId)
        {
            return msdnId != null
                ? $"[`{name}`](https://msdn.microsoft.com/en-us/library/windows/desktop/{msdnId}.aspx)"
                : $"`{name}`";
        }

        /// <summary>
        /// Extract a comment from tag inside the <see cref="NModelBase.DocNode"/> associated
        /// to this element.
        /// </summary>
        /// <param name="docNode">The doc node.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns>
        /// The content of the tag or null if empty or not found
        /// </returns>
        public static string GetTag(XmlNode docNode, string tagName)
        {
            if (docNode != null)
            {
                var selectedNode = docNode.SelectSingleNode(tagName);
                if (selectedNode != null)
                    return selectedNode.InnerXml.Trim();
            }

            return null;
        }

        private class DocEntry
        {
            public DocEntry(string nativeApi, string nativeShortApi, string msdnId)
            {
                NativeApi = nativeApi;
                NativeShortApi = nativeShortApi;
                MsdnId = msdnId;
            }

            public string NativeApi;

            public string NativeShortApi;

            public string MsdnId;

            public string Summary;
        }
    }
}

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
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace SharpDocPak
{
    /// <summary>
    /// Internal class to index files.
    /// </summary>
    internal class DocumentIndexer
    {
        private IndexWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentIndexer"/> class.
        /// </summary>
        public DocumentIndexer()
        {
            Tags = new List<TagIndex>();
        }

        /// <summary>
        /// Gets or sets the directory index.
        /// </summary>
        /// <value>The index.</value>
        public RAMDirectory Index {get;set;}

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public List<TagIndex> Tags { get; set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init()
        {
            Index = new RAMDirectory();
            writer = new IndexWriter(Index, new StandardAnalyzer(Version.LUCENE_29), true, new IndexWriter.MaxFieldLength(0xf4240));

            var defaultTitleIndex = TagIndex.DefaultTitleIndex;
            if (!Tags.Contains(defaultTitleIndex))
                Tags.Add(defaultTitleIndex);

            var defaultContentIndex = TagIndex.DefaultContentIndex;
            if (!Tags.Contains(defaultContentIndex))
                Tags.Add(defaultContentIndex);
        }

        /// <summary>
        /// Adds the file to index. Only html and htm files will be indexed.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="contentBuffer">The content buffer.</param>
        public void AddFile(string file, byte[] contentBuffer)
        {
            var fileExt = Path.GetExtension(file);
            if (fileExt == null)
                return;
            fileExt = fileExt.ToLower();

            // Only index *.html and *.htm files
            if (fileExt != ".htm" && fileExt != ".html")
                return;

            var content = new StreamReader(new MemoryStream(contentBuffer)).ReadToEnd();

            file = file.Replace('\\', '/');

            var document = new Document();
            document.Add(new Field("file", file, Field.Store.YES, Field.Index.NOT_ANALYZED));
            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            
            // Store tags
            foreach (var tagIndex in Tags)
            {
                var tagNode = doc.DocumentNode.SelectSingleNode(tagIndex.TagPath);
                if (tagNode != null)
                {
                    // Cleanup html
                    CleanupHtml(tagNode);
                    document.Add(new Field(tagIndex.Id, StripText(tagNode.InnerText), Field.Store.YES, Field.Index.ANALYZED));
                }
            }

            // Add an index to this file
            writer.AddDocument(document);
        }

        /// <summary>
        /// Strips the text.
        /// </summary>
        /// <param name="htmlText">The HTML text.</param>
        /// <returns></returns>
        private static string StripText(string htmlText)
        {
            return Regex.Replace(System.Web.HttpUtility.HtmlDecode(htmlText), @"[\r\n\s]+", " ").Trim();            
        }

        /// <summary>
        /// Cleanups the HTML. Removes all comments/scripts and convert all div/span to paragraphs.
        /// </summary>
        /// <param name="rootNode">The root node.</param>
        private static void CleanupHtml(HtmlNode rootNode)
        {
            // Remove comments
            var nodes = rootNode.SelectNodes("//comment()|//script");
            if (nodes != null)
                foreach (var node in nodes)
                    node.Remove();
            nodes = rootNode.SelectNodes("//div|//span");
            if (nodes != null)
                foreach (var node in nodes)
                    node.Name = "p";            
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            writer.Optimize();
            writer.Close();
        }
    }
}
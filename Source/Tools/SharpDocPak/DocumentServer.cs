// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Version = Lucene.Net.Util.Version;

namespace SharpDocPak
{
    /// <summary>
    /// Process documentation on a local http server.
    /// </summary>
    internal partial class DocumentServer
    {
        private HttpServer server;
        private IndexReader indexReader;
        private IndexSearcher indexSearcher;
        private StandardAnalyzer analyzer;
        private QueryParser queryParser;

        public Archive Content { get; set; }

        public string Url { get; set; }

        private string searchOptions;

        private const string NoResultsFound = "No results found.";

        /// <summary>
        /// Inits this instance.
        /// </summary>
        public void Init()
        {
            server = new HttpServer();
            Url = server.Url;
            var indexDir = Content.Index;
            indexReader = IndexReader.Open(indexDir, true);
            indexSearcher = new IndexSearcher(indexReader);
            analyzer = new StandardAnalyzer(Version.LUCENE_29);

            server.ProcessRequest += ServerProcessRequest;
            server.Start();

            var labels = new StringBuilder();

            // Sort tags by alphabetical order
            Content.Tags.Sort((from, to) => from.Name.CompareTo(to.Name));

            foreach (var tagIndex in Content.Tags)
            {
                labels.AppendFormat(
                    @"<label style='float: left; margin-right: 8px'><input type='checkbox' name='{0}' checked='true'/>{1}</label>",
                    tagIndex.Id, tagIndex.Name).AppendLine();
            }
            searchOptions = labels.ToString();
        }

        /// <summary>
        /// Handles the ProcessRequest event of the server control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SharpDocPak.HttpRequestEventArgs"/> instance containing the event data.</param>
        void ServerProcessRequest(object sender, HttpRequestEventArgs e)
        {
            HttpListenerResponse response = null;
            bool keepAlive = false;
            try
            {
                Console.WriteLine("Url Requested: {0}", e.RequestContext.Request.Url);
                Console.Out.Flush();
                var url = e.RequestContext.Request.Url;
                response = e.RequestContext.Response;

                keepAlive = e.RequestContext.Request.KeepAlive;

                byte[] buffer = null;
                Content.Files.TryGetValue(url.AbsolutePath, out buffer);

                string path = url.AbsolutePath;

                //var lastTime = File.GetLastWriteTime(path);
                //var expireTime = DateTime.Now.AddDays(180);
                //Console.WriteLine("Expires: {0:r} Last-Modified: {1:r} KeepAlive: {2}", expireTime, lastTime, keepAlive);

                if (path.EndsWith(".js") || path.EndsWith(".css") || path.EndsWith(".htm"))
                {
                    if (path.EndsWith(".css"))
                    {
                        response.ContentType = "text/css";
                    }
                    else if (path.EndsWith(".js"))
                    {
                        response.ContentType = "text/javascript";
                    }
                    else if (path.EndsWith(".htm"))
                    {
                        response.ContentType = "text/html";
                    }

                    var textStream = new StreamReader(new MemoryStream(buffer), Encoding.UTF8);
                    response.ContentEncoding = textStream.CurrentEncoding;

                    // If URL query
                    if (path.EndsWith(".htm"))
                    {
                        var text = ApplySearchOptions(textStream.ReadToEnd());
                        if (IsQuery(url.Query))
                        {
                            text = RunSearchQuery(text, url.Query);
                        }
                        buffer = Encoding.UTF8.GetBytes(text);
                    }
                    response.ContentEncoding = Encoding.UTF8;
                }
                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "OK";
                //response.AppendHeader("Expires", string.Format("{0:r}",expireTime));
                //response.AppendHeader("Last-Modified", string.Format("{0:r}", lastTime));
                response.KeepAlive = keepAlive;
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                if (!keepAlive)
                    response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in server: {0}", ex);
            }

            try
            {
                if (!keepAlive)
                    response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while closing: {0}", ex);
            }
        }

        /// <summary>
        /// Determines whether the specified query is query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>
        /// 	<c>true</c> if the specified query is query; otherwise, <c>false</c>.
        /// </returns>
        private bool IsQuery(string query)
        {
            var queryMap = HttpUtility.ParseQueryString(query);
            return !string.IsNullOrEmpty(queryMap.Get("query"));
        }

        /// <summary>
        /// Applies the search options.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private string ApplySearchOptions(string text)
        {
            const string matchSearchOptions = "<div id='sharpdoc-search-opt'>";
            text = Regex.Replace(text, matchSearchOptions, (match) =>
            {
                var labels = new StringBuilder(matchSearchOptions);
                labels.Append(searchOptions);
                return labels.ToString();
            });
            return text;
        }

        /// <summary>
        /// Runs the search query.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="queryText">The query text.</param>
        /// <returns></returns>
        public string RunSearchQuery(string doc, string queryText)
        {
            const string searchResultsId = "id=\"search-results\">";
            int indexInsert = doc.IndexOf(searchResultsId) + searchResultsId.Length;
            
            var queryMap = HttpUtility.ParseQueryString(queryText);
            queryText = queryMap.Get("query");

            var builder = new StringBuilder();
            bool resultsFound = false;
            try
            {
                var fields = new List<string>();
                for (int i = 0; i < Content.Tags.Count; i++)
                {
                    if (queryMap.Get(Content.Tags[i].Id) != null)
                        fields.Add(Content.Tags[i].Id);
                }

                if (fields.Count > 0) 
                {
                    queryParser = new MultiFieldQueryParser(Version.LUCENE_29, fields.ToArray(), analyzer);

                    var query = queryParser.Parse(queryText);
                    var maxDocs = indexReader.MaxDoc();
                    var docs = indexSearcher.Search(query, null, maxDocs);

                    foreach (var hit in docs.scoreDocs)
                    {
                        resultsFound = true;
                        var documentFromSearcher = indexSearcher.Doc(hit.doc);

                        builder.AppendFormat("<dl>").AppendLine();
                        var title = documentFromSearcher.Get("title");
                        var urlFile = documentFromSearcher.Get("file");
                        builder.AppendFormat("<dt><a href='{0}'>{1}</a><dt>", urlFile, title);
                        var content = documentFromSearcher.Get("content");
                        content = content.Substring(0, Math.Min(500, content.Length));
                        builder.AppendFormat("<dd>{0}</dd>", content);
                        builder.AppendFormat("</dl>").AppendLine();
                        builder.AppendFormat("<hr>").AppendLine();
                    }
                } else
                {
                    builder.Append("<p>You must select at least one search option</p>");
                }
            }
            catch (Exception ex)
            {
                builder.Append("</br>").Append(ex);
            }

            // Print "no results found"
            if (!resultsFound)
                builder.Append(NoResultsFound);

            doc = doc.Insert(indexInsert, builder.ToString());
            return doc;
        }
    }
}
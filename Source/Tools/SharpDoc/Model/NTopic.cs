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
using System.Xml.Serialization;
using SharpCore;
using SharpCore.Logging;

namespace SharpDoc.Model
{
    /// <summary>
    /// Documentation topic store in an external file.
    /// </summary>
    [XmlType("topic")]
    public class NTopic : IModelReference
    {
        /// <summary>
        /// Id for the default class library topic
        /// </summary>
        public const string ClassLibraryTopicId = "X:ClassLibrary";

        /// <summary>
        /// Id for the default search results topic
        /// </summary>
        public const string SearchResultsTopicId = "X:SearchResults";
               
        /// <summary>
        /// Initializes a new instance of the <see cref="NTopic"/> class.
        /// </summary>
        public NTopic()
        {
            SubTopics = new List<NTopic>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NTopic"/> class.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public NTopic(IModelReference reference)
        {
            Id = reference.Id;
            NormalizedId = reference.NormalizedId;
            Name = reference.Name;
            FullName = reference.FullName;
            SubTopics = new List<NTopic>();
        }

        /// <summary>
        /// Gets or sets the XML generated commment ID.
        /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x.aspx for more information.
        /// </summary>
        /// <value>The id.</value>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the normalized id. This is a normalized version of the <see cref="IModelReference.Id"/> that
        /// can be used for filename.
        /// </summary>
        /// <value>The file id.</value>
        [XmlAttribute("fileid")]
        public string NormalizedId { get; set; }

        /// <summary>
        /// Gets or sets the name of this instance.
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full name of this instance.
        /// </summary>
        /// <value>The full name.</value>
        [XmlAttribute("fullname")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the name of the file that contains the documentation.
        /// </summary>
        /// <value>The name of the file.</value>
        [XmlAttribute("filename")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the html content. This is loaded from the filename.
        /// </summary>
        /// <value>The content.</value>
        [XmlIgnore]
        public string Content { get; set;  }

        /// <summary>
        /// Gets or sets the parent topic.
        /// </summary>
        /// <value>The parent topic.</value>
        [XmlIgnore]
        public NTopic Parent { get; set; }

        /// <summary>
        /// Gets or sets the sub topics.
        /// </summary>
        /// <value>The sub topics.</value>
        [XmlElement("topic")]
        public List<NTopic> SubTopics { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is class library.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is class library; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsClassLibrary
        {
            get { return Id == ClassLibraryTopicId; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is search result.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is search result; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool IsSearchResult
        {
            get { return Id == SearchResultsTopicId; }
        }

        /// <summary>
        /// Finds the topic by id.
        /// </summary>
        /// <param name="topicId">The topic id.</param>
        /// <returns></returns>
        public NTopic FindTopicById(string topicId)
        {
            if (Id == topicId)
                return this;
            return FindTopicById(SubTopics, topicId);
        }

        /// <summary>
        /// Finds the topic by id.
        /// </summary>
        /// <param name="topics">The topics.</param>
        /// <param name="topicId">The topic id.</param>
        /// <returns></returns>
        public static NTopic FindTopicById(IEnumerable<NTopic> topics, string topicId)
        {
            NTopic topicFound = null;
            foreach (var topic in topics)
            {
                topicFound = topic.FindTopicById(topicId);
                if (topicFound != null)
                    break;
            }
            return topicFound;
        }

        /// <summary>
        /// Associate topics with their parent
        /// </summary>
        public void BuildParents(NTopic parentTopic = null)
        {
            Parent = parentTopic;
            foreach (var subTopic in SubTopics)
                subTopic.BuildParents(this);
        }

        /// <summary>
        /// Gets the parents of this intance.
        /// </summary>
        /// <returns>Parents of this instance</returns>
        /// <remarks>
        /// The parents is ordered from the root level to this instance (excluding this instance)
        /// </remarks>
        public List<NTopic> GetParents()
        {
            var topics = new List<NTopic>();
            var topic = Parent;
            while (topic != null)
            {
                topics.Insert(0, topic);
                topic = topic.Parent;
            }
            return topics;
        }

        /// <summary>
        /// Loads the content of this topic.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        public void Init(string rootPath)
        {
            // Check that id is valid
            if (string.IsNullOrEmpty(Id))
                Logger.Error("Missing id for topic [{0}]", this);

            // Check that name is valid
            if (string.IsNullOrEmpty(Name))
            {
                if (Id == ClassLibraryTopicId)
                {
                    Name = "Class Library";
                }
                else
                {
                    Logger.Error("Missing name for topic [{0}]", this);
                }
            }


            // Copy Name to Fullname if empty
            if (string.IsNullOrEmpty(FullName))
                FullName = Name;

            rootPath = rootPath ?? "";

            // Initialize sub topics
            foreach(var topic in SubTopics)
                topic.Init(rootPath);

            if (Id != ClassLibraryTopicId)
            {
                // Load content file
                if (string.IsNullOrEmpty(FileName))
                {
                    // Check that filename is valid
                    Logger.Error("Filname for topic [{0}] cannot be empty", this);
                }
                else
                {
                    string filePath = null;
                    try
                    {
                        if (string.IsNullOrEmpty(NormalizedId))
                            NormalizedId = Path.GetFileNameWithoutExtension(FileName);
                        filePath = Path.Combine(rootPath, FileName);
                        Content = File.ReadAllText(filePath);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Cannot load content for topic [{0}] from path [{1}]. Reason: {2}", this, filePath, ex.Message);
                    }
                }
            }

            if (string.IsNullOrEmpty(NormalizedId))
                NormalizedId = DocIdHelper.StripXmlId(Id);

            // Check that NormalizeId is a valid filename
            if (!Utility.IsValidFilename(NormalizedId))
                Logger.Error("Invalid fileid/normalizedId [{0}] for topic [{1}]. Fileid must contain valid filename chars", NormalizedId, this);
        }

        /// <summary>
        /// Gets the default class library topic.
        /// </summary>
        /// <value>The default class library topic.</value>
        public static NTopic DefaultClassLibraryTopic
        {
            get
            {
                return new NTopic()
                           {
                               Id = ClassLibraryTopicId,
                               NormalizedId = "ClassLibrary",
                               Name = "Class Library"
                           };
            }
        }

        /// <summary>
        /// Gets the default search results topic.
        /// </summary>
        /// <value>The default search results topic.</value>
        public static NTopic DefaultSearchResultsTopic
        {
            get
            {
                return new NTopic()
                {
                    Id = SearchResultsTopicId,
                    NormalizedId = "SearchResults",
                    Name = "Search results"
                };
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "Id: {0}, NormalizedId: {1}, Name: {2}, FullName: {3}, FileName: {4}, SubTopics.Count: {5}", Id, NormalizedId, Name, FullName, FileName, SubTopics.Count);
        }
    }
}
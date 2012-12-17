 /*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Assimp.Configs;
using Assimp.Unmanaged;

namespace Assimp {
    /// <summary>
    /// Assimp importer that will use Assimp to load a model into managed memory.
    /// </summary>
    internal class AssimpImporter : IDisposable {
        private static Object s_sync = new Object();

        private bool m_isDisposed;
        private bool m_verboseEnabled;
        private Dictionary<String, PropertyConfig> m_configs;
        private List<LogStream> m_logStreams;

        private ExportFormatDescription[] m_exportFormats;
        private String[] m_importFormats;

        private float m_scale = 1.0f;
        private float m_xAxisRotation = 0.0f;
        private float m_yAxisRotation = 0.0f;
        private float m_zAxisRotation = 0.0f;
        private bool m_buildMatrix = false;
        private Matrix4x4 m_scaleRot = Matrix4x4.Identity;

        private IntPtr m_propStore = IntPtr.Zero;

        /// <summary>
        /// Gets if the importer has been disposed.
        /// </summary>
        public bool IsDisposed {
            get {
                return m_isDisposed;
            }
        }

        /// <summary>
        /// Gets or sets the uniform scale for the model. This is multiplied
        /// with the existing root node's transform.
        /// </summary>
        public float Scale {
            get {
                return m_scale;
            }
            set {
               if(m_scale != value) {
                    m_scale = value;
                    m_buildMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the model's rotation about the X-Axis, in degrees. This is multiplied
        /// with the existing root node's transform.
        /// </summary>
        public float XAxisRotation {
            get {
                return m_xAxisRotation;
            }
            set {
                if(m_xAxisRotation != value) {
                    m_xAxisRotation = value;
                    m_buildMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the model's rotation abut the Y-Axis, in degrees. This is multiplied
        /// with the existing root node's transform.
        /// </summary>
        public float YAxisRotation {
            get {
                return m_yAxisRotation;
            }
            set {
                if(m_yAxisRotation != value) {
                    m_yAxisRotation = value;
                    m_buildMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the model's rotation about the Z-Axis, in degrees. This is multiplied
        /// with the existing root node's transform.
        /// </summary>
        public float ZAxisRotation {
            get {
                return m_zAxisRotation;
            }
            set {
                if(m_zAxisRotation != value) {
                    m_zAxisRotation = value;
                    m_buildMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets if verbose logging should be enabled.
        /// </summary>
        public bool VerboseLoggingEnabled {
            get {
                return m_verboseEnabled;
            }
            set {
                m_verboseEnabled = value;
            }
        }

        /// <summary>
        /// Gets the property configurations set to this importer.
        /// </summary>
        public Dictionary<String, PropertyConfig> PropertyConfigurations {
            get {
                return m_configs;
            }
        }

        /// <summary>
        /// Gets the logstreams attached to this importer.
        /// </summary>
        public List<LogStream> LogStreams {
            get {
                return m_logStreams;
            }
        }

        /// <summary>
        /// Constructs a new AssimpImporter.
        /// </summary>
        public AssimpImporter() {
            m_configs = new Dictionary<String, PropertyConfig>();
            m_logStreams = new List<LogStream>();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="AssimpImporter"/> is reclaimed by garbage collection.
        /// </summary>
        ~AssimpImporter() {
            Dispose(false);
        }

        #region Import

        #region ImportFileFromStream

        /// <summary>
        /// Importers a model from the stream without running any post-process steps. The importer sets configurations
        /// and loads the model into managed memory, releasing the unmanaged memory used by Assimp. It is up to the caller to dispose of the stream.
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <param name="formatHint">Format extension to serve as a hint to Assimp to choose which importer to use</param>
        /// <returns>The imported scene</returns>
        /// <exception cref="AssimpException">Thrown if the stream is not valid (null or write-only) or if the format hint is null or empty.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public Scene ImportFileFromStream(Stream stream, String formatHint) {
            return ImportFileFromStream(stream, PostProcessSteps.None, formatHint);
        }

        /// <summary>
        /// Importers a model from the stream. The importer sets configurations
        /// and loads the model into managed memory, releasing the unmanaged memory used by Assimp. It is up to the caller to dispose of the stream.
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        /// <param name="postProcessFlags">Post processing flags, if any</param>
        /// <param name="formatHint">Format extension to serve as a hint to Assimp to choose which importer to use</param>
        /// <returns>The imported scene</returns>
        /// <exception cref="AssimpException">Thrown if the stream is not valid (null or write-only) or if the format hint is null or empty.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public Scene ImportFileFromStream(Stream stream, PostProcessSteps postProcessFlags, String formatHint) {
            lock(s_sync) {
                if(m_isDisposed) {
                    throw new ObjectDisposedException("Importer has been disposed.");
                }

                if(stream == null || stream.CanRead != true) {
                    throw new AssimpException("stream", "Can't read from the stream it's null or write-only");
                }

                if(String.IsNullOrEmpty(formatHint)) {
                    throw new AssimpException("formatHint", "Format hint is null or empty");
                }

                IntPtr ptr = IntPtr.Zero;
                PrepareImport();

                try {
                    ptr = AssimpMethods.ImportFileFromStream(stream, PostProcessSteps.None, formatHint, m_propStore);

                    if(ptr == IntPtr.Zero)
                        throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());

                    TransformScene(ptr);

                    ptr = AssimpMethods.ApplyPostProcessing(ptr, postProcessFlags);

                    return ValidateAndCreateScene(ptr);
                } finally {
                    CleanupImport();

                    if(ptr != IntPtr.Zero) {
                        AssimpMethods.ReleaseImport(ptr);
                    }
                }
            }
        }

        #endregion

        #region ImportFile

        /// <summary>
        /// Importers a model from the specified file without running any post-process steps. The importer sets configurations
        /// and loads the model into managed memory, releasing the unmanaged memory used by Assimp.
        /// </summary>
        /// <param name="file">Full path to the file</param>
        /// <returns>The imported scene</returns>
        /// <exception cref="AssimpException">Thrown if there was a general error in importing the model.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the file could not be located.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public Scene ImportFile(String file) {
            return ImportFile(file, PostProcessSteps.None);
        }

        /// <summary>
        /// Importers a model from the specified file. The importer sets configurations
        /// and loads the model into managed memory, releasing the unmanaged memory used by Assimp.
        /// </summary>
        /// <param name="file">Full path to the file</param>
        /// <param name="postProcessFlags">Post processing flags, if any</param>
        /// <returns>The imported scene</returns>
        /// <exception cref="AssimpException">Thrown if there was a general error in importing the model.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the file could not be located.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public Scene ImportFile(String file, PostProcessSteps postProcessFlags) {
            lock(s_sync) {
                if(m_isDisposed) {
                    throw new ObjectDisposedException("Importer has been disposed.");
                }
                if(String.IsNullOrEmpty(file) || !File.Exists(file)) {
                    throw new FileNotFoundException("Filename was null or could not be found", file);
                }

                IntPtr ptr = IntPtr.Zero;
                PrepareImport();
                try {
                    ptr = AssimpMethods.ImportFile(file, PostProcessSteps.None, m_propStore);

                    if(ptr == IntPtr.Zero)
                        throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());

                    TransformScene(ptr);

                    ptr = AssimpMethods.ApplyPostProcessing(ptr, postProcessFlags);

                    return ValidateAndCreateScene(ptr);
                } finally {
                    CleanupImport();

                    if(ptr != IntPtr.Zero) {
                        AssimpMethods.ReleaseImport(ptr);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region ConvertFromFile

        #region File to File

        /// <summary>
        /// Converts the model contained in the file to the specified format and save it to a file.
        /// </summary>
        /// <param name="inputFilename">Input file name to import</param>
        /// <param name="outputFilename">Output file name to export to</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        ///<exception cref="AssimpException">Thrown if there was a general error in importing the model.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the file could not be located.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public void ConvertFromFileToFile(String inputFilename, String outputFilename, String exportFormatId) {
            ConvertFromFileToFile(inputFilename, PostProcessSteps.None, outputFilename, exportFormatId, PostProcessSteps.None);
        }

        /// <summary>
        /// Converts the model contained in the file to the specified format and save it to a file.
        /// </summary>
        /// <param name="inputFilename">Input file name to import</param>
        /// <param name="outputFilename">Output file name to export to</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        /// <param name="exportProcessSteps">Pre processing steps used for the export</param>
        ///<exception cref="AssimpException">Thrown if there was a general error in importing the model.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the file could not be located.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public void ConvertFromFileToFile(String inputFilename, String outputFilename, String exportFormatId, PostProcessSteps exportProcessSteps) {
            ConvertFromFileToFile(inputFilename, PostProcessSteps.None, outputFilename, exportFormatId, exportProcessSteps);
        }

        /// <summary>
        /// Converts the model contained in the file to the specified format and save it to a file.
        /// </summary>
        /// <param name="inputFilename">Input file name to import</param>
        /// <param name="importProcessSteps">Post processing steps used for the import</param>
        /// <param name="outputFilename">Output file name to export to</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        /// <param name="exportProcessSteps">Pre processing steps used for the export</param>
        ///<exception cref="AssimpException">Thrown if there was a general error in importing the model.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the file could not be located.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public void ConvertFromFileToFile(String inputFilename, PostProcessSteps importProcessSteps, String outputFilename, String exportFormatId, PostProcessSteps exportProcessSteps) {
            lock(s_sync) {
                if(m_isDisposed) {
                    throw new ObjectDisposedException("Importer has been disposed.");
                }

                if(String.IsNullOrEmpty(inputFilename) || !File.Exists(inputFilename)) {
                    throw new FileNotFoundException("Filename was null or could not be found", inputFilename);
                }

                IntPtr ptr = IntPtr.Zero;
                PrepareImport();

                try {
                    ptr = AssimpMethods.ImportFile(inputFilename, PostProcessSteps.None, m_propStore);

                    if(ptr == IntPtr.Zero)
                        throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());

                    TransformScene(ptr);
                    ptr = AssimpMethods.ApplyPostProcessing(ptr, importProcessSteps);

                    ValidateScene(ptr);

                    AssimpMethods.ExportScene(ptr, exportFormatId, outputFilename, exportProcessSteps);
                } finally {
                    CleanupImport();

                    if(ptr != IntPtr.Zero)
                        AssimpMethods.ReleaseImport(ptr);
                }
            }
        }

        #endregion

        #region File to Blob

        /// <summary>
        /// Converts the model contained in the file to the specified format and save it to a data blob.
        /// </summary>
        /// <param name="inputFilename">Input file name to import</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        /// <returns>Data blob containing the exported scene in a binary form</returns>
        /// <exception cref="AssimpException">Thrown if there was a general error in importing the model.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the file could not be located.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public ExportDataBlob ConvertFromFileToBlob(String inputFilename, String exportFormatId) {
            return ConvertFromFileToBlob(inputFilename, PostProcessSteps.None, exportFormatId, PostProcessSteps.None);
        }

        /// <summary>
        /// Converts the model contained in the file to the specified format and save it to a data blob.
        /// </summary>
        /// <param name="inputFilename">Input file name to import</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        /// <param name="exportProcessSteps">Pre processing steps used for the export</param>
        /// <returns>Data blob containing the exported scene in a binary form</returns>
        /// <exception cref="AssimpException">Thrown if there was a general error in importing the model.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the file could not be located.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public ExportDataBlob ConvertFromFileToBlob(String inputFilename, String exportFormatId, PostProcessSteps exportProcessSteps) {
            return ConvertFromFileToBlob(inputFilename, PostProcessSteps.None, exportFormatId, exportProcessSteps);
        }

        /// <summary>
        /// Converts the model contained in the file to the specified format and save it to a data blob.
        /// </summary>
        /// <param name="inputFilename">Input file name to import</param>
        /// <param name="importProcessSteps">Post processing steps used for the import</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        /// <param name="exportProcessSteps">Pre processing steps used for the export</param>
        /// <returns>Data blob containing the exported scene in a binary form</returns>
        /// <exception cref="AssimpException">Thrown if there was a general error in importing the model.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the file could not be located.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public ExportDataBlob ConvertFromFileToBlob(String inputFilename, PostProcessSteps importProcessSteps, String exportFormatId, PostProcessSteps exportProcessSteps) {
            lock(s_sync) {
                if(m_isDisposed) {
                    throw new ObjectDisposedException("Importer has been disposed.");
                }

                if(String.IsNullOrEmpty(inputFilename) || !File.Exists(inputFilename)) {
                    throw new FileNotFoundException("Filename was null or could not be found", inputFilename);
                }

                IntPtr ptr = IntPtr.Zero;
                PrepareImport();

                try {
                    ptr = AssimpMethods.ImportFile(inputFilename, PostProcessSteps.None, m_propStore);

                    if(ptr == IntPtr.Zero)
                        throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());

                    TransformScene(ptr);
                    ptr = AssimpMethods.ApplyPostProcessing(ptr, importProcessSteps);

                    ValidateScene(ptr);

                    return AssimpMethods.ExportSceneToBlob(ptr, exportFormatId, exportProcessSteps);
                } finally {
                    CleanupImport();

                    if(ptr != IntPtr.Zero)
                        AssimpMethods.ReleaseImport(ptr);
                }
            }
        }

        #endregion

        #endregion

        #region ConvertFromStream

        #region Stream to File

        /// <summary>
        /// Converts the model contained in the stream to the specified format and save it to a file.
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <param name="importFormatHint">Format extension to serve as a hint to Assimp to choose which importer to use</param>
        /// <param name="outputFilename">Output file name to export to</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        /// <exception cref="AssimpException">Thrown if the stream is not valid (null or write-only) or if the format hint is null or empty.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public void ConvertFromStreamToFile(Stream inputStream, String importFormatHint, String outputFilename, String exportFormatId) {
            ConvertFromStreamToFile(inputStream, importFormatHint, PostProcessSteps.None, outputFilename, exportFormatId, PostProcessSteps.None);
        }

        /// <summary>
        /// Converts the model contained in the stream to the specified format and save it to a file.
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <param name="importFormatHint">Format extension to serve as a hint to Assimp to choose which importer to use</param>
        /// <param name="outputFilename">Output file name to export to</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        /// <param name="exportProcessSteps">Pre processing steps used for the export</param>
        /// <exception cref="AssimpException">Thrown if the stream is not valid (null or write-only) or if the format hint is null or empty.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public void ConvertFromStreamToFile(Stream inputStream, String importFormatHint, String outputFilename, String exportFormatId, PostProcessSteps exportProcessSteps) {
            ConvertFromStreamToFile(inputStream, importFormatHint, PostProcessSteps.None, outputFilename, exportFormatId, exportProcessSteps);
        }

        /// <summary>
        /// Converts the model contained in the stream to the specified format and save it to a file.
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <param name="importFormatHint">Format extension to serve as a hint to Assimp to choose which importer to use</param>
        /// <param name="importProcessSteps">Post processing steps used for import</param>
        /// <param name="outputFilename">Output file name to export to</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        /// <param name="exportProcessSteps">Pre processing steps used for the export</param>
        /// <exception cref="AssimpException">Thrown if the stream is not valid (null or write-only) or if the format hint is null or empty.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public void ConvertFromStreamToFile(Stream inputStream, String importFormatHint, PostProcessSteps importProcessSteps, String outputFilename, String exportFormatId, PostProcessSteps exportProcessSteps) {
            lock(s_sync) {
                if(m_isDisposed) {
                    throw new ObjectDisposedException("Importer has been disposed.");
                }

                if(inputStream == null || inputStream.CanRead != true) {
                    throw new AssimpException("stream", "Can't read from the stream it's null or write-only");
                }

                if(String.IsNullOrEmpty(importFormatHint)) {
                    throw new AssimpException("formatHint", "Format hint is null or empty");
                }

                IntPtr ptr = IntPtr.Zero;
                PrepareImport();

                try {
                    ptr = AssimpMethods.ImportFileFromStream(inputStream, importProcessSteps, importFormatHint, m_propStore);

                    if(ptr == IntPtr.Zero)
                        throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());

                    TransformScene(ptr);
                    ptr = AssimpMethods.ApplyPostProcessing(ptr, importProcessSteps);

                    ValidateScene(ptr);

                    AssimpMethods.ExportScene(ptr, exportFormatId, outputFilename, exportProcessSteps);
                } finally {
                    CleanupImport();

                    if(ptr != IntPtr.Zero)
                        AssimpMethods.ReleaseImport(ptr);
                }
            }
        }

        #endregion

        #region Stream to Blob

        /// <summary>
        /// Converts the model contained in the stream to the specified format and save it to a data blob.
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <param name="importFormatHint">Format extension to serve as a hint to Assimp to choose which importer to use</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        /// <returns>Data blob containing the exported scene in a binary form</returns>
        /// <exception cref="AssimpException">Thrown if the stream is not valid (null or write-only) or if the format hint is null or empty.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public ExportDataBlob ConvertFromStreamToBlob(Stream inputStream, String importFormatHint, String exportFormatId) {
            return ConvertFromStreamToBlob(inputStream, importFormatHint, PostProcessSteps.None, exportFormatId, PostProcessSteps.None);
        }

        /// <summary>
        /// Converts the model contained in the stream to the specified format and save it to a data blob.
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <param name="importFormatHint">Format extension to serve as a hint to Assimp to choose which importer to use</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        /// <param name="exportProcessSteps">Pre processing steps used for the export</param>
        /// <returns>Data blob containing the exported scene in a binary form</returns>
        /// <exception cref="AssimpException">Thrown if the stream is not valid (null or write-only) or if the format hint is null or empty.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public ExportDataBlob ConvertFromStreamToBlob(Stream inputStream, String importFormatHint, String exportFormatId, PostProcessSteps exportProcessSteps) {
            return ConvertFromStreamToBlob(inputStream, importFormatHint, PostProcessSteps.None, exportFormatId, exportProcessSteps);
        }

        /// <summary>
        /// Converts the model contained in the stream to the specified format and save it to a data blob.
        /// </summary>
        /// <param name="inputStream">Stream to read from</param>
        /// <param name="importFormatHint">Format extension to serve as a hint to Assimp to choose which importer to use</param>
        /// <param name="importProcessSteps">Post processing steps used for import</param>
        /// <param name="exportFormatId">Format id that specifies what format to export to</param>
        /// <param name="exportProcessSteps">Pre processing steps used for the export</param>
        /// <returns>Data blob containing the exported scene in a binary form</returns>
        /// <exception cref="AssimpException">Thrown if the stream is not valid (null or write-only) or if the format hint is null or empty.</exception>
        /// <exception cref="System.ObjectDisposedException">Thrown if attempting to import a model if the importer has been disposed of</exception>
        public ExportDataBlob ConvertFromStreamToBlob(Stream inputStream, String importFormatHint, PostProcessSteps importProcessSteps, String exportFormatId, PostProcessSteps exportProcessSteps) {
            lock(s_sync) {
                if(m_isDisposed) {
                    throw new ObjectDisposedException("Importer has been disposed.");
                }

                if(inputStream == null || inputStream.CanRead != true) {
                    throw new AssimpException("stream", "Can't read from the stream it's null or write-only");
                }

                if(String.IsNullOrEmpty(importFormatHint)) {
                    throw new AssimpException("formatHint", "Format hint is null or empty");
                }

                IntPtr ptr = IntPtr.Zero;
                PrepareImport();

                try {
                    ptr = AssimpMethods.ImportFileFromStream(inputStream, importProcessSteps, importFormatHint, m_propStore);

                    if(ptr == IntPtr.Zero)
                        throw new AssimpException("Error importing file: " + AssimpMethods.GetErrorString());

                    TransformScene(ptr);
                    ptr = AssimpMethods.ApplyPostProcessing(ptr, importProcessSteps);

                    ValidateScene(ptr);

                    return AssimpMethods.ExportSceneToBlob(ptr, exportFormatId, exportProcessSteps);
                } finally {
                    CleanupImport();

                    if(ptr != IntPtr.Zero)
                        AssimpMethods.ReleaseImport(ptr);
                }
            }
        }

        #endregion

        #endregion

        #region Logstreams

        /// <summary>
        /// Attaches a logging stream to the importer.
        /// </summary>
        /// <param name="logstream">Logstream to attach</param>
        public void AttachLogStream(LogStream logstream) {
            if(logstream == null || m_logStreams.Contains(logstream)) {
                return;
            }
            m_logStreams.Add(logstream);
        }

        /// <summary>
        /// Detaches a logging stream from the importer.
        /// </summary>
        /// <param name="logStream">Logstream to detatch</param>
        public void DetachLogStream(LogStream logStream) {
            if(logStream == null) {
                return;
            }
            m_logStreams.Remove(logStream);
        }

        /// <summary>
        /// Detaches all logging streams that are currently attached to the importer.
        /// </summary>
        public void DetachLogStreams() {
            m_logStreams.Clear();
        }

        #endregion

        #region Format support

        #region Format support

        /// <summary>
        /// Gets the model formats that are supported for export by Assimp.
        /// </summary>
        /// <returns>Export formats supported</returns>
        public ExportFormatDescription[] GetSupportedExportFormats() {
            if(m_exportFormats == null)
                m_exportFormats = AssimpMethods.GetExportFormatDescriptions();

            return (ExportFormatDescription[]) m_exportFormats.Clone();
        }

        /// <summary>
        /// Gets the model formats that are supported for import by Assimp.
        /// </summary>
        /// <returns>Import formats supported</returns>
        public String[] GetSupportedImportFormats() {
            if(m_importFormats == null)
                m_importFormats = AssimpMethods.GetExtensionList();

            return (String[]) m_importFormats.Clone();
        }

        /// <summary>
        /// Checks if the format extension (e.g. ".dae" or ".obj") is supported for import.
        /// </summary>
        /// <param name="format">Model format</param>
        /// <returns>True if the format is supported, false otherwise</returns>
        public bool IsImportFormatSupported(String format) {
            return AssimpMethods.IsExtensionSupported(format);
        }

        /// <summary>
        /// Checks if the format extension (e.g. ".dae" or ".obj") is supported for export.
        /// </summary>
        /// <param name="format">Model format</param>
        /// <returns>True if the format is supported, false otherwise</returns>
        public bool IsExportFormatSupported(String format) {
            if(String.IsNullOrEmpty(format))
                return false;

            ExportFormatDescription[] exportFormats = GetSupportedExportFormats();

            if(format.StartsWith(".") && format.Length >= 2)
                format = format.Substring(1);

            foreach(ExportFormatDescription desc in exportFormats) {
                if(String.Equals(desc.FileExtension, format))
                    return true;
            }

            return false;
        }

        #endregion

        #endregion

        #region Configs

        /// <summary>
        /// Sets a configuration property to the importer.
        /// </summary>
        /// <param name="config">Config to set</param>
        public void SetConfig(PropertyConfig config) {
            if(config == null) {
                return;
            }
            String name = config.Name;
            m_configs[config.Name] = config;
        }

        /// <summary>
        /// Removes a set configuration property by name.
        /// </summary>
        /// <param name="configName">Name of the config property</param>
        public void RemoveConfig(String configName) {
            if(String.IsNullOrEmpty(configName)) {
                return;
            }
            PropertyConfig oldConfig;
            if(m_configs.TryGetValue(configName, out oldConfig)) {
                m_configs.Remove(configName);
            }
        }

        /// <summary>
        /// Removes all configuration properties from the importer.
        /// </summary>
        public void RemoveConfigs() {
            m_configs.Clear();
        }

        /// <summary>
        /// Checks if the importer has a config set by the specified name.
        /// </summary>
        /// <param name="configName">Name of the config property</param>
        /// <returns>True if the config is present, false otherwise</returns>
        public bool ContainsConfig(String configName) {
            if(String.IsNullOrEmpty(configName)) {
                return false;
            }
            return m_configs.ContainsKey(configName);
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing) {

            if(!m_isDisposed) {
                if(disposing) {
                    //Dispose of managed resources
                }
                m_isDisposed = true;
            }
        }

        #endregion

        #region Private methods

        //Build import transformation matrix
        private void BuildMatrix() {

            if(m_buildMatrix) {
                Matrix4x4 scale = Matrix4x4.FromScaling(new Vector3D(m_scale, m_scale, m_scale));
                Matrix4x4 xRot = Matrix4x4.FromRotationX(m_xAxisRotation * (float) (180.0d / Math.PI));
                Matrix4x4 yRot = Matrix4x4.FromRotationY(m_yAxisRotation * (float) (180.0d / Math.PI));
                Matrix4x4 zRot = Matrix4x4.FromRotationZ(m_zAxisRotation * (float) (180.0d / Math.PI));
                m_scaleRot = scale * ((xRot * yRot) * zRot);
            }

            m_buildMatrix = false;
        }

        //Transforms the root node of the scene and writes it back to the native structure
        private unsafe bool TransformScene(IntPtr scene) {
            BuildMatrix();

            try {
                if(!m_scaleRot.IsIdentity) {
                    IntPtr rootNode = Marshal.ReadIntPtr(MemoryHelper.AddIntPtr(scene, sizeof(uint))); //Skip over sceneflags

                    IntPtr matrixPtr = MemoryHelper.AddIntPtr(rootNode, Marshal.SizeOf(typeof(AiString))); //Skip over AiString

                    Matrix4x4 matrix = MemoryHelper.MarshalStructure<Matrix4x4>(matrixPtr); //Get the root transform

                    matrix = matrix * m_scaleRot; //Transform

                    //Save back to unmanaged mem
                    int index = 0;
                    for(int i = 1; i <= 4; i++) {
                        for(int j = 1; j <= 4; j++) {
                            float value = matrix[i, j];
                            byte[] bytes = BitConverter.GetBytes(value);
                            foreach(byte b in bytes) {
                                Marshal.WriteByte(matrixPtr, index, b);
                                index++;
                            }
                        }
                    }
                    return true;
                }
            } catch(Exception) {

            }
            return false;
        }

        //Creates all property stores and sets their values
        private void CreateConfigs() {
            m_propStore = AssimpMethods.CreatePropertyStore();

            foreach(KeyValuePair<String, PropertyConfig> config in m_configs) {
                config.Value.ApplyValue(m_propStore);
            }
        }

        //Destroys all property stores
        private void ReleaseConfigs() {
            if(m_propStore != IntPtr.Zero)
                AssimpMethods.ReleasePropertyStore(m_propStore);
        }

        //Attachs all logstreams to Assimp
        private void AttachLogs() {
            foreach(LogStream log in m_logStreams) {
                log.Attach();
            }
        }

        //Detatches all logstreams from Assimp
        private void DetatachLogs() {
            foreach(LogStream log in m_logStreams) {
                log.Detach();
            }
        }

        //Does all the necessary prep work before we import
        private void PrepareImport() {
            AssimpMethods.EnableVerboseLogging(m_verboseEnabled);
            AttachLogs();
            CreateConfigs();
        }

        //Does all the necessary cleanup work after we import
        private void CleanupImport() {
            ReleaseConfigs();
            DetatachLogs();
        }

        //Validate the imported scene to ensure its complete and load the return scene
        private Scene ValidateAndCreateScene(IntPtr ptr) {
            AiScene scene = MemoryHelper.MarshalStructure<AiScene>(ptr);
            if((scene.Flags & SceneFlags.Incomplete) == SceneFlags.Incomplete) {
                throw new AssimpException("Error importing file: Imported scene is incomplete. " + AssimpMethods.GetErrorString());
            }

            return new Scene(scene);
        }

        //Validate the imported scene to ensure its complete and load the return scene
        private void ValidateScene(IntPtr ptr) {
            AiScene scene = MemoryHelper.MarshalStructure<AiScene>(ptr);
            if((scene.Flags & SceneFlags.Incomplete) == SceneFlags.Incomplete) {
                throw new AssimpException("Error importing file: Imported scene is incomplete. " + AssimpMethods.GetErrorString());
            }
        }

        #endregion

    }
}

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
using System.IO;
using System.Runtime.InteropServices;

using SharpDX;

namespace Assimp.Unmanaged {
    /// <summary>
    /// Static class containing the P/Invoke methods exposing the Assimp C-API.
    /// </summary>
    internal static class AssimpMethods {

        #region Native DLL Declarations

        private const String Assimp32DLL = "assimp_x86.dll";
        private const String Assimp64DLL = "assimp_x64.dll";

        #endregion

        #region Import Methods

        private static IntPtr assimpDllHandle;
        private static IntPtr AssimpDllHandle
        {
            get
            {
                if (assimpDllHandle == IntPtr.Zero)
                {
                    //assimpDllHandle = Utilities.LoadLibrary(Path.Combine(Path.GetDirectoryName(typeof(AssimpMethods).Assembly.Location), IntPtr.Size == 8 ? Assimp64DLL : Assimp32DLL));
                    assimpDllHandle = Utilities.LoadLibrary(IntPtr.Size == 8 ? Assimp64DLL : Assimp32DLL);
                }
                return assimpDllHandle;
            }
        }

        private static T DllImportFunction<T>(string name, ref T delegateField) where T : class
        {
            return delegateField ?? (delegateField = (T)(object)Marshal.GetDelegateForFunctionPointer(Utilities.GetProcAddress(AssimpDllHandle, name), typeof(T)));
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr aiImportFileExWithPropertiesDelegate([InAttribute()] [MarshalAs(UnmanagedType.LPStr)] String file, uint flag, IntPtr fileIO, IntPtr propStore);
        private static aiImportFileExWithPropertiesDelegate _aiImportFileExWithProperties;
        private static aiImportFileExWithPropertiesDelegate aiImportFileExWithProperties
        {
            get { return DllImportFunction("aiImportFileExWithProperties", ref _aiImportFileExWithProperties); }
        }

        /// <summary>
        /// Imports a file.
        /// </summary>
        /// <param name="file">Valid filename</param>
        /// <param name="flags">Post process flags specifying what steps are to be run after the import.</param>
        /// <param name="propStore">Property store containing config name-values, may be null.</param>
        /// <returns>Pointer to the unmanaged data structure.</returns>
        public static IntPtr ImportFile(String file, PostProcessSteps flags, IntPtr propStore) {
            return aiImportFileExWithProperties(file, (uint) flags, IntPtr.Zero, propStore);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr aiImportFileFromMemoryDelegate(byte[] buffer, uint bufferLength, uint flags, [InAttribute()] [MarshalAs(UnmanagedType.LPStr)] String formatHint);
        private static aiImportFileFromMemoryDelegate _aiImportFileFromMemory;
        private static aiImportFileFromMemoryDelegate aiImportFileFromMemory
        {
            get { return DllImportFunction("aiImportFileFromMemory", ref _aiImportFileFromMemory); }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr aiImportFileFromMemoryWithPropertiesDelegate(byte[] buffer, uint bufferLength, uint flags, [InAttribute()] [MarshalAs(UnmanagedType.LPStr)] String formatHint, IntPtr propStore);
        private static aiImportFileFromMemoryWithPropertiesDelegate _aiImportFileFromMemoryWithProperties;
        private static aiImportFileFromMemoryWithPropertiesDelegate aiImportFileFromMemoryWithProperties
        {
            get { return DllImportFunction("aiImportFileFromMemoryWithProperties", ref _aiImportFileFromMemoryWithProperties); }
        }

        /// <summary>
        /// Imports a scene from a stream. This uses the "aiImportFileFromMemory" function. The stream can be from anyplace,
        /// not just a memory stream. It is up to the caller to dispose of the stream.
        /// </summary>
        /// <param name="stream">Stream containing the scene data</param>
        /// <param name="flags">Post processing flags</param>
        /// <param name="formatHint">A hint to Assimp to decide which importer to use to process the data</param>
        /// <param name="propStore">Property store containing the config name-values, may be null.</param>
        /// <returns></returns>
        public static IntPtr ImportFileFromStream(Stream stream, PostProcessSteps flags, String formatHint, IntPtr propStore) {
            byte[] buffer = MemoryHelper.ReadStreamFully(stream, 0);

            return aiImportFileFromMemoryWithProperties(buffer, (uint) buffer.Length, (uint) flags, formatHint, propStore);
        }

        /// <summary>
        /// Releases the unmanaged scene data structure.
        /// </summary>
        /// <param name="scene">Pointer to the unmanaged scene data structure.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiReleaseImportDelegate(IntPtr scene);
        private static aiReleaseImportDelegate _aiReleaseImport;
        public static aiReleaseImportDelegate ReleaseImport
        {
            get { return DllImportFunction("aiReleaseImport", ref _aiReleaseImport); }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr aiApplyPostProcessingDelegate(IntPtr scene, uint Flags);
        private static aiApplyPostProcessingDelegate _aiApplyPostProcessing;
        private static aiApplyPostProcessingDelegate aiApplyPostProcessing
        {
            get { return DllImportFunction("aiApplyPostProcessing", ref _aiApplyPostProcessing); }
        }

        /// <summary>
        /// Applies a post-processing step on an already imported scene.
        /// </summary>
        /// <param name="scene">Pointer to the unmanaged scene data structure.</param>
        /// <param name="flags">Post processing steps to run.</param>
        /// <returns>Pointer to the unmanaged scene data structure.</returns>
        public static IntPtr ApplyPostProcessing(IntPtr scene, PostProcessSteps flags) {
            return aiApplyPostProcessing(scene, (uint) flags);
        }

        #endregion

        #region Export Methods

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr aiGetExportFormatCountDelegate();
        private static aiGetExportFormatCountDelegate _aiGetExportFormatCount;
        private static aiGetExportFormatCountDelegate aiGetExportFormatCount
        {
            get { return DllImportFunction("aiGetExportFormatCount", ref _aiGetExportFormatCount); }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr aiGetExportFormatDescriptionDelegate(IntPtr index);
        private static aiGetExportFormatDescriptionDelegate _aiGetExportFormatDescription;
        private static aiGetExportFormatDescriptionDelegate aiGetExportFormatDescription
        {
            get { return DllImportFunction("aiGetExportFormatDescription", ref _aiGetExportFormatDescription); }
        }

        /// <summary>
        /// Gets all supported export formats.
        /// </summary>
        /// <returns>Array of supported export formats.</returns>
        public static ExportFormatDescription[] GetExportFormatDescriptions() {
            int count = aiGetExportFormatCount().ToInt32();

            if(count == 0)
                return new ExportFormatDescription[0];

            ExportFormatDescription[] descriptions = new ExportFormatDescription[count];

            for(int i = 0; i < count; i++) {
                IntPtr formatDescPtr = aiGetExportFormatDescription(new IntPtr(i));
                if(formatDescPtr != null) {
                    AiExportFormatDesc desc = MemoryHelper.MarshalStructure<AiExportFormatDesc>(formatDescPtr);
                    descriptions[i] = new ExportFormatDescription(ref desc);
                }
            }

            return descriptions;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr aiExportSceneToBlobDelegate(IntPtr scene, [InAttribute()] [MarshalAs(UnmanagedType.LPStr)] String formatId, uint preProcessing);
        private static aiExportSceneToBlobDelegate _aiExportSceneToBlob;
        private static aiExportSceneToBlobDelegate aiExportSceneToBlob
        {
            get { return DllImportFunction("aiExportSceneToBlob", ref _aiExportSceneToBlob); }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void aiReleaseExportBlobDelegate(IntPtr blobData);
        private static aiReleaseExportBlobDelegate _aiReleaseExportBlob;
        private static aiReleaseExportBlobDelegate aiReleaseExportBlob
        {
            get { return DllImportFunction("aiReleaseExportBlob", ref _aiReleaseExportBlob); }
        }

        /// <summary>
        /// Exports the given scene to a chosen file format. Returns the exported data as a binary blob which you can embed into another data structure or file.
        /// </summary>
        /// <param name="scene">Scene to export, it is the responsibility of the caller to free this when finished.</param>
        /// <param name="formatId">Format id describing which format to export to.</param>
        /// <param name="preProcessing">Pre processing flags to operate on the scene during the export.</param>
        /// <returns>Exported binary blob, or null if there was an error.</returns>
        public static ExportDataBlob ExportSceneToBlob(IntPtr scene, String formatId, PostProcessSteps preProcessing) {
            if(String.IsNullOrEmpty(formatId) || scene == IntPtr.Zero)
                return null;

            IntPtr blobPtr = aiExportSceneToBlob(scene, formatId, (uint) preProcessing);

            if(blobPtr == IntPtr.Zero)
                return null;

            AiExportDataBlob blob = MemoryHelper.MarshalStructure<AiExportDataBlob>(blobPtr);
            ExportDataBlob dataBlob = new ExportDataBlob(ref blob);
            aiReleaseExportBlob(blobPtr);

            return dataBlob;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ReturnCode aiExportSceneDelegate(IntPtr scene, [InAttribute()] [MarshalAs(UnmanagedType.LPStr)] String formatId, [InAttribute()] [MarshalAs(UnmanagedType.LPStr)] String fileName, uint preProcessing);
        private static aiExportSceneDelegate _aiExportScene;
        private static aiExportSceneDelegate aiExportScene
        {
            get { return DllImportFunction("aiExportScene", ref _aiExportScene); }
        }

        /// <summary>
        /// Exports the given scene to a chosen file format and writes the result file(s) to disk.
        /// </summary>
        /// <param name="scene">The scene to export, which needs to be freed by the caller. The scene is expected to conform to Assimp's Importer output format. In short,
        /// this means the model data should use a right handed coordinate system, face winding should be counter clockwise, and the UV coordinate origin assumed to be upper left. If the input is different, specify the pre processing flags appropiately.</param>
        /// <param name="formatId">Format id describing which format to export to.</param>
        /// <param name="fileName">Output filename to write to</param>
        /// <param name="preProcessing">Pre processing flags - accepts any post processing step flag. In reality only a small subset are actually supported, e.g. to ensure the input
        /// conforms to the standard Assimp output format. Some may be redundant, such as triangulation, which some exporters may have to enforce due to the export format.</param>
        /// <returns>Return code specifying if the operation was a success.</returns>
        public static ReturnCode ExportScene(IntPtr scene, String formatId, String fileName, PostProcessSteps preProcessing) {
            if(String.IsNullOrEmpty(formatId) || scene == IntPtr.Zero)
                return ReturnCode.Failure;

            return aiExportScene(scene, formatId, fileName, (uint) preProcessing);
        }

        #endregion

        #region Logging Methods

        /// <summary>
        /// Attaches a log stream callback to catch Assimp messages.
        /// </summary>
        /// <param name="stream">Logstream to attach</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiAttachLogStreamDelegate(ref AiLogStream stream);
        private static aiAttachLogStreamDelegate _aiAttachLogStream;
        public static aiAttachLogStreamDelegate AttachLogStream
        {
            get { return DllImportFunction("aiAttachLogStream", ref _aiAttachLogStream); }
        }

        /// <summary>
        /// Enables verbose logging.
        /// </summary>
        /// <param name="enable">True if verbose logging is to be enabled or not.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiEnableVerboseLoggingDelegate([InAttribute()] [MarshalAs(UnmanagedType.Bool)] bool enable);
        private static aiEnableVerboseLoggingDelegate _aiEnableVerboseLogging;
        public static aiEnableVerboseLoggingDelegate EnableVerboseLogging
        {
            get { return DllImportFunction("aiEnableVerboseLogging", ref _aiEnableVerboseLogging); }
        }

        /// <summary>
        /// Detaches a logstream callback.
        /// </summary>
        /// <param name="stream">Logstream to detach</param>
        /// <returns>A return code signifying if the function was successful or not.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ReturnCode aiDetachLogStreamDelegate(ref AiLogStream stream);
        private static aiDetachLogStreamDelegate _aiDetachLogStream;
        public static aiDetachLogStreamDelegate DetachLogStream
        {
            get { return DllImportFunction("aiDetachLogStream", ref _aiDetachLogStream); }
        }

        /// <summary>
        /// Detaches all logstream callbacks currently attached to Assimp.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiDetachAllLogStreamsDelegate();
        private static aiDetachAllLogStreamsDelegate _aiDetachAllLogStreams;
        public static aiDetachAllLogStreamsDelegate DetachAllLogStreams
        {
            get { return DllImportFunction("aiDetachAllLogStreams", ref _aiDetachAllLogStreams); }
        }

        #endregion

        #region Error and Info methods

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr aiGetErrorStringDelegate();
        private static aiGetErrorStringDelegate _aiGetErrorString;
        private static aiGetErrorStringDelegate aiGetErrorString
        {
            get { return DllImportFunction("aiGetErrorString", ref _aiGetErrorString); }
        }

        /// <summary>
        /// Gets the last error logged in Assimp.
        /// </summary>
        /// <returns>The last error message logged.</returns>
        public static String GetErrorString() {
            return Marshal.PtrToStringAnsi(aiGetErrorString());
        }

        /// <summary>
        /// Checks whether the model format extension is supported by Assimp.
        /// </summary>
        /// <param name="extension">Model format extension, e.g. ".3ds"</param>
        /// <returns>True if the format is supported, false otherwise.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public delegate bool aiIsExtensionSupportedDelegate([InAttribute()] [MarshalAs(UnmanagedType.LPStr)] String extension);
        private static aiIsExtensionSupportedDelegate _aiIsExtensionSupported;
        public static aiIsExtensionSupportedDelegate IsExtensionSupported
        {
            get { return DllImportFunction("aiIsExtensionSupported", ref _aiIsExtensionSupported); }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void aiGetExtensionListDelegate(ref AiString extensionsOut);
        private static aiGetExtensionListDelegate _aiGetExtensionList;
        private static aiGetExtensionListDelegate aiGetExtensionList
        {
            get { return DllImportFunction("aiGetExtensionList", ref _aiGetExtensionList); }
        }

        /// <summary>
        /// Gets all the model format extensions that are currently supported by Assimp.
        /// </summary>
        /// <returns>Array of supported format extensions</returns>
        public static String[] GetExtensionList() {
            AiString aiString = new AiString();
            aiGetExtensionList(ref aiString);
            return aiString.GetString().Split(new String[] { "*", ";*" }, StringSplitOptions.RemoveEmptyEntries);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void aiGetMemoryRequirementsDelegate(IntPtr scene, ref AiMemoryInfo memoryInfo);
        private static aiGetMemoryRequirementsDelegate _aiGetMemoryRequirements;
        private static aiGetMemoryRequirementsDelegate aiGetMemoryRequirements
        {
            get { return DllImportFunction("aiGetMemoryRequirements", ref _aiGetMemoryRequirements); }
        }

        /// <summary>
        /// Gets the memory requirements of the scene.
        /// </summary>
        /// <param name="scene">Pointer to the unmanaged scene data structure.</param>
        /// <returns>The memory information about the scene.</returns>
        public static AiMemoryInfo GetMemoryRequirements(IntPtr scene) {
            AiMemoryInfo info = new AiMemoryInfo();
            if(scene != IntPtr.Zero) {
                aiGetMemoryRequirements(scene, ref info);
            }
            return info;
        }

        #endregion

        #region Import Properties setters

        /// <summary>
        /// Create an empty property store. Property stores are used to collect import settings.
        /// </summary>
        /// <returns>Pointer to property store</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr aiCreatePropertyStoreDelegate();
        private static aiCreatePropertyStoreDelegate _aiCreatePropertyStore;
        public static aiCreatePropertyStoreDelegate CreatePropertyStore
        {
            get { return DllImportFunction("aiCreatePropertyStore", ref _aiCreatePropertyStore); }
        }

        /// <summary>
        /// Deletes a property store.
        /// </summary>
        /// <param name="propertyStore">Pointer to property store</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiReleasePropertyStoreDelegate(IntPtr propertyStore);
        private static aiReleasePropertyStoreDelegate _aiReleasePropertyStore;
        public static aiReleasePropertyStoreDelegate ReleasePropertyStore
        {
            get { return DllImportFunction("aiReleasePropertyStore", ref _aiReleasePropertyStore); }
        }

        /// <summary>
        /// Sets an integer property value.
        /// </summary>
        /// <param name="propertyStore">Pointer to property store</param>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiSetImportPropertyIntegerDelegate(IntPtr propertyStore, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String name, int value);
        private static aiSetImportPropertyIntegerDelegate _aiSetImportPropertyInteger;
        public static aiSetImportPropertyIntegerDelegate SetImportPropertyInteger
        {
            get { return DllImportFunction("aiSetImportPropertyInteger", ref _aiSetImportPropertyInteger); }
        }

        /// <summary>
        /// Sets a float property value.
        /// </summary>
        /// <param name="propertyStore">Pointer to property store</param>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiSetImportPropertyFloatDelegate(IntPtr propertyStore, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String name, float value);
        private static aiSetImportPropertyFloatDelegate _aiSetImportPropertyFloat;
        public static aiSetImportPropertyFloatDelegate SetImportPropertyFloat
        {
            get { return DllImportFunction("aiSetImportPropertyFloat", ref _aiSetImportPropertyFloat); }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void aiSetImportPropertyStringDelegate(IntPtr propertyStore, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String name, ref AiString value);
        private static aiSetImportPropertyStringDelegate _aiSetImportPropertyString;
        private static aiSetImportPropertyStringDelegate aiSetImportPropertyString
        {
            get { return DllImportFunction("aiSetImportPropertyString", ref _aiSetImportPropertyString); }
        }

        /// <summary>
        /// Sets a string property value.
        /// </summary>
        /// <param name="propertyStore">Pointer to property store</param>
        /// <param name="name">Property name</param>
        /// <param name="value">Property value</param>
        public static void SetImportPropertyString(IntPtr propertyStore, String name, String value) {
            AiString str = new AiString();
            if(str.SetString(value))
                aiSetImportPropertyString(propertyStore, name, ref str);
        }

        #endregion

        #region Material getters

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ReturnCode aiGetMaterialPropertyDelegate(ref AiMaterial mat, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String key, uint texType, uint texIndex, out IntPtr propertyOut);
        private static aiGetMaterialPropertyDelegate _aiGetMaterialProperty;
        private static aiGetMaterialPropertyDelegate aiGetMaterialProperty
        {
            get { return DllImportFunction("aiGetMaterialProperty", ref _aiGetMaterialProperty); }
        }

        /// <summary>
        /// Retrieves a material property with the specific key from the material.
        /// </summary>
        /// <param name="mat">Material to retrieve the property from</param>
        /// <param name="key">Ai mat key (base) name to search for</param>
        /// <param name="texType">Texture Type semantic, always zero for non-texture properties</param>
        /// <param name="texIndex">Texture index, always zero for non-texture properties</param>
        /// <returns>The material property, if found.</returns>
        public static AiMaterialProperty GetMaterialProperty(ref AiMaterial mat, String key, TextureType texType, uint texIndex) {
            IntPtr ptr;
            ReturnCode code = aiGetMaterialProperty(ref mat, key, (uint)texType, texIndex, out ptr);
            AiMaterialProperty prop = new AiMaterialProperty();
            if(code == ReturnCode.Success && ptr != IntPtr.Zero) {
                prop = MemoryHelper.MarshalStructure<AiMaterialProperty>(ptr);
            }
            return prop;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ReturnCode aiGetMaterialFloatArrayDelegate(ref AiMaterial mat, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String key, uint texType, uint texIndex, IntPtr ptrOut, ref uint valueCount);
        private static aiGetMaterialFloatArrayDelegate _aiGetMaterialFloatArray;
        private static aiGetMaterialFloatArrayDelegate aiGetMaterialFloatArray
        {
            get { return DllImportFunction("aiGetMaterialFloatArray", ref _aiGetMaterialFloatArray); }
        }

        /// <summary>
        /// Retrieves an array of float values with the specific key from the material.
        /// </summary>
        /// <param name="mat">Material to retrieve the data from</param>
        /// <param name="key">Ai mat key (base) name to search for</param>
        /// <param name="texType">Texture Type semantic, always zero for non-texture properties</param>
        /// <param name="texIndex">Texture index, always zero for non-texture properties</param>
        /// <param name="floatCount">The maximum number of floats to read. This may not accurately describe the data returned, as it may not exist or be smaller. If this value is less than
        /// the available floats, then only the requested number is returned (e.g. 1 or 2 out of a 4 float array).</param>
        /// <returns>The float array, if it exists</returns>
        public static float[] GetMaterialFloatArray(ref AiMaterial mat, String key, TextureType texType, uint texIndex, uint floatCount) {
            IntPtr ptr = IntPtr.Zero;
            try {
                ptr = Marshal.AllocHGlobal(IntPtr.Size);
                ReturnCode code = aiGetMaterialFloatArray(ref mat, key, (uint) texType, texIndex, ptr, ref floatCount);
                float[] array = null;
                if(code == ReturnCode.Success && floatCount > 0) {
                    array = new float[floatCount];
                    Marshal.Copy(ptr, array, 0, (int) floatCount);
                }
                return array;
            } finally {
                if(ptr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ReturnCode aiGetMaterialIntegerArrayDelegate(ref AiMaterial mat, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String key, uint texType, uint texIndex, IntPtr ptrOut, ref uint valueCount);
        private static aiGetMaterialIntegerArrayDelegate _aiGetMaterialIntegerArray;
        private static aiGetMaterialIntegerArrayDelegate aiGetMaterialIntegerArray
        {
            get { return DllImportFunction("aiGetMaterialIntegerArray", ref _aiGetMaterialIntegerArray); }
        }

        /// <summary>
        /// Retrieves an array of integer values with the specific key from the material.
        /// </summary>
        /// <param name="mat">Material to retrieve the data from</param>
        /// <param name="key">Ai mat key (base) name to search for</param>
        /// <param name="texType">Texture Type semantic, always zero for non-texture properties</param>
        /// <param name="texIndex">Texture index, always zero for non-texture properties</param>
        /// <param name="intCount">The maximum number of integers to read. This may not accurately describe the data returned, as it may not exist or be smaller. If this value is less than
        /// the available integers, then only the requested number is returned (e.g. 1 or 2 out of a 4 float array).</param>
        /// <returns>The integer array, if it exists</returns>
        public static int[] GetMaterialIntegerArray(ref AiMaterial mat, String key, TextureType texType, uint texIndex, uint intCount) {
            IntPtr ptr = IntPtr.Zero;
            try {
                ptr = Marshal.AllocHGlobal(IntPtr.Size);
                ReturnCode code = aiGetMaterialIntegerArray(ref mat, key, (uint) texType, texIndex, ptr, ref intCount);
                int[] array = null;
                if(code == ReturnCode.Success && intCount > 0) {
                    array = new int[intCount];
                    Marshal.Copy(ptr, array, 0, (int) intCount);
                }
                return array;
            } finally {
                if(ptr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ReturnCode aiGetMaterialColorDelegate(ref AiMaterial mat, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String key, uint texType, uint texIndex, IntPtr colorOut);
        private static aiGetMaterialColorDelegate _aiGetMaterialColor;
        private static aiGetMaterialColorDelegate aiGetMaterialColor
        {
            get { return DllImportFunction("aiGetMaterialColor", ref _aiGetMaterialColor); }
        }

        /// <summary>
        /// Retrieves a color value from the material property table.
        /// </summary>
        /// <param name="mat">Material to retrieve the data from</param>
        /// <param name="key">Ai mat key (base) name to search for</param>
        /// <param name="texType">Texture Type semantic, always zero for non-texture properties</param>
        /// <param name="texIndex">Texture index, always zero for non-texture properties</param>
        /// <returns>The color if it exists. If not, the default Color4D value is returned.</returns>
        public static Color4D GetMaterialColor(ref AiMaterial mat, String key, TextureType texType, uint texIndex) {
            IntPtr ptr = IntPtr.Zero;
            try {
                ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Color4D)));
                aiGetMaterialColor(ref mat, key, (uint) texType, texIndex, ptr);
                Color4D color = new Color4D();
                if(ptr != IntPtr.Zero) {
                    color = MemoryHelper.MarshalStructure<Color4D>(ptr);
                }
                return color;
            } finally {
                if(ptr != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ReturnCode aiGetMaterialStringDelegate(ref AiMaterial mat, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] String key, uint texType, uint texIndex, out AiString str);
        private static aiGetMaterialStringDelegate _aiGetMaterialString;
        private static aiGetMaterialStringDelegate aiGetMaterialString
        {
            get { return DllImportFunction("aiGetMaterialString", ref _aiGetMaterialString); }
        }

        /// <summary>
        /// Retrieves a string from the material property table.
        /// </summary>
        /// <param name="mat">Material to retrieve the data from</param>
        /// <param name="key">Ai mat key (base) name to search for</param>
        /// <param name="texType">Texture Type semantic, always zero for non-texture properties</param>
        /// <param name="texIndex">Texture index, always zero for non-texture properties</param>
        /// <returns>The string, if it exists. If not, an empty string is returned.</returns>
        public static String GetMaterialString(ref AiMaterial mat, String key, TextureType texType, uint texIndex) {
            AiString str;
            ReturnCode code = aiGetMaterialString(ref mat, key, (uint) texType, texIndex, out str);
            if(code == ReturnCode.Success) {
                return str.GetString();
            }
            return String.Empty;
        }

        /// <summary>
        /// Gets the number of textures contained in the material for a particular texture type.
        /// </summary>
        /// <param name="mat">Material to retrieve the data from</param>
        /// <param name="type">Texture Type semantic</param>
        /// <returns>The number of textures for the type.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint aiGetMaterialTextureCountDelegate(ref AiMaterial mat, TextureType type);
        private static aiGetMaterialTextureCountDelegate _aiGetMaterialTextureCount;
        public static aiGetMaterialTextureCountDelegate GetMaterialTextureCount
        {
            get { return DllImportFunction("aiGetMaterialTextureCount", ref _aiGetMaterialTextureCount); }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ReturnCode aiGetMaterialTextureDelegate(ref AiMaterial mat, TextureType type, uint index, out AiString path, out TextureMapping mapping, out uint uvIndex, out float blendFactor, out TextureOperation textureOp, out TextureWrapMode wrapMode, out uint flags);
        private static aiGetMaterialTextureDelegate _aiGetMaterialTexture;
        private static aiGetMaterialTextureDelegate aiGetMaterialTexture
        {
            get { return DllImportFunction("aiGetMaterialTexture", ref _aiGetMaterialTexture); }
        }

        /// <summary>
        /// Gets the texture filepath contained in the material.
        /// </summary>
        /// <param name="mat">Material to retrieve the data from</param>
        /// <param name="type">Texture type semantic</param>
        /// <param name="index">Texture index</param>
        /// <returns>The texture filepath, if it exists. If not an empty string is returned.</returns>
        public static String GetMaterialTextureFilePath(ref AiMaterial mat, TextureType type, uint index) {
            AiString str;
            TextureMapping mapping;
            uint uvIndex;
            float blendFactor;
            TextureOperation texOp;
            TextureWrapMode wrapMode;
            uint flags;
            ReturnCode code = aiGetMaterialTexture(ref mat, type, index, out str, out mapping, out uvIndex, out blendFactor, out texOp, out wrapMode, out flags);
            if(code == ReturnCode.Success) {
                return str.GetString();
            }
            return String.Empty;
        }

        /// <summary>
        /// Gets all values pertaining to a particular texture from a material.
        /// </summary>
        /// <param name="mat">Material to retrieve the data from</param>
        /// <param name="type">Texture type semantic</param>
        /// <param name="index">Texture index</param>
        /// <returns>Returns the texture slot struct containing all the information.</returns>
        public static TextureSlot GetMaterialTexture(ref AiMaterial mat, TextureType type, uint index) {
            AiString str;
            TextureMapping mapping;
            uint uvIndex;
            float blendFactor;
            TextureOperation texOp;
            TextureWrapMode wrapMode;
            uint flags;
            ReturnCode code = aiGetMaterialTexture(ref mat, type, index, out str, out mapping, out uvIndex, out blendFactor, out texOp, out wrapMode, out flags);
            return new TextureSlot(str.GetString(), type, index, mapping, uvIndex, blendFactor, texOp, wrapMode, flags);
        }
        
        #endregion

        #region Math methods

        /// <summary>
        /// Creates a quaternion from the 3x3 rotation matrix.
        /// </summary>
        /// <param name="quat">Quaternion struct to fill</param>
        /// <param name="mat">Rotation matrix</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiCreateQuaternionFromMatrixDelegate(out Quaternion quat, ref Matrix3x3 mat);
        private static aiCreateQuaternionFromMatrixDelegate _aiCreateQuaternionFromMatrix;
        public static aiCreateQuaternionFromMatrixDelegate CreateQuaternionFromMatrix
        {
            get { return DllImportFunction("aiCreateQuaternionFromMatrix", ref _aiCreateQuaternionFromMatrix); }
        }

        /// <summary>
        /// Decomposes a 4x4 matrix into its scaling, rotation, and translation parts.
        /// </summary>
        /// <param name="mat">4x4 Matrix to decompose</param>
        /// <param name="scaling">Scaling vector</param>
        /// <param name="rotation">Quaternion containing the rotation</param>
        /// <param name="position">Translation vector</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiDecomposeMatrixDelegate(ref Matrix4x4 mat, out Vector3D scaling, out Quaternion rotation, out Vector3D position);
        private static aiDecomposeMatrixDelegate _aiDecomposeMatrix;
        public static aiDecomposeMatrixDelegate DecomposeMatrix
        {
            get { return DllImportFunction("aiDecomposeMatrix", ref _aiDecomposeMatrix); }
        }

        /// <summary>
        /// Transposes the 4x4 matrix.
        /// </summary>
        /// <param name="mat">Matrix to transpose</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiTransposeMatrix4Delegate(ref Matrix4x4 mat);
        private static aiTransposeMatrix4Delegate _aiTransposeMatrix4;
        public static aiTransposeMatrix4Delegate TransposeMatrix4
        {
            get { return DllImportFunction("aiTransposeMatrix4", ref _aiTransposeMatrix4); }
        }

        /// <summary>
        /// Transposes the 3x3 matrix.
        /// </summary>
        /// <param name="mat">Matrix to transpose</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiTransposeMatrix3Delegate(ref Matrix3x3 mat);
        private static aiTransposeMatrix3Delegate _aiTransposeMatrix3;
        public static aiTransposeMatrix3Delegate TransposeMatrix3
        {
            get { return DllImportFunction("aiTransposeMatrix3", ref _aiTransposeMatrix3); }
        }

        /// <summary>
        /// Transforms the vector by the 3x3 rotation matrix.
        /// </summary>
        /// <param name="vec">Vector to transform</param>
        /// <param name="mat">Rotation matrix</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiTransformVecByMatrix3Delegate(ref Vector3D vec, ref Matrix3x3 mat);
        private static aiTransformVecByMatrix3Delegate _aiTransformVecByMatrix3;
        public static aiTransformVecByMatrix3Delegate TransformVecByMatrix3
        {
            get { return DllImportFunction("aiTransformVecByMatrix3", ref _aiTransformVecByMatrix3); }
        }

        /// <summary>
        /// Transforms the vector by the 4x4 matrix.
        /// </summary>
        /// <param name="vec">Vector to transform</param>
        /// <param name="mat">Matrix transformation</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiTransformVecByMatrix4Delegate(ref Vector3D vec, ref Matrix4x4 mat);
        private static aiTransformVecByMatrix4Delegate _aiTransformVecByMatrix4;
        public static aiTransformVecByMatrix4Delegate TransformVecByMatrix4
        {
            get { return DllImportFunction("aiTransformVecByMatrix4", ref _aiTransformVecByMatrix4); }
        }

        /// <summary>
        /// Multiplies two 4x4 matrices. The destination matrix receives the result.
        /// </summary>
        /// <param name="dst">First input matrix and is also the Matrix to receive the result</param>
        /// <param name="src">Second input matrix, to be multiplied with "dst".</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiMultiplyMatrix4Delegate(ref Matrix4x4 dst, ref Matrix4x4 src);
        private static aiMultiplyMatrix4Delegate _aiMultiplyMatrix4;
        public static aiMultiplyMatrix4Delegate MultiplyMatrix4
        {
            get { return DllImportFunction("aiMultiplyMatrix4", ref _aiMultiplyMatrix4); }
        }

        /// <summary>
        /// Multiplies two 3x3 matrices. The destination matrix receives the result.
        /// </summary>
        /// <param name="dst">First input matrix and is also the Matrix to receive the result</param>
        /// <param name="src">Second input matrix, to be multiplied with "dst".</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiMultiplyMatrix3Delegate(ref Matrix3x3 dst, ref Matrix3x3 src);
        private static aiMultiplyMatrix3Delegate _aiMultiplyMatrix3;
        public static aiMultiplyMatrix3Delegate MultiplyMatrix3
        {
            get { return DllImportFunction("aiMultiplyMatrix3", ref _aiMultiplyMatrix3); }
        }

        /// <summary>
        /// Creates a 3x3 identity matrix.
        /// </summary>
        /// <param name="mat">Matrix to hold the identity</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiIdentityMatrix3Delegate(out Matrix3x3 mat);
        private static aiIdentityMatrix3Delegate _aiIdentityMatrix3;
        public static aiIdentityMatrix3Delegate IdentityMatrix3
        {
            get { return DllImportFunction("aiIdentityMatrix3", ref _aiIdentityMatrix3); }
        }

        /// <summary>
        /// Creates a 4x4 identity matrix.
        /// </summary>
        /// <param name="mat">Matrix to hold the identity</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void aiIdentityMatrix4Delegate(out Matrix4x4 mat);
        private static aiIdentityMatrix4Delegate _aiIdentityMatrix4;
        public static aiIdentityMatrix4Delegate IdentityMatrix4
        {
            get { return DllImportFunction("aiIdentityMatrix4", ref _aiIdentityMatrix4); }
        }

        #endregion

        #region Version info

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr aiGetLegalStringDelegate();
        private static aiGetLegalStringDelegate _aiGetLegalString;
        private static aiGetLegalStringDelegate aiGetLegalString
        {
            get { return DllImportFunction("aiGetLegalString", ref _aiGetLegalString); }
        }

        /// <summary>
        /// Gets the Assimp legal info.
        /// </summary>
        /// <returns>String containing Assimp legal info.</returns>
        public static String GetLegalString() {
            return Marshal.PtrToStringAnsi(aiGetLegalString());
        }

        /// <summary>
        /// Gets the native Assimp DLL's minor version number.
        /// </summary>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint aiGetVersionMinorDelegate();
        private static aiGetVersionMinorDelegate _aiGetVersionMinor;
        public static aiGetVersionMinorDelegate GetVersionMinor
        {
            get { return DllImportFunction("aiGetVersionMinor", ref _aiGetVersionMinor); }
        }

        /// <summary>
        /// Gets the native Assimp DLL's major version number.
        /// </summary>
        /// <returns>Assimp major version number</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint aiGetVersionMajorDelegate();
        private static aiGetVersionMajorDelegate _aiGetVersionMajor;
        public static aiGetVersionMajorDelegate GetVersionMajor
        {
            get { return DllImportFunction("aiGetVersionMajor", ref _aiGetVersionMajor); }
        }

        /// <summary>
        /// Gets the native Assimp DLL's revision version number.
        /// </summary>
        /// <returns>Assimp revision version number</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint aiGetVersionRevisionDelegate();
        private static aiGetVersionRevisionDelegate _aiGetVersionRevision;
        public static aiGetVersionRevisionDelegate GetVersionRevision
        {
            get { return DllImportFunction("aiGetVersionRevision", ref _aiGetVersionRevision); }
        }

        /// <summary>
        /// Gets the native Assimp DLL's current version number as "major.minor.revision" string. This is the
        /// version of Assimp that this wrapper is currently using.
        /// </summary>
        /// <returns></returns>
        public static String GetVersion() {
            uint major = GetVersionMajor();
            uint minor = GetVersionMinor();
            uint rev = GetVersionRevision();

            return String.Format("{0}.{1}.{2}", major.ToString(), minor.ToString(), rev.ToString());
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint aiGetCompileFlagsDelegate();
        private static aiGetCompileFlagsDelegate _aiGetCompileFlags;
        private static aiGetCompileFlagsDelegate aiGetCompileFlags
        {
            get { return DllImportFunction("aiGetCompileFlags", ref _aiGetCompileFlags); }
        }

        /// <summary>
        /// Get the compilation flags that describe how the native Assimp DLL was compiled.
        /// </summary>
        /// <returns>Compilation flags</returns>
        public static CompileFlags GetCompileFlags() {
            return (CompileFlags) aiGetCompileFlags();
        }

        #endregion
    }
}

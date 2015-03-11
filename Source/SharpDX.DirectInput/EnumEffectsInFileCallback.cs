using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDX.DirectInput
{
    /// <summary>
    /// Enumerator callback for DirectInput EnumEffectsInFile.
    /// </summary>
    internal class EnumEffectsInFileCallback
    {
        private readonly IntPtr _nativePointer;
        private readonly DirectInputEnumEffectsInFileDelegate _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumEffectsInFileCallback"/> class.
        /// </summary>
        public EnumEffectsInFileCallback()
        {
            unsafe
            {
                _callback = new DirectInputEnumEffectsInFileDelegate(DirectInputEnumEffectsInFileImpl);
                _nativePointer = Marshal.GetFunctionPointerForDelegate(_callback);
                EffectsInFile = new List<EffectFile>();
            }
        }

        /// <summary>
        /// Natives the pointer.
        /// </summary>
        /// <returns></returns>
        public IntPtr NativePointer
        {
            get { return _nativePointer; }
        }

        /// <summary>
        /// Gets or sets the effects in file.
        /// </summary>
        /// <value>The effects in file.</value>
        public List<EffectFile> EffectsInFile { get; private set; }

        // BOOL DIEnumEffectsInFileCallback(LPCDIEffectInfo pdei,LPVOID pvRef)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate int DirectInputEnumEffectsInFileDelegate(void* deviceInstance, IntPtr data);
        private unsafe int DirectInputEnumEffectsInFileImpl(void* deviceInstance, IntPtr data)
        {
            var newEffect = new EffectFile();
            newEffect.__MarshalFrom(ref *((EffectFile.__Native*)deviceInstance));
            EffectsInFile.Add(newEffect);
            // Return true to continue iterating
            return 1;
        }
    }
}
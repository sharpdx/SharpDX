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

namespace SharpDX.DirectWrite
{
    public partial class Factory
    {
        private readonly List<FontCollectionLoaderCallback> _fontCollectionLoaderCallbacks = new List<FontCollectionLoaderCallback>();
        private readonly List<FontFileLoaderCallback> _fontFileLoaderCallbacks = new List<FontFileLoaderCallback>();

        internal FontCollectionLoaderCallback FindRegisteredFontCollectionLoaderCallback(FontCollectionLoader loader)
        {
            foreach (var fontCollectionLoaderCallback in _fontCollectionLoaderCallbacks)
            {
                if (fontCollectionLoaderCallback.Callback == loader)
                    return fontCollectionLoaderCallback;
            }
            return null;
        }

        internal FontFileLoaderCallback FindRegisteredFontFileLoaderCallback(FontFileLoader loader)
        {
            foreach (var fontFileLoaderCallback in _fontFileLoaderCallbacks)
            {
                if (fontFileLoaderCallback.Callback == loader)
                    return fontFileLoaderCallback;
            }
            return null;
        }

        /// <summary>
        /// Default Constructor for a <see cref = "SharpDX.Direct2D1.Factory" />.
        /// </summary>
        public Factory()
            : this(FactoryType.Shared)
        {
        }

        /// <summary>
        /// Default Constructor for a <see cref = "SharpDX.Direct2D1.Factory" />.
        /// </summary>
        public Factory(FactoryType factoryType)
            : base(IntPtr.Zero)
        {
            ComObject temp;
            DWrite.CreateFactory(factoryType, typeof(Factory).GUID, out temp);
            NativePointer = temp.NativePointer;
        }

        /// <summary>	
        /// Registers a custom font collection loader with the factory object. 	
        /// </summary>	
        /// <remarks>	
        /// This function registers a font collection loader with DirectWrite. The font collection loader interface, which should be implemented by a singleton object, handles enumerating font files in a font collection given a particular type of key. A given instance can only be registered once. Succeeding attempts will return an error, indicating that it has already been registered. Note that font file loader implementations must not register themselves with DirectWrite inside their constructors, and must not unregister themselves inside their destructors, because registration and unregistraton operations increment and decrement the object reference count respectively. Instead, registration and unregistration with DirectWrite of font file loaders should be performed outside of the font file loader implementation. 	
        /// </remarks>	
        /// <param name="fontCollectionLoader">Reference to a <see cref="SharpDX.DirectWrite.FontCollectionLoader"/> object to be registered. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteFactory::RegisterFontCollectionLoader([None] IDWriteFontCollectionLoader* fontCollectionLoader)</unmanaged>
        public Result RegisterFontCollectionLoader(FontCollectionLoader fontCollectionLoader)
        {
            var callback = new FontCollectionLoaderCallback(this, fontCollectionLoader);
            var result = this.RegisterFontCollectionLoader_(callback.NativePointer);
            _fontCollectionLoaderCallbacks.Add(callback);
            return result;
        }

        /// <summary>	
        /// Unregisters a custom font collection loader that was previously registered using {{RegisterFontCollectionLoader}}. 	
        /// </summary>	
        /// <param name="fontCollectionLoader">Pointer to a <see cref="SharpDX.DirectWrite.FontCollectionLoader"/> object to be unregistered. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteFactory::UnregisterFontCollectionLoader([None] IDWriteFontCollectionLoader* fontCollectionLoader)</unmanaged>
        public SharpDX.Result UnregisterFontCollectionLoader(FontCollectionLoader fontCollectionLoader)
        {
            FontCollectionLoaderCallback callback = null;
            int indexToRemove = 0;
            for (int i = _fontCollectionLoaderCallbacks.Count - 1; i >= 0; i--)
            {
                if (_fontCollectionLoaderCallbacks[i].Callback == fontCollectionLoader)
                {
                    callback = _fontCollectionLoaderCallbacks[i];
                    indexToRemove = i;
                    break;
                }
            }
            if (callback == null) throw new ArgumentException("This font collection loader is not registered", "fontCollectionLoader");

            var result = UnregisterFontCollectionLoader_(callback.NativePointer);

            _fontCollectionLoaderCallbacks.RemoveAt(indexToRemove);
            return result;
        }


        /// <summary>	
        /// Registers a font file loader with DirectWrite. 	
        /// </summary>	
        /// <remarks>	
        /// This function registers a font file loader with DirectWrite. The font file loader interface, which should be implemented   by a singleton object, handles loading font file resources of a particular type from a key. A given instance can only be registered once. Succeeding attempts will return an error, indicating that it has already been registered. Note that font file loader implementations must not register themselves with DirectWrite inside their constructors, and must not unregister themselves inside their destructors, because registration and unregistraton operations increment and decrement the object reference count respectively. Instead, registration and unregistration with DirectWrite of font file loaders should be performed outside of the font file loader implementation.  	
        /// </remarks>	
        /// <param name="fontFileLoader">Pointer to a <see cref="SharpDX.DirectWrite.FontFileLoader"/> object for a particular file resource type. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteFactory::RegisterFontFileLoader([None] IDWriteFontFileLoader* fontFileLoader)</unmanaged>
        public SharpDX.Result RegisterFontFileLoader(FontFileLoader fontFileLoader)
        {
            var callback = new FontFileLoaderCallback(fontFileLoader);
            var result = this.RegisterFontFileLoader_(callback.NativePointer);
            _fontFileLoaderCallbacks.Add(callback);
            return result;
        }

        /// <summary>	
        /// Unregisters a font file loader that was previously registered with the DirectWrite font system using {{RegisterFontFileLoader}}. 	
        /// </summary>	
        /// <remarks>	
        /// This function unregisters font file loader callbacks with the DirectWrite font system. You should implement the font file loader interface by a singleton object. Note that font file loader implementations must not register themselves with DirectWrite inside their constructors and must not unregister themselves in their destructors, because registration and unregistraton operations increment and decrement the object reference count respectively. Instead, registration and unregistration of font file loaders with DirectWrite should be performed outside of the font file loader implementation.  	
        /// </remarks>	
        /// <param name="fontFileLoader">Pointer to the file loader that was previously registered with the DirectWrite font system using {{RegisterFontFileLoader}}. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteFactory::UnregisterFontFileLoader([None] IDWriteFontFileLoader* fontFileLoader)</unmanaged>
        public SharpDX.Result UnregisterFontFileLoader(FontFileLoader fontFileLoader)
        {
            FontFileLoaderCallback callback = null;
            int indexToRemove = 0;
            for (int i = _fontFileLoaderCallbacks.Count - 1; i >= 0; i--)
            {
                if (_fontFileLoaderCallbacks[i].Callback == fontFileLoader)
                {
                    callback = _fontFileLoaderCallbacks[i];
                    indexToRemove = i;
                    break;
                }
            }
            if (callback == null) throw new ArgumentException("This font file loader is not registered", "FontFileLoader");

            var result = UnregisterFontFileLoader_(callback.NativePointer);

            _fontFileLoaderCallbacks.RemoveAt(indexToRemove);
            return result;
        }

    }
}

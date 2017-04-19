// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D9
{
    /// <summary>
    /// EffectHandle used to identify a shader parameter.
    /// </summary>
    public class EffectHandle : DisposeBase
    {
        #region Constants and Fields

        /// <summary>
        /// Defines the behavior for caching strings. True by default.
        /// </summary>
        private const bool UseCacheStrings = true;

        /// <summary>
        /// Cache of allocated strings.
        /// </summary>
        private static readonly Dictionary<string, IntPtr> AllocatedStrings = new Dictionary<string, IntPtr>();

        /// <summary>
        /// Pointer to the handle or the allocated string.
        /// </summary>
        private IntPtr pointer;

        /// <summary>
        /// If the <see cref="pointer"/> is a custom string not cached that needs to be released by this instance.
        /// </summary>
        private bool isStringToRelease;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectHandle"/> class.
        /// </summary>
        public EffectHandle()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectHandle"/> class.
        /// </summary>
        /// <param name="pointer">
        /// The pointer.
        /// </param>
        public EffectHandle(IntPtr pointer)
        {
            this.pointer = pointer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectHandle"/> class.
        /// </summary>
        /// <param name="pointer">
        /// The pointer.
        /// </param>
        public unsafe EffectHandle(void* pointer)
        {
            this.pointer = new IntPtr(pointer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectHandle"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public EffectHandle(string name)
        {
            pointer = AllocateString(name);
            isStringToRelease = !UseCacheStrings;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears the cache.
        /// </summary>
        /// <remarks>
        /// By default, this class is caching all strings that are implicitly used as an effect handle.
        /// Use this method in order to deallocate all strings that were previously cached.
        /// </remarks>
        public static void ClearCache()
        {
            lock (AllocatedStrings)
            {
                foreach (var value in AllocatedStrings.Values)
                    Marshal.FreeHGlobal(value);
                AllocatedStrings.Clear();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// marshal free.
        /// </summary>
        /// <param name="__from">The __from.</param>
        /// <param name="ref">The @ref.</param>
        internal static void __MarshalFree(ref EffectHandle __from, ref __Native @ref)
        {
        }

        /// <summary>
        /// Method to marshal from native to managed struct
        /// </summary>
        /// <param name="__from">The __from.</param>
        /// <param name="ref">The @ref.</param>
        internal static void __MarshalFrom(ref EffectHandle __from, ref __Native @ref)
        {
            if (@ref.Pointer == IntPtr.Zero)
                __from = null;
            else
                __from.pointer = @ref.Pointer;
        }

        /// <summary>
        /// Method to marshal from managed struct tot native
        /// </summary>
        /// <param name="__from">The __from.</param>
        /// <param name="ref">The @ref.</param>
        internal static void __MarshalTo(ref EffectHandle __from, ref __Native @ref)
        {
            if (__from == null)
                @ref.Pointer = IntPtr.Zero;
            else
                @ref.Pointer = __from.pointer;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (isStringToRelease)
            {
                Marshal.FreeHGlobal(pointer);
                pointer = IntPtr.Zero;
                isStringToRelease = false;
            }
        }

        /// <summary>
        /// Allocates a string.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// Pointer to the allocated string
        /// </returns>
        private static IntPtr AllocateString(string name)
        {
            IntPtr value;
            if (UseCacheStrings)
            {
                lock (AllocatedStrings)
                {
                    if (!AllocatedStrings.TryGetValue(name, out value))
                    {
                        value = Marshal.StringToHGlobalAnsi(name);
                        AllocatedStrings.Add(name, value);
                    }
                }
            }
            else
            {
                value = Marshal.StringToHGlobalAnsi(name);
            }

            return value;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.Direct3D9.EffectHandle"/> to <see cref="System.IntPtr"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator IntPtr(EffectHandle value)
        {
            return value.pointer;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.IntPtr"/> to <see cref="SharpDX.Direct3D9.EffectHandle"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator EffectHandle(IntPtr value)
        {
            return new EffectHandle(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.Direct3D9.EffectHandle"/> to raw pointer"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static unsafe implicit operator void*(EffectHandle value)
        {
            return (void*)value.pointer;
        }

        /// <summary>
        /// Performs an implicit conversion from raw pointer to <see cref="SharpDX.Direct3D9.EffectHandle"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static unsafe implicit operator EffectHandle(void* value)
        {
            return new EffectHandle(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="SharpDX.Direct3D9.EffectHandle"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator EffectHandle(string name)
        {
            return new EffectHandle(name);
        }

        #endregion

        [StructLayout(LayoutKind.Sequential)]
        internal struct __Native
        {
            public IntPtr Pointer;

            internal void __MarshalFree()
            {
            }

            public static implicit operator IntPtr(__Native value)
            {
                return value.Pointer;
            }

            public static implicit operator __Native(IntPtr value)
            {
                return new __Native() { Pointer = value };
            }

            public static unsafe implicit operator void*(__Native value)
            {
                return (void*)value.Pointer;
            }

            public static unsafe implicit operator __Native(void* value)
            {
                return new __Native() { Pointer = (IntPtr)value };
            }
        }
    }
}
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System.Reflection;
using System.Runtime.InteropServices;
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    internal partial class PropertyBinding
    {
        private NativeGetSet nativeGetSet;

        protected PropertyBinding()
        {
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public string TypeName
        {
            get
            {
                return PropertyTypeHelper.ConvertToString(Attribute.Type);
            }
        }

        public PropertyBindingAttribute Attribute { get; private set; }

        public static PropertyBinding Get(Type customEffectType, PropertyInfo propertyInfo)
        {
            var bindingAttr = Utilities.GetCustomAttribute<PropertyBindingAttribute>(propertyInfo, true);
            if (bindingAttr == null)
                return null;

            var binding = new PropertyBinding {Attribute = bindingAttr};
            var propType = propertyInfo.PropertyType;
            var effectPropType = bindingAttr.BindingType;

            // Handle native types.
            if(effectPropType == PropertyType.Unknown)
            {
                var propTypeName = propType.Name;
                if(propType == typeof(int))
                {
                    binding.nativeGetSet = new NativeGetSetValue<int>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Int32;
                }
                else if(propType == typeof(float))
                {
                    binding.nativeGetSet = new NativeGetSetValue<float>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Float;
                }
                else if(propType == typeof(uint))
                {
                    binding.nativeGetSet = new NativeGetSetValue<uint>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.UInt32;
                }
                else if(propType == typeof(bool))
                {
                    // For bool, we are using int as a transient value 
                    binding.nativeGetSet = new NativeGetSetValue<int>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Bool;
                }
                else if (propTypeName.Contains("Vector2")) // Use string property name to allow external types (Mathematics, System Numerics...etc.)
                {
                    binding.nativeGetSet = new NativeGetSetValue<RawVector2>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Vector2;
                }
                else if(propTypeName.Contains("Vector3"))
                {
                    binding.nativeGetSet = new NativeGetSetValue<RawVector3>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Vector3;
                }
                else if (propTypeName.Contains("RectangleF"))
                {
                    binding.nativeGetSet = new NativeGetSetValue<RawRectangleF>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Vector4;
                }
                else if(propTypeName.Contains("Vector4"))
                {
                    binding.nativeGetSet = new NativeGetSetValue<RawVector4>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Vector4;
                }
                else if(propTypeName.Contains("Color3"))
                {
                    binding.nativeGetSet = new NativeGetSetValue<RawColor3>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Vector3;
                }
                else if(propTypeName.Contains("Color4"))
                {
                    binding.nativeGetSet = new NativeGetSetValue<RawColor4>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Vector4;
                }
                else if(propTypeName.Contains("Matrix3x2"))
                {
                    binding.nativeGetSet = new NativeGetSetValue<RawMatrix3x2>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Matrix3x2;
                }
                else if(propTypeName.Contains("Matrix5x4"))
                {
                    binding.nativeGetSet = new NativeGetSetValue<RawMatrix5x4>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Matrix5x4;
                }
                else if (propTypeName.Contains("Matrix"))
                {
                    binding.nativeGetSet = new NativeGetSetValue<RawMatrix>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Matrix4x4;
                }
                else if (Utilities.IsEnum(propType))
                {
                    // For enum, we are using int as a transient value
                    binding.nativeGetSet = new NativeGetSetValue<int>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.Enum;
                }
                else if(Utilities.IsAssignableFrom(typeof(ComObject), propType))
                {
                    // For ComObject we are using IntPtr as a transient value
                    binding.nativeGetSet = new NativeGetSetValue<IntPtr>(customEffectType, propertyInfo);
                    effectPropType = PropertyType.IUnknown;
                }
                //else if (propTypeInfo.IsArray)
                //{
                //    // TODO handle arrays
                //    // binding.nativeGetSet = new NativeGetSetValue<Matrix5x4>(customEffectType, propertyInfo);
                //}
                else
                {
                    throw new SharpDXException(string.Format("Unsupported property type [{0}] with binding [{1}] for custom effect [{2}]",
                        propType,
                        effectPropType,
                        customEffectType));
                }
            }

            // Set the type
            bindingAttr.Type = effectPropType;
            binding.PropertyName = bindingAttr.DisplayName ?? propertyInfo.Name;
            binding.GetFunction = binding.nativeGetSet.GetFunctionPtr;
            binding.SetFunction = binding.nativeGetSet.SetFunctionPtr;

            return binding;
        }

        public abstract class NativeGetSet {
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            protected delegate int NativeSetFunctionDelegate(IntPtr thisPtr, IntPtr dataPtr, int dataSize);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            protected delegate int NativeGetFunctionDelegate(IntPtr thisPtr, IntPtr dataPtr, int dataSize, out int actualSize);

            protected NativeGetFunctionDelegate getterNative;
            protected NativeSetFunctionDelegate setterNative;

            protected Type customEffectType;
            protected PropertyInfo propertyInfo;

            public NativeGetSet(Type customEffectType, PropertyInfo propertyInfo)
            {
                this.customEffectType = customEffectType;
                this.propertyInfo = propertyInfo;
            }

            public IntPtr GetFunctionPtr
            {
                get;
                protected set;
            }

            public IntPtr SetFunctionPtr
            {
                get;
                protected set;
            }
        }

        public class NativeGetSetValue<T> : NativeGetSet where T : struct
        {
            private readonly GetValueFastDelegate<T> getter;
            private readonly SetValueFastDelegate<T> setter;
            private static readonly int ValueSize;

            static NativeGetSetValue()
            {
                ValueSize = Utilities.SizeOf<T>();
            }

            public NativeGetSetValue(Type customEffectType, PropertyInfo propertyInfo)
                : base(customEffectType, propertyInfo)
            {
                if (propertyInfo.CanRead)
                {
                    getter = Utilities.BuildPropertyGetter<T>(customEffectType, propertyInfo);
                    getterNative = new NativeGetFunctionDelegate(NativeGetInt);
                    GetFunctionPtr = Marshal.GetFunctionPointerForDelegate(getterNative);
                }

                if (propertyInfo.CanWrite)
                {
                    setter = Utilities.BuildPropertySetter<T>(customEffectType, propertyInfo);
                    setterNative = new NativeSetFunctionDelegate(NativeSetInt);
                    SetFunctionPtr = Marshal.GetFunctionPointerForDelegate(setterNative);
                }
            }

            protected void SetValue(IntPtr sourceValue, out T destValue)
            {
                Utilities.ReadOut<T>(sourceValue, out destValue);
            }

            protected void GetValue(IntPtr destValue, ref T sourceValue)
            {
                Utilities.Write<T>(destValue, ref sourceValue);
            }

            private int NativeSetInt(IntPtr thisPtr, IntPtr dataPtr, int dataSize)
            {
                try
                {
                    if(dataPtr == IntPtr.Zero)
                    {
                        // TODO: Should we throw an exception
                        return Result.Ok.Code;
                    }

                    if(dataSize != ValueSize)
                    {
                        return Result.InvalidArg.Code;
                    }

                    // TODO: Check dataSize against <T>?
                    var shadow = CustomEffectShadow.ToShadow<CustomEffectShadow>(thisPtr);
                    var callback = (CustomEffect)shadow.Callback;
                    T value;
                    SetValue(dataPtr, out value);
                    setter(callback, ref value);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            private int NativeGetInt(IntPtr thisPtr, IntPtr dataPtr, int dataSize, out int actualSize)
            {
                actualSize = ValueSize;
                try
                {
                    if (dataPtr == IntPtr.Zero)
                    {
                        // TODO: Should we throw an exception
                        return Result.Ok.Code;
                    }

                    // TODO: Check dataSize against <T>?
                    var shadow = CustomEffectShadow.ToShadow<CustomEffectShadow>(thisPtr);
                    var callback = (CustomEffect)shadow.Callback;
                    T value;
                    actualSize = ValueSize;
                    getter(callback, out value);
                    GetValue(dataPtr, ref value);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }
        }
    }
}
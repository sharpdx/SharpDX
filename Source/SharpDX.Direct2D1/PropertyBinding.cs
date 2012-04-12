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
#if DIRECT3D11_1
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
using System.Xml.Linq;

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
                return Attribute.Type.ConvertToString();
            }
        }

        public PropertyBindingAttribute Attribute { get; private set; }

        public static PropertyBinding Get(Type customEffectType, PropertyInfo propertyInfo)
        {
            var bindingAttr = propertyInfo.GetCustomAttribute<PropertyBindingAttribute>(true);
            if (bindingAttr == null)
                return null;

            var binding = new PropertyBinding();
            binding.Attribute = bindingAttr;

            var propType = propertyInfo.PropertyType;
            var propTypeInfo = propType.GetTypeInfo();

            var effectPropType = PropertyType.Unknown;

            if (propType == typeof(int))
            {
                binding.nativeGetSet = new NativeGetSetValue<int>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Int32;
            }
            else if (propType == typeof(float))
            {
                binding.nativeGetSet = new NativeGetSetValue<float>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Float;
            }
            else if (propType == typeof(uint))
            {
                binding.nativeGetSet = new NativeGetSetValue<uint>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Uint32;
            }
            else if (propType == typeof(bool))
            {
                // For bool, we are using int as a transient value 
                binding.nativeGetSet = new NativeGetSetValue<int>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Bool;
            }
            else if (propType == typeof(Vector2))
            {
                binding.nativeGetSet = new NativeGetSetValue<Vector2>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Vector2;
            }
            else if (propType == typeof(Vector3))
            {
                binding.nativeGetSet = new NativeGetSetValue<Vector3>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Vector3;
            }
            else if (propType == typeof(Vector4))
            {
                binding.nativeGetSet = new NativeGetSetValue<Vector4>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Vector4;
            }
            else if (propType == typeof(Color3))
            {
                binding.nativeGetSet = new NativeGetSetValue<Color3>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Vector3;
            }
            else if (propType == typeof(Color4))
            {
                binding.nativeGetSet = new NativeGetSetValue<Color4>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Vector4;
            }
            else if (propType == typeof(Matrix))
            {
                binding.nativeGetSet = new NativeGetSetValue<Matrix>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Matrix4x4;
            }
            else if (propType == typeof(Matrix3x2))
            {
                binding.nativeGetSet = new NativeGetSetValue<Matrix3x2>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Matrix3x2;
            }
            else if (propType == typeof(Matrix5x4))
            {
                binding.nativeGetSet = new NativeGetSetValue<Matrix5x4>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Matrix5x4;
            }
            else if (propTypeInfo.IsEnum)
            {
                // For enum, we are using int as a transient value
                binding.nativeGetSet = new NativeGetSetValue<int>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Enum;
            }
            else if (typeof(ComObject).GetTypeInfo().IsAssignableFrom(propTypeInfo))
            {
                // For ComObject we are using IntPtr as a transient value
                binding.nativeGetSet = new NativeGetSetValue<IntPtr>(customEffectType, propertyInfo);
                effectPropType = PropertyType.Iunknown;
            }
            //else if (propTypeInfo.IsArray)
            //{
            //    // TODO handle arrays
            //    // binding.nativeGetSet = new NativeGetSetValue<Matrix5x4>(customEffectType, propertyInfo);
            //}
            else
            {
                throw new SharpDXException(string.Format("Unsupported property type [{0}] for custom effect [{1}]", propType, customEffectType));
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

            protected ParameterExpression customEffectParam;
            protected Expression propertyAccessor;

            public NativeGetSet(Type customEffectType, PropertyInfo propertyInfo)
            {
                this.customEffectType = customEffectType;
                this.propertyInfo = propertyInfo;
                this.customEffectParam = Expression.Parameter(typeof(CustomEffect));
                var castParam = Expression.Convert(customEffectParam, customEffectType);
                this.propertyAccessor = Expression.Property(castParam, propertyInfo);
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
            private delegate void GetValueDelegate(CustomEffect effect, out T value);
            private delegate void SetValueDelegate(CustomEffect effect, ref T value);

            private GetValueDelegate getter;
            private SetValueDelegate setter;

            protected ParameterExpression valueParam;

            public NativeGetSetValue(Type customEffectType, PropertyInfo propertyInfo)
                : base(customEffectType, propertyInfo)
            {
                this.valueParam = Expression.Parameter(typeof(T).MakeByRefType());

                if (propertyInfo.CanRead)
                {
                    // void GetValueDelegate(CustomEffect effect, out T value) {
                    //      value = (T)effect.Property;
                    // }
                    Expression convertExpression;
                    if (propertyInfo.PropertyType == typeof(bool))
                    {
                        // Convert bool to int: effect.Property ? 1 : 0
                        convertExpression = Expression.Condition(propertyAccessor, Expression.Constant(1), Expression.Constant(0));
                    }
                    else
                    {
                        convertExpression = Expression.Convert(propertyAccessor, typeof(T));
                    }
                    getter = Expression.Lambda<GetValueDelegate>(Expression.Assign(valueParam, convertExpression), customEffectParam, valueParam).Compile();
                    getterNative = new NativeGetFunctionDelegate(NativeGetInt);
                    GetFunctionPtr = Marshal.GetFunctionPointerForDelegate(getterNative);
                }

                if (propertyInfo.CanWrite)
                {
                    // void SetValueDelegate(CustomEffect effect, ref T value) {
                    //      effect.Property = (PropertyType)value;
                    // }
                    Expression convertExpression;
                    if (propertyInfo.PropertyType == typeof(bool))
                    {
                        // Convert int to bool: value != 0
                        convertExpression = Expression.NotEqual(valueParam, Expression.Constant(0));
                    }
                    else
                    {
                        convertExpression = Expression.Convert(valueParam, propertyInfo.PropertyType);
                    }
                    setter = Expression.Lambda<SetValueDelegate>(Expression.Assign(propertyAccessor, convertExpression), customEffectParam, valueParam).Compile();
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

            protected int ValueSize
            {
                get
                {
                    return Utilities.SizeOf<T>();
                }
            }

            private int NativeSetInt(IntPtr thisPtr, IntPtr dataPtr, int dataSize)
            {
                try
                {
                    // TODO: Check dataSize against <T>?
                    var shadow = CustomEffectShadow.ToShadow<CustomEffectShadow>(thisPtr);
                    var callback = (CustomEffect)shadow.Callback;
                    T value;
                    SetValue(dataPtr, out value);
                    setter(callback, ref value);
                }
                catch (SharpDXException exception)
                {
                    return exception.ResultCode.Code;
                }
                catch (Exception)
                {
                    return Result.Fail.Code;
                }
                return Result.Ok.Code;
            }

            private int NativeGetInt(IntPtr thisPtr, IntPtr dataPtr, int dataSize, out int actualSize)
            {
                actualSize = ValueSize;
                try
                {
                    // TODO: Check dataSize against <T>?
                    var shadow = CustomEffectShadow.ToShadow<CustomEffectShadow>(thisPtr);
                    var callback = (CustomEffect)shadow.Callback;
                    T value;
                    getter(callback, out value);
                    GetValue(dataPtr, ref value);
                }
                catch (SharpDXException exception)
                {
                    return exception.ResultCode.Code;
                }
                catch (Exception)
                {
                    return Result.Fail.Code;
                }
                return Result.Ok.Code;
            }
        }
    }
}
#endif
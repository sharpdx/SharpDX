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
#if WIN8
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Delegate used by to create a custom effect.
    /// </summary>
    /// <returns>A new instance of custom effect</returns>
    internal delegate CustomEffect CustomEffectFactoryDelegate();

    /// <summary>
    /// Internal class used to keep reference to factory.
    /// </summary>
    internal class CustomEffectFactory
    {
        private Type customEffectType;
        private CreateCustomEffectDelegate callback;
        private XDocument xml;

        public CustomEffectFactory(CustomEffectFactoryDelegate factory, Type customEffectType)
        {
            this.customEffectType = customEffectType;

            // Gets the guid of this class
            Guid = customEffectType.GetTypeInfo().GUID;

            unsafe
            {
                Factory = factory;
                callback = new CreateCustomEffectDelegate(CreateCustomEffectImpl);
                NativePointer = Marshal.GetFunctionPointerForDelegate(callback);
            }

            InitializeBindings();
            InitializeXml();
        }

        public Guid Guid { get; private set; }

        public CustomEffectFactoryDelegate Factory { get; private set; }

        public IntPtr NativePointer { get; private set; }

        public PropertyBinding[] Bindings { get; private set; }

        /// <summary>
        /// Converts custom effect to an xml description
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            return string.Format("<?xml version='1.0'?>\r\n{0}", xml.ToString(SaveOptions.None));
        }

        /// <summary>
        /// Initializes the property bindings
        /// </summary>
        private void InitializeBindings()
        {
            var bindings = new List<PropertyBinding>();
            foreach (var propertyInfo in customEffectType.GetTypeInfo().DeclaredProperties)
            {
                var binding = PropertyBinding.Get(customEffectType, propertyInfo);
                if (binding != null) 
                    bindings.Add(binding);
            }

            // Sort property bindings by property order
            // Don't rely on reflection to get property order, as there is no waranty
            bindings.Sort((left, right) => left.Attribute.Order.CompareTo(right.Attribute.Order));

            Bindings = bindings.ToArray();
        }

        private static XElement CreateXmlProperty(string name, string type, string value = null)
        {
            var property = new XElement("Property");
            property.SetAttributeValue("name", name);
            property.SetAttributeValue("type", type);
            if (value != null)
                property.SetAttributeValue("value", value);

            return property;
        }

        /// <summary>
        /// Initializes the xml descriptor for this effect.
        /// </summary>
        private void InitializeXml()
        {
            xml = new XDocument();
            var effect = new XElement("Effect");
            xml.Add(effect);

            var customEffectTypeInfo = customEffectType.GetTypeInfo();

            // Add 
            var customEffectAttribute = customEffectTypeInfo.GetCustomAttribute<CustomEffectAttribute>(true);           
            effect.Add(CreateXmlProperty("DisplayName", "string", customEffectAttribute != null ? customEffectAttribute.DisplayName : string.Empty));
            effect.Add(CreateXmlProperty("Author", "string", customEffectAttribute != null ? customEffectAttribute.Author : string.Empty));
            effect.Add(CreateXmlProperty("Category", "string", customEffectAttribute != null ? customEffectAttribute.Category : string.Empty));
            effect.Add(CreateXmlProperty("Description", "string", customEffectAttribute != null ? customEffectAttribute.Description : string.Empty));

            var inputs = new XElement("Inputs");
            var inputAttributes = customEffectTypeInfo.GetCustomAttributes<CustomEffectInputAttribute>(true);
            foreach(var inputAttribute in inputAttributes) {
                var inputXml = new XElement("Input");
                inputXml.SetAttributeValue("name", inputAttribute.Input);
                inputs.Add(inputXml);
            }
            effect.Add(inputs);

            // Add custom properties
            foreach(var binding in Bindings) {
                var property = CreateXmlProperty(binding.PropertyName,  binding.TypeName);

                property.Add(CreateXmlProperty("DisplayName", "string", binding.PropertyName));
                property.Add(CreateXmlProperty("Min", binding.TypeName, binding.Attribute.Min != null ? binding.Attribute.Min.ToString() : string.Empty));
                property.Add(CreateXmlProperty("Max", binding.TypeName, binding.Attribute.Max != null ? binding.Attribute.Max.ToString() : string.Empty));
                property.Add(CreateXmlProperty("Default", binding.TypeName, binding.Attribute.Default != null ? binding.Attribute.Default.ToString() : string.Empty));

                effect.Add(property);
            }
        }

        /// <unmanaged>HRESULT ID2D1EffectImpl::Initialize([In] ID2D1EffectContext* effectContext,[In] ID2D1TransformGraph* transformGraph)</unmanaged>	
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int CreateCustomEffectDelegate(out IntPtr nativeCustomEffectPtr);
        private int CreateCustomEffectImpl(out IntPtr nativeCustomEffectPtr)
        {
            nativeCustomEffectPtr = IntPtr.Zero;
            try
            {
                var customEffect = Factory();
                nativeCustomEffectPtr = CustomEffectShadow.ToIntPtr(customEffect);
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
#endif
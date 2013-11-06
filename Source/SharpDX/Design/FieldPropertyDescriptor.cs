// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
// -----------------------------------------------------------------------------
// Original code from SlimMath project. http://code.google.com/p/slimmath/
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
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
#if !W8CORE
using System;
using System.ComponentModel;
using System.Reflection;

namespace SharpDX.Design
{
    /// <summary>The field property descriptor class.</summary>
    public class FieldPropertyDescriptor : PropertyDescriptor
    {
        /// <summary>The field information.</summary>
        readonly FieldInfo fieldInfo;

        /// <summary>When overridden in a derived class, gets the type of the component this property is bound to.</summary>
        /// <value>The type of the component.</value>
        /// <returns>A <see cref="T:System.Type" /> that represents the type of component this property is bound to. When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)" /> or <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)" /> methods are invoked, the object specified might be an instance of this type.</returns>
        public override Type ComponentType
        {
            get { return fieldInfo.DeclaringType; }
        }

        /// <summary>When overridden in a derived class, gets a value indicating whether this property is read-only.</summary>
        /// <value><see langword="true" /> if this instance is read only; otherwise, <see langword="false" />.</value>
        /// <returns>true if the property is read-only; otherwise, false.</returns>
        public override bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>When overridden in a derived class, gets the type of the property.</summary>
        /// <value>The type of the property.</value>
        /// <returns>A <see cref="T:System.Type" /> that represents the type of the property.</returns>
        public override Type PropertyType
        {
            get { return fieldInfo.FieldType; }
        }

        /// <summary>Initializes a new instance of the <see cref="FieldPropertyDescriptor"/> class.</summary>
        /// <param name="fieldInfo">The field information.</param>
        public FieldPropertyDescriptor(FieldInfo fieldInfo)
            : base(fieldInfo.Name, new Attribute[0])
        {
            this.fieldInfo = fieldInfo;

            var attributesObject = fieldInfo.GetCustomAttributes(true);
            var attributes = new Attribute[attributesObject.Length];
            for (int i = 0; i < attributes.Length; i++)
                attributes[i] = (Attribute)attributesObject[i];
            this.AttributeArray = attributes;
        }

        /// <summary>When overridden in a derived class, returns whether resetting an object changes its value.</summary>
        /// <param name="component">The component to test for reset capability.</param>
        /// <returns>true if resetting the component changes its value; otherwise, false.</returns>
        public override bool CanResetValue(object component)
        {
            return false;
        }

        /// <summary>When overridden in a derived class, gets the current value of the property on a component.</summary>
        /// <param name="component">The component with the property for which to retrieve the value.</param>
        /// <returns>The value of a property for a given component.</returns>
        public override object GetValue(object component)
        {
            return fieldInfo.GetValue(component);
        }

        /// <summary>When overridden in a derived class, resets the value for this property of the component to the default value.</summary>
        /// <param name="component">The component with the property value that is to be reset to the default value.</param>
        public override void ResetValue(object component)
        {
        }

        /// <summary>When overridden in a derived class, sets the value of the component to a different value.</summary>
        /// <param name="component">The component with the property value that is to be set.</param>
        /// <param name="value">The new value.</param>
        public override void SetValue(object component, object value)
        {
            fieldInfo.SetValue(component, value);
            OnValueChanged(component, EventArgs.Empty);
        }

        /// <summary>When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.</summary>
        /// <param name="component">The component with the property to be examined for persistence.</param>
        /// <returns>true if the property should be persisted; otherwise, false.</returns>
        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return fieldInfo.GetHashCode();
        }

        /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
        /// <param name="obj">The object to compare to this <see cref="T:System.ComponentModel.PropertyDescriptor" />.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return GetType() == obj.GetType() && ((FieldPropertyDescriptor)obj).fieldInfo.Equals(fieldInfo);
        }
    }
}
#endif
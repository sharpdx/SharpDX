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
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Helper functions for <see cref="PropertyType"/>.
    /// </summary>
    public static class PropertyTypeHelper
    {
        /// <summary>
        /// Converts a property type to a text.
        /// </summary>
        /// <param name="propertyType">The property type</param>
        /// <returns>A string representing this property type.</returns>
        public static string ConvertToString(PropertyType propertyType)
        {
            string propText;
            switch (propertyType)
            {
                case PropertyType.Bool:
                    propText = "bool";
                    break;
                case PropertyType.Clsid:
                    propText = "clsid";
                    break;
                case PropertyType.ColorContext:
                    propText = "colorcontext";
                    break;
                case PropertyType.Enum:
                    propText = "enum";
                    break;
                case PropertyType.Float:
                    propText = "float";
                    break;
                case PropertyType.Int32:
                    propText = "int32";
                    break;
                case PropertyType.IUnknown:
                    propText = "iunknown";
                    break;
                case PropertyType.Matrix3x2:
                    propText = "matrix3x2";
                    break;
                case PropertyType.Matrix4x3:
                    propText = "matrix4x3";
                    break;
                case PropertyType.Matrix4x4:
                    propText = "matrix4x4";
                    break;
                case PropertyType.Matrix5x4:
                    propText = "matrix5x4";
                    break;
                case PropertyType.String:
                    propText = "string";
                    break;
                case PropertyType.UInt32:
                    propText = "uint32";
                    break;
                case PropertyType.Unknown:
                    propText = "unknown";
                    break;
                case PropertyType.Vector2:
                    propText = "vector2";
                    break;
                case PropertyType.Vector3:
                    propText = "vector3";
                    break;
                case PropertyType.Vector4:
                    propText = "vector4";
                    break;
                default:
                    propText = "unknown";
                    break;
            }

            return propText;
        }
    }
}
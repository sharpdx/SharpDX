using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Mathematics.Interop;
using System.Runtime.InteropServices;

namespace SharpDX.Direct2D1
{
    public partial class SvgElement
    {
        /// <summary>
        /// Gets all children of this element.
        /// </summary>
        public SvgElement[] Children
        {
            get
            {
                if (!HasChildren())
                    return new SvgElement[0];

                //We do not know the amount of children, and nothing in api does help us, count pass
                SvgElement current = FirstChild;
                int childCount = 0;

                SvgElement next;
                do
                {
                    GetNextChild(current, out next);
                    current = next;
                    childCount++;
                }
                while (next != null);

                SvgElement[] result = new SvgElement[childCount];

                current = FirstChild;
                for (int i = 0; i < childCount; i++)
                {
                    result[i] = current;
                    GetNextChild(current, out current);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets tag name for this element
        /// </summary>
        public unsafe string TagName
        {
            get
            {
                int nameLength = GetTagNameLength();
                sbyte* name = stackalloc sbyte[nameLength];

                GetTagName(new IntPtr(name), nameLength + 1);
                return Marshal.PtrToStringUni((IntPtr)name, nameLength);
            }
        }

        /// <summary>
        /// Sets float attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">New valuee</param>
        public unsafe void SetAttributeValue(string name, float value)
        {
            SetAttributeValue(name, SvgAttributePodType.Float, new IntPtr(&value), sizeof(float));
        }

        /// <summary>
        /// Gets a float attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out float value)
        {
            fixed (float* ptr = &value)
            {
                GetAttributeValue(name, SvgAttributePodType.Float, new IntPtr(ptr), sizeof(float));
            }
        }

        /// <summary>
        /// Sets color attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="color">New color</param>
        public unsafe void SetAttributeValue(string name, RawColor4 color)
        {
            SetAttributeValue(name, SvgAttributePodType.Color, new IntPtr(&color), sizeof(RawColor4));
        }

        /// <summary>
        /// Gets a color attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="color">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out RawColor4 color)
        {
            fixed (RawColor4* ptr = &color)
            {
                GetAttributeValue(name, SvgAttributePodType.Color, new IntPtr(ptr), sizeof(RawColor4));
            }
        }

        /// <summary>
        /// Sets fill mode attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="fillMode">New fill mode</param>
        public unsafe void SetAttributeValue(string name, FillMode fillMode)
        {
            SetAttributeValue(name, SvgAttributePodType.FillMode, new IntPtr(&fillMode), sizeof(FillMode));
        }

        /// <summary>
        /// Gets a fill mode  attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="fillMode">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out FillMode fillMode)
        {
            fixed (FillMode* ptr = &fillMode)
            {
                GetAttributeValue(name, SvgAttributePodType.FillMode, new IntPtr(ptr), sizeof(FillMode));
            }
        }

        /// <summary>
        /// Sets display mode attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="display">New svg display</param>
        public unsafe void SetAttributeValue(string name, SvgDisplay display)
        {
            SetAttributeValue(name, SvgAttributePodType.Display, new IntPtr(&display), sizeof(SvgDisplay));
        }

        /// <summary>
        /// Gets a display attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="display">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out SvgDisplay display)
        {
            fixed (SvgDisplay* ptr = &display)
            {
                GetAttributeValue(name, SvgAttributePodType.Display, new IntPtr(ptr), sizeof(SvgDisplay));
            }
        }

        /// <summary>
        /// Sets overflow mode attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="overflow">New svg overfloe<param>
        public unsafe void SetAttributeValue(string name, SvgOverflow overflow)
        {
            SetAttributeValue(name, SvgAttributePodType.Overflow, new IntPtr(&overflow), sizeof(SvgOverflow));
        }

        /// <summary>
        /// Gets an overflow attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="overflow">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out SvgOverflow overflow)
        {
            fixed (SvgOverflow* ptr = &overflow)
            {
                GetAttributeValue(name, SvgAttributePodType.Overflow, new IntPtr(ptr), sizeof(SvgOverflow));
            }
        }

        /// <summary>
        /// Sets line join attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="lineJoin">New svg line join</param>
        public unsafe void SetAttributeValue(string name, SvgLineJoin lineJoin)
        {
            SetAttributeValue(name, SvgAttributePodType.LineJoin, new IntPtr(&lineJoin), sizeof(SvgLineJoin));
        }

        /// <summary>
        /// Gets a line join attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="lineJoin">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out SvgLineJoin lineJoin)
        {
            fixed (SvgLineJoin* ptr = &lineJoin)
            {
                GetAttributeValue(name, SvgAttributePodType.LineJoin, new IntPtr(ptr), sizeof(SvgLineJoin));
            }
        }


        /// <summary>
        /// Sets line cap attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="lineCap">New svg line cap</param>
        public unsafe void SetAttributeValue(string name, SvgLineCap lineCap)
        {
            SetAttributeValue(name, SvgAttributePodType.LineCap, new IntPtr(&lineCap), sizeof(SvgLineCap));
        }


        /// <summary>
        /// Gets a line cap attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="lineCap">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out SvgLineCap lineCap)
        {
            fixed (SvgLineCap* ptr = &lineCap)
            {
                GetAttributeValue(name, SvgAttributePodType.LineCap, new IntPtr(ptr), sizeof(SvgLineCap));
            }
        }


        /// <summary>
        /// Sets visibility attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="visibility">New svg visibility</param>
        public unsafe void SetAttributeValue(string name, SvgVisibility visibility)
        {
            SetAttributeValue(name, SvgAttributePodType.Visibility, new IntPtr(&visibility), sizeof(SvgVisibility));
        }

        /// <summary>
        /// Gets a visibility attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="visibility">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out SvgVisibility visibility)
        {
            fixed (SvgVisibility* ptr = &visibility)
            {
                GetAttributeValue(name, SvgAttributePodType.Visibility, new IntPtr(ptr), sizeof(SvgVisibility));
            }
        }

        /// <summary>
        /// Sets transform attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="matrix">New transform</param>
        public unsafe void SetAttributeValue(string name, RawMatrix3x2 matrix)
        {
            SetAttributeValue(name, SvgAttributePodType.Matrix, new IntPtr(&matrix), sizeof(RawMatrix3x2));
        }


        /// <summary>
        /// Gets a transform attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="matrix">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out RawMatrix3x2 matrix)
        {
            fixed (RawMatrix3x2* ptr = &matrix)
            {
                GetAttributeValue(name, SvgAttributePodType.Matrix, new IntPtr(ptr), sizeof(RawMatrix3x2));
            }
        }

        /// <summary>
        /// Sets unit type attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="unitType">New unit type</param>
        public unsafe void SetAttributeValue(string name, SvgUnitType unitType)
        {
            SetAttributeValue(name, SvgAttributePodType.UnitType, new IntPtr(&unitType), sizeof(SvgUnitType));
        }

        /// <summary>
        /// Gets a unit type attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="unitType">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out SvgUnitType unitType)
        {
            fixed (SvgUnitType* ptr = &unitType)
            {
                GetAttributeValue(name, SvgAttributePodType.UnitType, new IntPtr(ptr), sizeof(SvgUnitType));
            }
        }

        /// <summary>
        /// Sets extend mode attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="extendMode">New extend mode</param>
        public unsafe void SetAttributeValue(string name, ExtendMode extendMode)
        {
            SetAttributeValue(name, SvgAttributePodType.ExtendMode, new IntPtr(&extendMode), sizeof(ExtendMode));
        }

        /// <summary>
        /// Gets an extend mode attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="extendMode">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out ExtendMode extendMode)
        {
            fixed (ExtendMode* ptr = &extendMode)
            {
                GetAttributeValue(name, SvgAttributePodType.ExtendMode, new IntPtr(ptr), sizeof(ExtendMode));
            }
        }

        /// <summary>
        /// Sets preserve Aspect Ratio attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="preserveAspectRatio">preserve Aspect Ratio/param>
        public unsafe void SetAttributeValue(string name, SvgPreserveAspectRatio preserveAspectRatio)
        {
            SetAttributeValue(name, SvgAttributePodType.PreserveAspectRatio, new IntPtr(&preserveAspectRatio), sizeof(SvgPreserveAspectRatio));
        }

        /// <summary>
        /// Gets a preserve aspect ratio attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="preserveAspectRatio">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out SvgPreserveAspectRatio preserveAspectRatio)
        {
            fixed (SvgPreserveAspectRatio* ptr = &preserveAspectRatio)
            {
                GetAttributeValue(name, SvgAttributePodType.PreserveAspectRatio, new IntPtr(ptr), sizeof(SvgPreserveAspectRatio));
            }
        }

        /// <summary>
        /// Sets length attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="length">New length</param>
        public unsafe void SetAttributeValue(string name, SvgLength length)
        {
            SetAttributeValue(name, SvgAttributePodType.Length, new IntPtr(&length), sizeof(SvgLength));
        }

        /// <summary>
        /// Gets an svg length attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="length">When this returns , contains the attribute value</param>
        public unsafe void GetAttributeValue(string name, out SvgLength length)
        {
            fixed (SvgLength* ptr = &length)
            {
                GetAttributeValue(name, SvgAttributePodType.Length, new IntPtr(ptr), sizeof(SvgLength));
            }
        }

        /// <summary>
        /// Gets an attribute from a type id
        /// </summary>
        /// <typeparam name="T">Attribute type, must inherit from <see cref="SvgAttribute"/></typeparam>
        /// <param name="name">Attribute name</param>
        /// <returns>Attribute instance</returns>
        public T GetAttributeValue<T>(string name) where T : SvgAttribute
        {
            IntPtr nativePointer;
            GetAttributeValue(name, SharpDX.Utilities.GetGuidFromType(typeof(T)), out nativePointer);
            return ComObject.FromPointer<T>(nativePointer);
        }
    }
}

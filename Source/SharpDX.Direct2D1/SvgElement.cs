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
        /// Sets color attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="color">New color</param>
        public unsafe void SetAttributeValue(string name, RawColor4 color)
        {
            SetAttributeValue(name, SvgAttributePodType.Color, new IntPtr(&color), sizeof(RawColor4));
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
        /// Sets display mode attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="display">New svg display</param>
        public unsafe void SetAttributeValue(string name, SvgDisplay display)
        {
            SetAttributeValue(name, SvgAttributePodType.Display, new IntPtr(&display), sizeof(SvgDisplay));
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
        /// Sets line join attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="lineJoin">New svg line join</param>
        public unsafe void SetAttributeValue(string name, SvgLineJoin lineJoin)
        {
            SetAttributeValue(name, SvgAttributePodType.LineJoin, new IntPtr(&lineJoin), sizeof(SvgLineJoin));
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
        /// Sets visibility attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="visibility">New svg visibility</param>
        public unsafe void SetAttributeValue(string name, SvgVisibility visibility)
        {
            SetAttributeValue(name, SvgAttributePodType.Visibility, new IntPtr(&visibility), sizeof(SvgVisibility));
        }

        /// <summary>
        /// Sets transform attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="matrix">New transform</param>
        public unsafe void SetAttributeValue(string name, RawMatrix3x2 matrix)
        {
            SetAttributeValue(name, SvgAttributePodType.Visibility, new IntPtr(&matrix), sizeof(RawMatrix3x2));
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
        /// Sets extend mode attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="extendMode">New extend mode</param>
        public unsafe void SetAttributeValue(string name, ExtendMode extendMode)
        {
            SetAttributeValue(name, SvgAttributePodType.ExtendMode, new IntPtr(&extendMode), sizeof(ExtendMode));
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
        /// Sets length attribute
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="length">New length</param>
        public unsafe void SetAttributeValue(string name, SvgLength length)
        {
            SetAttributeValue(name, SvgAttributePodType.Length, new IntPtr(&length), sizeof(SvgLength));
        }
    }
}

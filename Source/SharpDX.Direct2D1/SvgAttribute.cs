using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.Direct2D1
{
    public partial class SvgAttribute
    {
        /// <summary>
        /// Clones an svg attribute
        /// </summary>
        /// <returns></returns>
        public SvgAttribute Clone()
        {
            SvgAttribute svgAttribute;
            Clone(out svgAttribute);
            return svgAttribute;
        }
    }
}

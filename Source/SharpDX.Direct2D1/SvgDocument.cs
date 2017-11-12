using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.Direct2D1
{
    public partial class SvgDocument
    {
        /// <summary>
        /// Finds an svg element by id
        /// </summary>
        /// <param name="id">Id to lookup for</param>
        /// <returns>SvgElement if found, null otherwise</returns>
        public SvgElement FindElementById(string id)
        {
            SharpDX.Result __result__;
            SvgElement svgElement;
            __result__ = TryFindElementById_(id, out svgElement);

            __result__.CheckError();
            return svgElement;
        }
    }
}

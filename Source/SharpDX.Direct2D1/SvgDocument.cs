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
        /// <returns>SvgElement</returns>
        public SvgElement FindElementById(string id)
        {
            SharpDX.Result __result__;
            SvgElement svgElement;
            __result__ = TryFindElementById_(id, out svgElement);

            __result__.CheckError();
            return svgElement;
        }

        /// <summary>
        /// Try to find an element by id
        /// </summary>
        /// <param name="id">id to search for</param>
        /// <param name="svgElement">When this method completes, contains the relevant element (if applicable)</param>
        /// <returns>true if element has been found, false otherwise</returns>
        public bool TryFindElementById(string id, out SvgElement svgElement)
        {
            SharpDX.Result __result__;
            __result__ = TryFindElementById_(id, out svgElement);

            return __result__.Code >= 0;
        }
    }
}

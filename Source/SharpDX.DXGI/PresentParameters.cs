using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpDX.DXGI
{
    public partial struct PresentParameters 
    {
        /// <summary>	
        /// <para>A list of updated rectangles that you update in the back buffer for the presented frame. An application must update every single pixel in each rectangle that it reports to the runtime; the application cannot assume that the pixels are saved from the previous frame. For more information about updating dirty rectangles, see Remarks. You can set this member to <c>null</c> if DirtyRectsCount is 0. An application must not update any pixel outside of the dirty rectangles.</para>	
        /// </summary>	
        /// <unmanaged>RECT* pDirtyRects</unmanaged>	
        public SharpDX.Rectangle[] DirtyRectangles;

        /// <summary>	
        /// <para> A reference to the scrolled rectangle. The scrolled rectangle is the rectangle of the previous frame from which the runtime bit-block transfers (bitblts) content. The runtime also uses the scrolled rectangle to optimize presentation in terminal server and indirect display scenarios.</para>	
        ///  <para>The scrolled rectangle also describes the destination rectangle, that is, the region on the current frame that is filled with scrolled content. You can set this member to <c>null</c> to indicate that no content is scrolled from the previous frame.</para>	
        /// </summary>	
        /// <unmanaged>RECT* pScrollRect</unmanaged>	
        public SharpDX.Rectangle? ScrollRectangle;

        /// <summary>	
        /// <para>A reference to the offset of the scrolled area that goes from the source rectangle (of previous frame) to the destination rectangle (of current frame). You can set this member to <c>null</c> to indicate no offset.</para>	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='DXGI_PRESENT_PARAMETERS::pScrollOffset']/*"/>	
        /// <unmanaged>POINT* pScrollOffset</unmanaged>	
        public SharpDX.DrawingPoint? ScrollOffset;

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public int DirtyRectsCount;
            public System.IntPtr PDirtyRects;
            public System.IntPtr PScrollRect;
            public System.IntPtr PScrollOffset;
        }
    }
}

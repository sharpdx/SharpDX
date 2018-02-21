using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectManipulation
{
	partial class CompositorNative
	{
        /// <summary>
        /// Class ID for compositor
        /// </summary>
		public static readonly Guid DirectCompositionCompositorClassId = new Guid("79DEA627-A08A-43AC-8EF5-6900B9299126");

		/// <summary>
		/// Creates a default implementation of IDirectManipulationCompositor that wraps a DirectComposition render tree.
		/// </summary>
		/// <returns></returns>
		public static CompositorNative CreateDefaultDirectCompositor()
		{
			CompositorNative newCompositor = new CompositorNative(IntPtr.Zero);
			Utilities.CreateComInstance(DirectCompositionCompositorClassId, Utilities.CLSCTX.ClsctxInprocServer, Utilities.GetGuidFromType(typeof(Compositor)), newCompositor);
			return newCompositor;
		}

		/// <summary>	
		/// <p>Associates content with the compositor, assigns a composition device to the content, and specifies the position of the content in the composition tree relative to other composition visuals. </p>Syntax<pre><see cref="SharpDX.Result"/> AddContent( [in]????????????<see cref="SharpDX.DirectManipulation.Content"/> *content, [in, optional]??<see cref="SharpDX.ComObject"/>   device, [in]????????????<see cref="SharpDX.ComObject"/> *parentVisual, [in]????????????<see cref="SharpDX.ComObject"/> *childVisual	
		/// );</pre>Parameters<dl> <dt><em>content</em> [in]</dt> <dd> <p>The content to add to the composition tree.</p> <p><em>content</em> is placed  between <em>parentVisual</em> and <em>childVisual</em> in the composition tree. </p> </dd> <dt><em> device</em> [in, optional]</dt> <dd> <p>The device used to compose the content. </p> <p><strong>Note</strong>??<em>device</em> is created by the application.</p> </dd> <dt><em>parentVisual</em> [in]</dt> <dd> <p>The parent in the composition tree for the content being added.</p> <p><em>parentVisual</em> must also be a parent of <em>childVisual</em> in the composition tree.</p> </dd> <dt><em>childVisual</em> [in]</dt> <dd> <p>The child in the composition tree for the content being added.</p> <p><em>parentVisual</em> must also be a parent of <em>childVisual</em> in the composition tree.</p> </dd> </dl>Return value<p>If the method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p>Remarks<p>All content, regardless of type, must be added to the compositor. This can be primary content, obtained from the viewport by calling <strong>GetPrimaryContent</strong>, or secondary content, such as a panning indicator, created by calling <strong>CreateContent</strong>.	
		/// </p><p>If the application uses a system-provided <strong><see cref="SharpDX.DirectManipulation.Compositor"/></strong>:</p><ul> <li><em>device</em> must be an  <strong><see cref="SharpDX.DirectComposition.Device"/></strong> object, and parent and child visuals must be <strong><see cref="SharpDX.DirectComposition.Visual"/></strong> objects.</li> <li><em>device</em>, <em>parentVisual</em>, and <em>childVisual</em> cannot be <c>null</c>. </li> <li><em>device</em>, <em>parentVisual</em>, and <em>childVisual</em> objects are created and owned by the application.	
		/// </li> <li>When content is added to the composition tree using this method, the new composition visuals are inserted between <em>parentVisual</em> and <em>childVisual</em>. The new visuals should not be destroyed until they are disassociated from the compositor with <strong>RemoveContent</strong>.</li> </ul><p>If the application uses a custom implementation of <strong><see cref="SharpDX.DirectManipulation.Compositor"/></strong>:</p><ul> <li><em>device</em>, <em>parentVisual</em>, and <em>childVisual</em> must be a valid type for the compositor. They do not have to be <strong><see cref="SharpDX.DirectComposition.Device"/></strong> or <strong><see cref="SharpDX.DirectComposition.Visual"/></strong> objects.</li> <li><em>device</em>, <em>parentVisual</em>, and <em>childVisual</em> can be <c>null</c>, depending on the compositor. </li> </ul>Requirements	
		/// </summary>	
		/// <param name="content"><dd> <p>The content to add to the composition tree.</p> <p><em>content</em> is placed  between <em>parentVisual</em> and <em>childVisual</em> in the composition tree. </p> </dd></param>	
		/// <param name="device"><dd> <p>The device used to compose the content. </p> <p><strong>Note</strong>??<em>device</em> is created by the application.</p> </dd></param>	
		/// <param name="arentVisualRef"><dd> <p>The parent in the composition tree for the content being added.</p> <p><em>parentVisual</em> must also be a parent of <em>childVisual</em> in the composition tree.</p> </dd></param>	
		/// <param name="childVisual"><dd> <p>The child in the composition tree for the content being added.</p> <p><em>parentVisual</em> must also be a parent of <em>childVisual</em> in the composition tree.</p> </dd></param>	
		/// <returns>No documentation.</returns>	
		/// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IDirectManipulationCompositor::AddContent']/*"/>	
		/// <msdn-id>Hh768898</msdn-id>	
		/// <unmanaged>HRESULT IDirectManipulationCompositor::AddContent([In] IDirectManipulationContent* content,[In, Optional] IUnknown* device,[In, Optional] IUnknown* parentVisual,[In, Optional] IUnknown* childVisual)</unmanaged>	
		/// <unmanaged-short>IDirectManipulationCompositor::AddContent</unmanaged-short>	
		public void AddContent(SharpDX.DirectManipulation.Content content, SharpDX.ComObject device, SharpDX.ComObject arentVisualRef, SharpDX.ComObject childVisual)
		{
			AddContent(content, (SharpDX.IUnknown)device, (SharpDX.IUnknown)arentVisualRef, (SharpDX.IUnknown)childVisual);
		}
	}
}

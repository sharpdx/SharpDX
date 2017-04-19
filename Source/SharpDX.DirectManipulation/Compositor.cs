using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectManipulation
{
	[Shadow(typeof(CompositorShadow))]
	partial interface Compositor
	{
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
		void AddContent(SharpDX.DirectManipulation.Content content, SharpDX.ComObject device, SharpDX.ComObject arentVisualRef, SharpDX.ComObject childVisual);

		/// <summary>	
		/// <p>Removes content from the compositor.</p>Syntax<pre><see cref="SharpDX.Result"/> RemoveContent( [in]??<see cref="SharpDX.DirectManipulation.Content"/> *content	
		/// );</pre>Parameters<dl> <dt><em>content</em> [in]</dt> <dd> <p>The content to remove from the composition tree.</p> </dd> </dl>Return value<p>If the method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p>Remarks<p>This method removes content added with <strong>AddContent</strong> and restores the original relationships between parent visuals and child visuals in the composition tree. In other words, <strong>RemoveContent</strong> undoes <strong>AddContent</strong>.</p>Requirements	
		/// </summary>	
		/// <param name="content"><dd> <p>The content to remove from the composition tree.</p> </dd></param>	
		/// <returns>No documentation.</returns>	
		/// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IDirectManipulationCompositor::RemoveContent']/*"/>	
		/// <msdn-id>Hh768899</msdn-id>	
		/// <unmanaged>HRESULT IDirectManipulationCompositor::RemoveContent([In] IDirectManipulationContent* content)</unmanaged>	
		/// <unmanaged-short>IDirectManipulationCompositor::RemoveContent</unmanaged-short>	
		void RemoveContent(SharpDX.DirectManipulation.Content content);

		/// <summary>	
		/// <p> </p><p> Sets the update manager used to send compositor updates to Direct Manipulation. </p>Syntax<pre><see cref="SharpDX.Result"/> SetUpdateManager( [in]??<see cref="SharpDX.DirectManipulation.UpdateManager"/> *updateManager	
		/// );</pre>Parameters<dl> <dt><em>updateManager</em> [in]</dt> <dd> <p>The <strong>update manager</strong>.</p> </dd> </dl>Return value<p>If the method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p>Remarks<p>Retrieve <em>updateManager</em> by calling <strong>GetUpdateManager</strong>.</p><p>Call this method during Direct Manipulation initialization to connect the compositor to the <em>update manager</em>.</p>Requirements	
		/// </summary>	
		/// <param name="updateManager">No documentation.</param>	
		/// <returns>No documentation.</returns>	
		/// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IDirectManipulationCompositor::SetUpdateManager']/*"/>	
		/// <msdn-id>Hh768900</msdn-id>	
		/// <unmanaged>HRESULT IDirectManipulationCompositor::SetUpdateManager([In] IDirectManipulationUpdateManager* updateManager)</unmanaged>	
		/// <unmanaged-short>IDirectManipulationCompositor::SetUpdateManager</unmanaged-short>	
		void SetUpdateManager(SharpDX.DirectManipulation.UpdateManager updateManager);

		/// <summary>	
		/// <p>Commits all pending updates in the compositor to the system for rendering.</p>Syntax<pre><see cref="SharpDX.Result"/> Flush();</pre>Parameters<p>This method has no parameters.</p>Return value<p>If the method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p>Remarks<p>This method enables Direct Manipulation to flush any pending changes to its visuals before a system event, such as a process suspension.</p>Requirements	
		/// </summary>	
		/// <returns>No documentation.</returns>	
		/// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IDirectManipulationCompositor::Flush']/*"/>	
		/// <msdn-id>jj647930</msdn-id>	
		/// <unmanaged>HRESULT IDirectManipulationCompositor::Flush()</unmanaged>	
		/// <unmanaged-short>IDirectManipulationCompositor::Flush</unmanaged-short>	
		void Flush();
	}

}

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
#if DIRECTX11_1
using System;

namespace SharpDX.Direct2D1
{
    public partial class TransformGraph
    {
        /// <summary>	
        /// Sets a single transform node as being equivalent to the whole graph.
        /// </summary>	
        /// <param name="node"><para>The node to be set.</para></param>	
        /// <remarks>	
        /// This equivalent to calling <see cref="SharpDX.Direct2D1.TransformGraph.Clear"/>, adding a single node, and connecting all of the node inputs to the effect inputs in order.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1TransformGraph::SetSingleTransformNode([In] ID2D1TransformNode* node)</unmanaged>	
        public void SetSingleTransformNode(TransformNode node)
        {
            SetSingleTransformNode_(TransformNodeShadow.ToIntPtr(node));
        }

        /// <summary>	
        /// Adds the provided node to the transform graph.
        /// </summary>	
        /// <param name="node"><para>The node that will be added to the transform graph.</para></param>	
        /// <remarks>	
        /// This adds a transform node to the transform graph. A node must be added to the transform graph before it can be interconnected in any way.A transform graph cannot be directly added to another transform graph. 	
        /// Any other kind of interface derived from <see cref="SharpDX.Direct2D1.TransformNode"/> can be added to the transform graph.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1TransformGraph::AddNode([In] ID2D1TransformNode* node)</unmanaged>	
        public void AddNode(TransformNode node)
        {
            AddNode_(TransformNodeShadow.ToIntPtr(node));
        }

        /// <summary>	
        /// Removes the provided node from the transform graph.
        /// </summary>	
        /// <param name="node"><para>The node that will be removed from the transform graph.</para></param>	
        /// <remarks>	
        /// The node must already exist in the graph; otherwise, the call fails with D2DERR_NOT_FOUND.Any connections to this node will be removed when the node is removed.After the node is removed, it cannot be used by the interface until it has been added to the graph by AddNode.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1TransformGraph::RemoveNode([In] ID2D1TransformNode* node)</unmanaged>	
        public void RemoveNode(TransformNode node)
        {
            RemoveNode_(TransformNodeShadow.ToIntPtr(node));
        }

        /// <summary>	
        /// Sets the output node for the transform graph.
        /// </summary>	
        /// <param name="node"><para>The node that will be set as the output of the the transform graph.</para></param>	
        /// <unmanaged>HRESULT ID2D1TransformGraph::SetOutputNode([In] ID2D1TransformNode* node)</unmanaged>	
        public void SetOutputNode(TransformNode node)
        {
            SetOutputNode_(TransformNodeShadow.ToIntPtr(node));
        }

        /// <summary>	
        /// Connects two nodes inside the transform graph.
        /// </summary>	
        /// <param name="fromNode">The node from which the connection will be made.</param>	
        /// <param name="toNode">The node to which the connection will be made.</param>	
        /// <param name="toNodeInputIndex">The node input that will be connected.</param>	
        /// <unmanaged>HRESULT ID2D1TransformGraph::ConnectNode([In] ID2D1TransformNode* fromNode,[In] ID2D1TransformNode* toNode,[In] unsigned int toNodeInputIndex)</unmanaged>	
        public void ConnectNode(TransformNode fromNode, TransformNode toNode, int toNodeInputIndex)
        {
            ConnectNode__(TransformNodeShadow.ToIntPtr(fromNode), TransformNodeShadow.ToIntPtr(toNode), toNodeInputIndex);
        }

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="toEffectInputIndex"><para>The effect input to which the transform node will be bound.</para></param>	
        /// <param name="node"><para>The node to which the connection will be made.</para></param>	
        /// <param name="toNodeInputIndex"><para>The node input that will be connected.</para></param>	
        /// <returns>The method returns an <see cref="SharpDX.Result"/>. Possible values include, but are not limited to, those in the following table.HRESULTDescription S_OKNo error occurred D2DERR_NOT_FOUND = (HRESULT_FROM_WIN32(<see cref="SharpDX.Win32.ErrorCode.NotFound"/>))Direct2D could not locate the specified node.?</returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1TransformGraph::ConnectToEffectInput']/*"/>	
        /// <unmanaged>HRESULT ID2D1TransformGraph::ConnectToEffectInput([In] unsigned int toEffectInputIndex,[In] ID2D1TransformNode* node,[In] unsigned int toNodeInputIndex)</unmanaged>	
        public void ConnectToEffectInput(int toEffectInputIndex, TransformNode node, int toNodeInputIndex)
        {
            ConnectToEffectInput_(toEffectInputIndex, TransformNodeShadow.ToIntPtr(node), toNodeInputIndex);
        }
    }
}
#endif
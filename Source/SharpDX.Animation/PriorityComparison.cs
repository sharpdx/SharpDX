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

namespace SharpDX.Animation
{
    [Shadow(typeof(PriorityComparisonShadow))]
    internal partial interface PriorityComparison
    {
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="scheduledStoryboard">No documentation.</param>	
        /// <param name="newStoryboard">No documentation.</param>	
        /// <param name="priorityEffect">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IUIAnimationPriorityComparison::HasPriority']/*"/>	
        /// <unmanaged>HRESULT IUIAnimationPriorityComparison::HasPriority([In] IUIAnimationStoryboard* scheduledStoryboard,[In] IUIAnimationStoryboard* newStoryboard,[In] UI_ANIMATION_PRIORITY_EFFECT priorityEffect)</unmanaged>	
        bool HasPriority( SharpDX.Animation.Storyboard scheduledStoryboard, SharpDX.Animation.Storyboard newStoryboard, SharpDX.Animation.PriorityEffect priorityEffect);
    }
}
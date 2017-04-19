// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

using System;

namespace SharpDX.MediaFoundation
{
    [Shadow(typeof(ClockStateSinkShadow))]
    public partial interface ClockStateSink
    {
        /// <summary>	
        /// Called when the presentation clock pauses.
        /// </summary>	
        /// <param name="hnsSystemTime"><para>The system time when the clock was paused, in 100-nanosecond units.</para></param>	
        /// <remarks>	
        /// When the presentation clock's Pause method is called, the clock notifies the presentation time source by calling the time source's OnClockPause method. This call occurs synchronously within the Pause method. If the time source returns an error from OnClockPause, the presentation clock's Pause method returns an error and the state change does not take place. For any object that is not the presentation time source, the OnClockPause method is called asynchronously, after the state change is completed. In that case, the return value from this method is ignored.
        /// </remarks>	
        /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='IMFClockStateSink::OnClockPause']/*"/>	
        /// <unmanaged>HRESULT IMFClockStateSink::OnClockPause([In] longlong hnsSystemTime)</unmanaged>
        void OnClockPause(long hnsSystemTime);

        /// <summary>	
        /// Called when the presentation clock restarts from the same position while paused.
        /// </summary>	
        /// <param name="hnsSystemTime"><para>The system time when the clock restarted, in 100-nanosecond units.</para></param>	
        /// <remarks>	
        /// This method is called if the presentation clock is paused and the Start method is called with the value PRESENTATION_CURRENT_POSITION. The clock notifies the presentation time source by calling the time source's OnClockRestart method. This call occurs synchronously within the Start method. If the time source returns an error from OnClockRestart, the presentation clock's Start method returns an error and the state change does not take place. For any object that is not the presentation time source, the OnClockRestart method is called asynchronously, after the state change is completed. In that case, the return value from this method is ignored.
        /// </remarks>	
        /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='IMFClockStateSink::OnClockRestart']/*"/>	
        /// <unmanaged>HRESULT IMFClockStateSink::OnClockRestart([In] longlong hnsSystemTime)</unmanaged>
        void OnClockRestart(long hnsSystemTime);

        /// <summary>	
        /// Called when the rate changes on the presentation clock.
        /// </summary>	
        /// <param name="hnsSystemTime"><para>The system time when the rate was set, in 100-nanosecond units.</para></param>	
        /// <param name="flRate"><para>The new rate, as a multiplier of the normal playback rate.</para></param>	
        /// <remarks>	
        /// When the presentation clock's SetRate method is called, the clock notifies the presentation time source by calling the time source's OnClockSetRate method. This call occurs synchronously within the SetRate method. If the time source returns an error from OnClockSetRate, the presentation clock's SetRate method returns an error and the state change does not take place. For any object that is not the presentation time source, the OnClockSetRate method is called asynchronously, after the state change is completed. In that case, the return value from this method is ignored.
        /// </remarks>
        /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='IMFClockStateSink::OnClockSetRate']/*"/>	
        /// <unmanaged>HRESULT IMFClockStateSink::OnClockSetRate([In] longlong hnsSystemTime,[In] float flRate)</unmanaged>
        void OnClockSetRate(long hnsSystemTime, float flRate);

        /// <summary>	
        /// Called when the presentation clock starts.	
        /// </summary>	
        /// <param name="hnsSystemTime"><para>The system time when the clock started, in 100-nanosecond units.</para></param>	
        /// <param name="llClockStartOffset"><para>The new starting time for the clock, in 100-nanosecond units. This parameter can also equal PRESENTATION_CURRENT_POSITION, indicating the clock has started or restarted from its current position.</para></param>	
        /// <remarks>	
        /// This method is called whe the presentation clock's Start method is called, with the following exception: If the clock is paused and Start is called with the value PRESENTATION_CURRENT_POSITION, OnClockRestart is called instead of OnClockStart. The clock notifies the presentation time source by calling the time source's OnClockStart method. This call occurs synchronously within the Start method. If the time source returns an error from OnClockStart, the presentation clock's Start method returns an error and the state change does not take place. For any object that is not the presentation time source, the OnClockStart method is called asynchronously, after the state change is completed. In that case, the return value from this method is ignored. The value given in llClockStartOffset is the presentation time when the clock starts, so it is relative to the start of the presentation. Media sinks should not render any data with a presentation time earlier than llClockStartOffSet. If a sample straddles the offset?that is, if the offset falls between the sample's start and stop times?the sink should either trim the sample so that only data after llClockStartOffset is rendered, or else simply drop the sample.
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFClockStateSink::OnClockStart']/*"/>	
        /// <unmanaged>HRESULT IMFClockStateSink::OnClockStart([In] longlong hnsSystemTime,[In] longlong llClockStartOffset)</unmanaged>
        void OnClockStart(long hnsSystemTime, long llClockStartOffset);

        /// <summary>	
        /// Called when the presentation clock stops.
        /// </summary>	
        /// <param name="hnsSystemTime"><para>The system time when the clock stopped, in 100-nanosecond units.</para></param>	
        /// <remarks>	
        /// When the presentation clock's Stop method is called, the clock notifies the presentation time source by calling the presentation time source's OnClockStop method. This call occurs synchronously within the Stop method. If the time source returns an error from OnClockStop, the presentation clock's Stop method returns an error and the state change does not take place. For any object that is not the presentation time source, the OnClockStop method is called asynchronously, after the state change is completed. If an object is already stopped, it should return Ok from OnClockStop. It should not return an error code. Note??Although the header file mferror.h defines an error code named MF_E_SINK_ALREADYSTOPPED, it should not be returned in this situation.
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFClockStateSink::OnClockStop']/*"/>	
        /// <unmanaged>HRESULT IMFClockStateSink::OnClockStop([In] longlong hnsSystemTime)</unmanaged>
        void OnClockStop(long hnsSystemTime);
    }
}
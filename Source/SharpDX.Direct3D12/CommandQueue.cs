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

using System;
using System.Threading;

namespace SharpDX.Direct3D12
{
    public partial class CommandQueue
    {
        /// <summary>	
        /// <p>Submits a command list for execution.</p>	
        /// </summary>	
        /// <param name="commandList"><dd> <p> The <strong><see cref="SharpDX.Direct3D12.CommandList"/></strong> command list to be executed. </p> </dd></param>
        /// <remarks>	
        /// <p> The driver is free to patch the submitted command lists. It is the calling application?s responsibility to ensure that the graphics processing unit (GPU) is not currently reading the any of the submitted command lists from a previous execution. </p><p> Applications are encouraged to batch together command list executions to reduce fixed costs associated with submitted commands to the GPU. </p>	
        /// </remarks>	
        public unsafe void ExecuteCommandList(CommandList commandList)
        {
            if(commandList == null) throw new ArgumentNullException("commandList");
            var ptr = commandList.NativePointer;
            this.ExecuteCommandLists(1, new IntPtr(&ptr));
        }

        /// <summary>	
        /// Submits an array of command lists for execution.	
        /// </summary>	
        /// <param name="commandLists">
        /// The array of <see cref="SharpDX.Direct3D12.CommandList"/> command lists to be executed.
        /// </param>	
        /// <remarks>	
        /// The driver is free to patch the submitted command lists.
        /// It is the calling application's responsibility to ensure that the graphics processing unit (GPU) is not currently reading the any of the submitted command lists from a previous execution.
        /// Applications are encouraged to batch together command list executions to reduce fixed costs associated with submitted commands to the GPU.
        /// </remarks>	
        public unsafe void ExecuteCommandLists(params SharpDX.Direct3D12.CommandList[] commandLists)
        {
            var commandListsPtr = (IntPtr*)0;

            int count = 0;
            if (commandLists != null)
            {
                count = commandLists.Length;
                IntPtr* tempPtr = stackalloc IntPtr[count];
                commandListsPtr = tempPtr;
                for (int i = 0; i < count; i++)
                    commandListsPtr[i] = (commandLists[i] == null) ? IntPtr.Zero : commandLists[i].NativePointer;
            }

            ExecuteCommandLists(count, new IntPtr(commandListsPtr));
        }
    }
}
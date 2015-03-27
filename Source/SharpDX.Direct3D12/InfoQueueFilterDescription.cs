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
using System;
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D12
{
    public partial class InfoQueueFilterDescription
    {
        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public MessageCategory[] Categories { get; set; }

        /// <summary>
        /// Gets or sets the severities.
        /// </summary>
        /// <value>
        /// The severities.
        /// </value>
        public MessageSeverity[] Severities { get; set; }

        /// <summary>
        /// Gets or sets the ids.
        /// </summary>
        /// <value>
        /// The ids.
        /// </value>
        public MessageId[] Ids { get; set; }

        // Internal native struct used for marshalling

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public int CategorieCount;
            public System.IntPtr PCategoryList;
            public int SeveritieCount;
            public System.IntPtr PSeverityList;
            public int IDCount;
            public System.IntPtr PIDList;
            // Method to free unmanaged allocation
            internal unsafe void __MarshalFree()
            {
                if (PCategoryList != IntPtr.Zero)
                    Marshal.FreeHGlobal(PCategoryList);
                if (PSeverityList != IntPtr.Zero)
                    Marshal.FreeHGlobal(PSeverityList);
                if (PIDList != IntPtr.Zero)
                    Marshal.FreeHGlobal(PIDList);
            }
        }

        internal unsafe void __MarshalFree(ref InfoQueueFilterDescription.__Native @ref)
        {
            @ref.__MarshalFree();
        }

        internal unsafe void __MarshalFrom(ref InfoQueueFilterDescription.__Native @ref)
        {
            this.Categories = new MessageCategory[@ref.CategorieCount];
            if (@ref.CategorieCount > 0)
                Utilities.Read(@ref.PCategoryList, this.Categories, 0, @ref.CategorieCount);

            this.Severities = new MessageSeverity[@ref.SeveritieCount];
            if (@ref.SeveritieCount > 0)
                Utilities.Read(@ref.PSeverityList, this.Severities, 0, @ref.SeveritieCount);

            this.Ids = new MessageId[@ref.IDCount];
            if (@ref.IDCount > 0)
                Utilities.Read(@ref.PIDList, this.Ids, 0, @ref.IDCount);
        }

        internal unsafe void __MarshalTo(ref InfoQueueFilterDescription.__Native @ref)
        {
            @ref.CategorieCount = this.Categories == null ? 0 : this.Categories.Length;
            if (@ref.CategorieCount > 0)
            {
                @ref.PCategoryList = Marshal.AllocHGlobal(sizeof (MessageCategory)*@ref.CategorieCount);
                Utilities.Write(@ref.PCategoryList, this.Categories, 0, @ref.CategorieCount);
            }
            @ref.SeveritieCount = this.Severities == null ? 0 : this.Severities.Length;
            if (@ref.SeveritieCount > 0)
            {
                @ref.PSeverityList = Marshal.AllocHGlobal(sizeof (MessageSeverity)*@ref.SeveritieCount);
                Utilities.Write(@ref.PSeverityList, this.Severities, 0, @ref.SeveritieCount);
            }
            @ref.IDCount = this.Ids == null? 0 : this.Ids.Length;
            if (@ref.IDCount > 0)
            {
                @ref.PIDList = Marshal.AllocHGlobal(sizeof (MessageId)*@ref.IDCount);
                Utilities.Write(@ref.PIDList, this.Ids, 0, @ref.IDCount);
            }
        }
    }
}
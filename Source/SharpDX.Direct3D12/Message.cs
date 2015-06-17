using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.Direct3D12
{
    public partial struct Message
    {
        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public SharpDX.Direct3D12.MessageCategory Category;
            public SharpDX.Direct3D12.MessageSeverity Severity;
            public SharpDX.Direct3D12.MessageId Id;
            public System.IntPtr PDescription;
            public SharpDX.PointerSize DescriptionByteLength;
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Category = @ref.Category;
            this.Severity = @ref.Severity;
            this.Id = @ref.Id;
            this.Description = (@ref.PDescription == IntPtr.Zero) ? null : Marshal.PtrToStringAnsi(@ref.PDescription, @ref.DescriptionByteLength);
            this.DescriptionByteLength = @ref.DescriptionByteLength;
        }

        public override string ToString()
        {
            return string.Format("[{0}] [{1}] [{2}] : {3}", Id, Severity, Category, Description);
        }
    }
}

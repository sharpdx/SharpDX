using System;
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D12
{
    public partial struct Message
    {
        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public MessageCategory Category;
            public MessageSeverity Severity;
            public MessageId Id;
            public IntPtr PDescription;
            public PointerSize DescriptionByteLength;
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

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("[{0}] [{1}] [{2}] : {3}", Id, Severity, Category, Description);
        }
    }
}

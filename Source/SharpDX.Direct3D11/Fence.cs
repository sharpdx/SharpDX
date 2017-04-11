using System;

namespace SharpDX.Direct3D11
{
    public partial class Fence
    {
        /// <summary>
        ///   Constructs a new <see cref = "T:SharpDX.Direct3D11.Fence" />
        /// </summary>
        /// <param name = "device">The device with which to associate the state object.</param>
        /// <param name="initialValue">The initial value for the fence.</param>
        /// <param name="flags">A combination of FenceFlags values that are combined by using a bitwise OR operation. The resulting value specifies options for the fence.</param>
        /// <returns>The newly created object.</returns>
        public Fence(Device5 device, long initialValue, FenceFlags flags)
            : base(IntPtr.Zero)
        {
            device.CreateFence(initialValue, flags, Utilities.GetGuidFromType(typeof(Fence)), this);
        }
    }
}
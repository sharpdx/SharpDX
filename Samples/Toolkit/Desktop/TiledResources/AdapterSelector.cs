namespace TiledResources
{
    using System;
    using System.Linq;
    using SharpDX.Direct3D11;
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Helper class for selecting appropiate WARP graphics adapter
    /// </summary>
    internal static class AdapterSelector
    {
        /// <summary>
        /// Searches trough all available adapters and returns first WARP that allows creation of the device with needed flags
        /// </summary>
        /// <returns>a WARP adapter</returns>
        internal static GraphicsAdapter FintMatchingWarpAdapter()
        {
            Exception ex = null;

            foreach (var a in GraphicsAdapter.Adapters.Where(x => x.Description.Description == "Microsoft Basic Render Driver"))
            {
                try
                {
                    // try to create a device to make sure it supports Direct2D interop
                    using (GraphicsDevice.New(a, DeviceCreationFlags.BgraSupport | DeviceCreationFlags.Debug))
                    {
                    }

                    return a;
                }
                catch (Exception e)
                {
                    ex = e;
                }
            }

            if (ex != null)
                throw new ApplicationException("Cannot find a suitable graphics adapter!", ex);

            return null;
        }
    }
}
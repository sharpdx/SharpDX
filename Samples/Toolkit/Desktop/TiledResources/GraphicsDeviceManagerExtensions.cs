namespace TiledResources
{
    using SharpDX.Direct3D11;
    using SharpDX.Toolkit;

    /// <summary>
    /// Provide convenient extensions to obtain device-specific information
    /// </summary>
    internal static class GraphicsDeviceManagerExtensions
    {
        /// <summary>
        /// Gets an instance of the DirectX 11.2 device
        /// </summary>
        /// <param name="manager">The current GraphicsDeviceManager</param>
        /// <returns>DirectX 11.2 Device instance</returns>
        public static Device2 GetDevice2(this GraphicsDeviceManager manager)
        {
            return ((Device)manager.GraphicsDevice).QueryInterface<Device2>();
        }

        /// <summary>
        /// Gets an instance of the DirectX 11.2 device context
        /// </summary>
        /// <param name="manager">The current GraphicsDeviceManager</param>
        /// <returns>DirectX 11.2 DeviceContext instance</returns>
        public static DeviceContext2 GetContext2(this GraphicsDeviceManager manager)
        {
            return manager.GetDevice2().ImmediateContext2;
        }
    }
}
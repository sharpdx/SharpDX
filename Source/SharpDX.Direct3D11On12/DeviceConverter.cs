using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.Direct3D11On12
{
    public class DeviceConverter
    {
        /// <summary>
        /// Creates a Direct3D11 Device from its Direct3D12 counterpart
        /// </summary>
        /// <param name="device">Direct3D12 Device</param>
        /// <param name="commandQueues">An array of Command Queues used by the Direct3D12 Device</param>
        /// <param name="flags">The Flags</param>
        /// <param name="featureLevels">the feature Levels</param>
        /// <param name="outDevice">The Direct3D11 Device to be written to</param>
        /// <param name="outDeviceContext">The Direct3D11 Device's Immediate Context</param>
        /// <param name="outFeatureLevel">The FeatureLevel of the created Direct3D11 Device</param>
        public static void CreateD3D11DeviceFromD3D12Device(Direct3D12.Device device, Direct3D12.CommandQueue[] commandQueues,
            Direct3D11.DeviceCreationFlags flags, Direct3D.FeatureLevel[] featureLevels, DXGI.Adapter adapter, out Direct3D11.Device outDevice,
            Direct3D11.DeviceContext outDeviceContext = null, Direct3D.FeatureLevel outFeatureLevel = Direct3D.FeatureLevel.Level_12_1)
        {
            outDevice = new Direct3D11.Device(adapter, flags);
            D3D11On12.D3D11On12CreateDevice(device, flags, featureLevels, featureLevels == null ? 0 : featureLevels.Length, commandQueues, 
                commandQueues.Length, 0, outDevice, out outDeviceContext, out outFeatureLevel);
        }
    }
}

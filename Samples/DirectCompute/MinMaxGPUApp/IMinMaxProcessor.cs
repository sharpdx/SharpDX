using System.Drawing;

using SharpDX;
using SharpDX.Direct3D11;

namespace MinMaxGPUApp
{
    public interface IMinMaxProcessor
    {
        Vector2 MinMaxFactor { get; }

        void Initialize(Device device);

        Size Size { get; set; }

        void GetResults(DeviceContext context, out float min, out float max);

        void Reduce(DeviceContext context, ShaderResourceView from);
    }
}
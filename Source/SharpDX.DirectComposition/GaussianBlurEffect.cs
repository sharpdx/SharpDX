using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.DirectComposition
{
    public partial class GaussianBlurEffect
    {
        public GaussianBlurEffect(Device3 device) : base(IntPtr.Zero)
        {
            device.CreateGaussianBlurEffect(this);
        }
    }
}

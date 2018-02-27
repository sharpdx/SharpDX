using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.DirectComposition
{
    public partial class BrightnessEffect
    {
        public BrightnessEffect(Device3 device) : base(IntPtr.Zero)
        {
            device.CreateBrightnessEffect(this);
        }
    }
}

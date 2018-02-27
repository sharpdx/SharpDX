using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.DirectComposition
{
    public partial class ColorMatrixEffect
    {
        public ColorMatrixEffect(Device3 device) : base(IntPtr.Zero)
        {
            device.CreateColorMatrixEffect(this);
        }
    }
}

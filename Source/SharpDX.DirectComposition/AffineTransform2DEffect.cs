using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.DirectComposition
{
    public partial class AffineTransform2DEffect
    {
        public AffineTransform2DEffect(Device3 device) : base(IntPtr.Zero)
        {
            device.CreateAffineTransform2DEffect(this);
        }
    }
}

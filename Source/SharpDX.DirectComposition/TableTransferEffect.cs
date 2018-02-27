using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.DirectComposition
{
    public partial class TableTransferEffect
    {
        public TableTransferEffect(Device3 device) : base(IntPtr.Zero)
        {
            device.CreateTableTransferEffect(this);
        }
    }
}

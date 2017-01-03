#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class CaptureSink
    {
        T GetService<T>(int sinkStreamIndex, Guid serviceGuid)
            where T : ComObject
        {
            return (T)GetService(sinkStreamIndex, serviceGuid, Utilities.GetGuidFromType(typeof(T)));
        }
    }
}

#endif
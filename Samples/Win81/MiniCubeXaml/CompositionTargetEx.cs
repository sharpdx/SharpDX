using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Media;

namespace MiniCubeXaml
{
    public static class CompositionTargetEx { 
        private static TimeSpan _last = TimeSpan.Zero; 
        private static event EventHandler<RenderingEventArgs> _FrameUpdating; 
        public static event EventHandler<RenderingEventArgs> Rendering { 
            add { 
                if (_FrameUpdating == null)                 
                    CompositionTarget.Rendering += CompositionTarget_Rendering;
                _FrameUpdating += value; 
            } 
            remove { 
                _FrameUpdating -= value; 
                if (_FrameUpdating == null)                
                    CompositionTarget.Rendering -= CompositionTarget_Rendering; 
            } 
        } 
        static void CompositionTarget_Rendering(object sender, object e) { 
            RenderingEventArgs args = (RenderingEventArgs)e;
            if (args.RenderingTime == _last) 
                return;
            _last = args.RenderingTime; _FrameUpdating(sender, args); 
        } 
    }  
}

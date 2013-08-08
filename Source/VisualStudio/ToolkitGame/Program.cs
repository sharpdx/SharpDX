using System;
using System.Collections.Generic;

namespace $safeprojectname$
{
    /// <summary>
    /// Simple $safeclassname$ application using SharpDX.Toolkit.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
#if NETFX_CORE
        [MTAThread]
#else
        [STAThread]
#endif
        static void Main()
        {
$if$ ($sharpdx_platform_winrt_xaml$ == true)            global::Windows.UI.Xaml.Application.Start((p) => new App());
$else$            using (var program = new $safeclassname$())
                program.Run();
$endif$
        }
    }
}
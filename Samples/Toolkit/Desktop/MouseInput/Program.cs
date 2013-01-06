using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseInput.Desktop
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using(var game = new MouseInputGame())
                game.Run();
        }
    }
}

using System;

namespace MiniCube.SwitchContext.Desktop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using(var game = new MiniCubeGame())
            {
                var form = new Form1(game);
                form.Show(); // make sure the form is displayed
                form.RunGame(); // run the game
            }
        }
    }
}

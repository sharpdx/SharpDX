namespace ShaderLinking
{
    using System;
    using System.Windows.Forms;

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using(var game = new ShaderCompilerGame())
            {
                var form = new Form1(game.Data);
                form.Show();

                game.IsMouseVisible = true;
                game.Run(form.RenderControl);
            }
        }
    }
}

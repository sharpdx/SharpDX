namespace PointerInput.Desktop
{
    static class Program
    {
        [System.STAThread]
        static void Main()
        {
            using(var game = new PointerInputGame())
                game.Run();
        }
    }
}

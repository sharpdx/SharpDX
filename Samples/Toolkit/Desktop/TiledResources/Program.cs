namespace TiledResources
{
    static class Program
    {
        [System.STAThread]
        static void Main()
        {
            using (var game = new SampleGame())
                game.Run();
        }
    }
}

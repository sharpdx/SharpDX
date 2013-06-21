namespace MonitorInfo.Desktop
{
    static class Program
    {
        [System.STAThread]
        static void Main()
        {
            using (var game = new MonitorInfoGame())
                game.Run();
        }
    }
}

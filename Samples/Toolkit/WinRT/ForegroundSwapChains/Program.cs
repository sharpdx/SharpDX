namespace ForegroundSwapChains
{
    static class Program
    {
        static void Main()
        {
            using (var game = new ForegroundSwapChainGame())
                game.Run();
        }
    }
}

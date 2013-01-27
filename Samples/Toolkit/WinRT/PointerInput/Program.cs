namespace PointerInput.WinRT
{
    using System;

    class Program
    {
        [MTAThread]
        public static void Main()
        {
            using (var game = new PointerInputGame())
                game.Run();
        }
    }
}
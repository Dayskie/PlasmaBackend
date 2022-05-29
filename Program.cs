using System;
using PlasmaBackend;

namespace CustomLauncherTest
{
    public static class Program
    {
        private static void Main()
        {
            ParseAsf.ParseFiles();
            foreach (Game game in CreateGame.Games)
            {
                CreateGame.GameInfo(game.Name);
                Console.WriteLine("_________________________");
            }
            if (ParseAsf.DirCount == 0)
            {
                Console.WriteLine("If the terminal is empty, check the 'Resources' folder!");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Enter a game you wish to launch");
                CreateGame.LaunchGame(Console.ReadLine());
            }
        }
    }
}
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32;

namespace PlasmaBackend
{
  public static class CreateGame
  {
    public static List<Game> Games = new List<Game>();
    private static string absPath = Directory.GetCurrentDirectory() + "\\Resources\\";

    private static string CleanString(string s)
    {
      StringBuilder stringBuilder = new StringBuilder(s);
      stringBuilder.Replace(":", "");
      stringBuilder.Replace("™", "");
      stringBuilder.Replace("®", "");
      return stringBuilder.ToString();
    }

    public static void ConstructGame()
    {
      for (int fileNum = 0; fileNum != ParseAsf.AcfCount; ++fileNum)
      {
        Game game = new Game()
        {
          Name = ParseAsf.FileData("name", fileNum),
          Appid = ParseAsf.FileData("appid", fileNum),
          SizeOnDisk = long.Parse(ParseAsf.FileData("SizeOnDisk", fileNum))
        };
        game.CoverArt = SetCoverArt(game.Name);
        CreateGame.Games.Add(game);
      }
    }

    private static string SetCoverArt(string name)
    {
      string path = CreateGame.absPath + "CoverArt\\" + CreateGame.CleanString(name);
      if (!Directory.Exists(path))
      {
        try
        {
          Directory.CreateDirectory(path);
        }
        catch
        {
          Console.WriteLine("Unable to create folder " + path);
          return CreateGame.absPath + "CoverArt\\Default";
        }
      }
      return path;
    }

    public static void LaunchGame(string name)
    {
      object obj = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath", (object) null);
      Game game = CreateGame.Games.Find((Predicate<Game>) (x => x.Name.Contains(name)));
      try
      {
        Process.Start(obj?.ToString() + "\\steam.exe", "steam://rungameid/" + game.Appid);
        Console.WriteLine("Launching...");
        Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        switch (ex)
        {
          case NullReferenceException _:
            Console.WriteLine("Invalid game name. Unable to launch unknown game " + name + ".");
            break;
          case Win32Exception _:
            Console.WriteLine("Unable to find Steam.");
            break;
          default:
            Console.WriteLine((object) ex);
            break;
        }
        Console.ReadLine();
      }
    }

    public static void GameInfo(string name)
    {
      Game game = CreateGame.Games.Find((Predicate<Game>) (x => x.Name.Contains(name)));
      try
      {
        Console.WriteLine("Name: " + game.Name);
        Console.WriteLine("appid: " + game.Appid);
        Console.WriteLine($"Size on Disk: {(Math.Round(game.SizeOnDisk / 1e9, 2))}GB");
        Console.WriteLine("Cover art directory: " + game.CoverArt);
      }
      catch
      {
        Console.WriteLine("Unable to collect file information.");
      }
    }
  }
}

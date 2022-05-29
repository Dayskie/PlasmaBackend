using System.Text.RegularExpressions;

namespace PlasmaBackend
{
    public class ParseAsf
    {
        private static List<string> acfFiles = new List<string>();
        public static int DirCount;

        public static int AcfCount => ParseAsf.acfFiles.Count;

        public static void ParseFiles()
        {
            foreach (string readAllLine in File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Resources\\sdir.txt"))
            {
                if (Directory.Exists(readAllLine))
                {
                    foreach (string file in Directory.GetFiles(readAllLine))
                    {
                        if (Path.GetExtension(file) == ".acf")
                            ParseAsf.acfFiles.Add(file);
                        ++ParseAsf.DirCount;
                    }
                }
            }
            CreateGame.ConstructGame();
        }

        public static string FileData(string dataRequest, int fileNum)
        {
            foreach (string readAllLine in File.ReadAllLines(ParseAsf.acfFiles[fileNum] ?? ""))
            {
                if (readAllLine.Contains(dataRequest))
                    return Regex.Replace(readAllLine, "^.*(\\s{2})|\\\"", "");
            }
            return "Specified data not found!";
        }
    }
}
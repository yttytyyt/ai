using DataLayer;
using Models;
using Models.DoorDir;
using Models.ItemDir;
using System.Drawing;
using System.Numerics;
using ConsoleIO;
using Models.Convert;

namespace Main
{
    public class Program
    {
        public static IOConsole io = new IOConsole();

        static void Main(string[] args)
        {
            string path = "./TempleOfDoom_Extended_A_2122.json";

            var factoryReader = new ReaderFactory();
            IDataReader reader = factoryReader.GetReader(path);

            var gameConverter = new DataToGameConverter();
            var game = gameConverter.CreateGame(reader.ReadLevelData(path));
            
            io.WriteLine($"Using level {path.Substring(path.LastIndexOf('/') + 1)}.");
            io.WriteLine($"press any key to start!");
            io.ReadKey();
            io.DrawGame(game);
        }
    }
}

using System;
using System.IO;
using System.Text.Json;

namespace DataLayer
{
    public class JsonDataReader : IDataReader
    {
        public GameData ReadLevelData(string path)
        {
            GameData gameData;
            try
            {
                string jsonString = File.ReadAllText(path);
                gameData = JsonSerializer.Deserialize<GameData>(jsonString);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("File not found");
            }
            catch (JsonException)
            {
                throw new JsonException("Error deserializing JSON");
            }

            return gameData;
        }
    }
}

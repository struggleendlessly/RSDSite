using Newtonsoft.Json;

namespace shared.Helpers
{
    public class JsonFileManager
    {
        public static T ReadFromJsonFile<T>(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (T) serializer.Deserialize(file, typeof(T));
            }
        }

        public static void WriteToJsonFile<T>(T data, string filePath)
        {
            using (StreamWriter file = File.CreateText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }
        }
    }
}

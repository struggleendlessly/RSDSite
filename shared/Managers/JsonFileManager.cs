using Newtonsoft.Json;

namespace shared.Managers
{
    public class JsonFileManager
    {
        public static T ReadFromJsonFile<T>(string webRootPath, string jsonPath)
        {
            var filePath = Path.Combine(webRootPath, jsonPath);

            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (T) serializer.Deserialize(file, typeof(T));
            }
        }

        public static void WriteToJsonFile<T>(T data, string webRootPath, string jsonPath)
        {
            var filePath = Path.Combine(webRootPath, jsonPath);

            using (StreamWriter file = File.CreateText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }
        }
    }
}

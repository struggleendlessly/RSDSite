using Newtonsoft.Json;

namespace shared.Helpers
{
    public class JsonFileManager
    {
        public static object ReadFromJsonFile(Type modelType, string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize(file, modelType);
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

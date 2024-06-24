using Newtonsoft.Json;

namespace shared.Helpers
{
    public static class ObjectHelpers
    {
        public static T DeepCopy<T>(T model)
        {
            var json = JsonConvert.SerializeObject(model);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

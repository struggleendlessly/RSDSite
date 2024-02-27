using System;

namespace shared.Interfaces
{
    public interface IFileManager
    {
        T ReadFromJsonFile<T>(string webRootPath, string jsonPath);
        void WriteToJsonFile<T>(T data, string webRootPath, string jsonPath);
    }
}

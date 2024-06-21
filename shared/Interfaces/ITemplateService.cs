using System;

namespace shared.Interfaces
{
    public interface ITemplateService
    {
        Task<string> GetTemplateHtmlAsync(string templateFilePath, string templateMemoryCacheKey, Dictionary<string, string> placeholders);
    }
}

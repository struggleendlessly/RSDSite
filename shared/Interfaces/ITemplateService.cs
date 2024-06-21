using System;

namespace shared.Interfaces
{
    public interface ITemplateService
    {
        Task<string> GetTemplateHtmlAsync(string templateName, Dictionary<string, string> placeholders);
    }
}

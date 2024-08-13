using shared.Interfaces;

namespace shared.Managers
{
    public class TemplateService : ITemplateService
    {
        private readonly IPageDataService _pageDataService;

        public TemplateService(IPageDataService pageDataService)
        {
            _pageDataService = pageDataService;
        }

        public async Task<string> GetTemplateHtmlAsync(string templateFilePath, string templateMemoryCacheKey, Dictionary<string, string> placeholders)
        {
            //var templateContent = await _pageDataService.GetStringDataAsync(templateMemoryCacheKey, templateFilePath, StaticStrings.EmailTemplatesContainerName);
            var templateContent = string.Empty;

            foreach (var placeholder in placeholders)
            {
                templateContent = templateContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }

            return templateContent;
        }
    }
}

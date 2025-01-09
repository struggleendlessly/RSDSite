using shared.Interfaces;

namespace shared.Managers
{
    public class TemplateService : ITemplateService
    {
        private readonly IPageDataService _pageDataService;
        private readonly IStateManager _stateManager;

        public TemplateService(IPageDataService pageDataService, IStateManager stateManager)
        {
            _pageDataService = pageDataService;
            _stateManager = stateManager;
        }

        public async Task<string> GetTemplateHtmlAsync(string templateFilePath, string templateMemoryCacheKey, Dictionary<string, string> placeholders)
        {
            var templateContent = await _pageDataService.GetStringDataAsync(templateMemoryCacheKey, _stateManager.SiteName, _stateManager.Lang.TwoLetterISOLanguageName, templateFilePath, StaticStrings.EmailTemplatesContainerName);

            foreach (var placeholder in placeholders)
            {
                templateContent = templateContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }

            return templateContent;
        }
    }
}

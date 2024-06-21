using shared.Interfaces;

namespace shared.Managers
{
    public class TemplateService : ITemplateService
    {
        private readonly IStateManager _stateManager;

        public TemplateService(IStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task<string> GetTemplateHtmlAsync(string templateName, Dictionary<string, string> placeholders)
        {
            var path = Path.Combine(StaticStrings.WwwRootPath, StaticStrings.EmailTemplatesFolder, _stateManager.Lang.TwoLetterISOLanguageName, templateName);
            var templateContent = await File.ReadAllTextAsync(path);

            foreach (var placeholder in placeholders)
            {
                templateContent = templateContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }

            return templateContent;
        }
    }
}

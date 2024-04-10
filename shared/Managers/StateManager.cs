using Microsoft.Extensions.Options;

using shared.Interfaces;
using shared.ConfigurationOptions;

namespace shared.Managers
{
    public class StateManager : IStateManager
    {
        private readonly List<string> _mainSiteOwnersEmails;

        public StateManager(IOptions<MainSiteOwnersOptions> _mainSiteOwnersOptions)
        {
            _mainSiteOwnersEmails = _mainSiteOwnersOptions.Value.Emails;
        }

        private string _siteName;
        private string _lang;

        public string SiteName
        {
            get => _siteName;
            set => _siteName = string.IsNullOrWhiteSpace(value) ? StaticStrings.DefaultSiteName : value.ToLower();
        }

        public string Lang
        {
            get => _lang;
            set => _lang = string.IsNullOrWhiteSpace(value) ? StaticStrings.DefaultEnLang : value.ToLower();
        }

        public string UserEmail { get; set; }
        public List<string> UserSites { get; set; } = new List<string>();

        public bool CanEditSite()
        {
            bool isUserSiteOwner = UserSites.Contains(SiteName);
            bool isMainSiteOwner = SiteName == StaticStrings.DefaultSiteName && _mainSiteOwnersEmails.Contains(UserEmail);

            return isUserSiteOwner || isMainSiteOwner;
        }
    }
}

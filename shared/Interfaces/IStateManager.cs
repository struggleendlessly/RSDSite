using System;

namespace shared.Interfaces
{
    public interface IStateManager
    {
        public string SiteName { get; }
        public string Lang { get; }

        public string UserId { get; }
        public string UserEmail { get; }
        public List<string> UserSites { get; }

        bool CanEditSite();
        string GetPageUrl(string url, bool showSiteName = true);
        string GetCurrentUrlWithLanguage(string language);
    }
}

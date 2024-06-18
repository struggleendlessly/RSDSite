using shared.Data.Entities;
using System.Globalization;

namespace shared.Interfaces
{
    public interface IStateManager
    {
        public string SiteName { get; }
        public CultureInfo Lang { get; }

        public string UserId { get; }
        public Guid? SiteId { get; }
        public List<string> UserSites { get; }

        bool CanEditSite();
        void AddUserSite(Website website);
        void RenameUserSite(Guid siteId, string newName);
        string GetPageUrl(string url, bool showSiteName = true);
        string GetCurrentUrlWithLanguage(string language);
    }
}

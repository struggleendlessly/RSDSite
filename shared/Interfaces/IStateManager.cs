using shared.Data.Entities;
using System.Globalization;

namespace shared.Interfaces
{
    public interface IStateManager
    {
        public string SiteName { get; }
        public CultureInfo Lang { get; }

        public User User { get; set; }
        public Guid? SiteId { get; }
        public List<Website> UserWebsites { get; set; }

        bool CanEditSite();
        bool IsCustomDomain();
        void AddUserSite(Website website);
        void RenameUserSite(Guid siteId, string newName);
        string GetPageUrl(string url, bool showSiteName = true);
        string GetCurrentUrlWithLanguage(string language);
    }
}

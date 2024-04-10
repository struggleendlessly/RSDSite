using System;

namespace shared.Interfaces
{
    public interface IStateManager
    {
        public string SiteName { get; set; }
        public string Lang { get; set; }

        public string UserEmail { get; set; }
        public List<string> UserSites { get; set; }

        bool CanEditSite();
    }
}

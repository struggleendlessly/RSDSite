using shared.Interfaces;

namespace shared.Managers
{
    public class StateManager : IStateManager
    {
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
    }
}

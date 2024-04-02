using shared.Interfaces;

namespace shared.Managers
{
    public class StateManager : IStateManager
    {
        private string _siteName;

        public string SiteName
        {
            get => _siteName;
            set => _siteName = string.IsNullOrWhiteSpace(value) ? StaticStrings.DefaultSiteName : value.ToLower();
        }
    }
}

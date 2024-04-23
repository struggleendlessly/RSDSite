using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using shared.Interfaces;
using shared.ConfigurationOptions;

using System.Security.Claims;

namespace shared.Managers
{
    public class StateManager : IStateManager
    {
        private readonly NavigationManager _navigationManager;

        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IWebsiteService _websiteService;

        private readonly List<string> _mainSiteOwnersEmails;
        private readonly ClaimsPrincipal _user;

        public StateManager(
            IOptions<MainSiteOwnersOptions> _mainSiteOwnersOptions, 
            NavigationManager navigationManager, 
            AuthenticationStateProvider authenticationStateProvider,
            IWebsiteService websiteService)
        {
            _mainSiteOwnersEmails = _mainSiteOwnersOptions.Value.Emails;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
            _websiteService = websiteService;

            var authState = _authenticationStateProvider.GetAuthenticationStateAsync().Result;
            _user = authState.User;
        }

        public string SiteName
        {
            get
            {
                var baseRelativePath = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
                var parts = baseRelativePath.Split('/');
                return parts.Length >= 1 && !string.IsNullOrWhiteSpace(parts[0]) ? parts[0] : StaticStrings.DefaultSiteName;
            }
        }

        public string Lang
        {
            get
            {
                var baseRelativePath = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
                var parts = baseRelativePath.Split('/');
                return parts.Length >= 2 && !string.IsNullOrWhiteSpace(parts[1]) ? parts[1] : StaticStrings.DefaultEnLang;
            }
        }

        public string UserId
        {
            get
            {
                if (_user.Identity is not null && _user.Identity.IsAuthenticated)
                {
                    return _user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        public string UserEmail 
        {
            get
            {
                if (_user.Identity is not null && _user.Identity.IsAuthenticated)
                {
                    return _user.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<string> UserSites 
        {
            get
            {
                if (_user.Identity is not null && _user.Identity.IsAuthenticated)
                {
                    return _websiteService.GetUserSitesAsync(UserId).Result;
                }
                else
                {
                    return new List<string>();
                }
            }
        }

        public bool CanEditSite()
        {
            bool isUserSiteOwner = UserSites.Contains(SiteName);
            bool isMainSiteOwner = SiteName == StaticStrings.DefaultSiteName && _mainSiteOwnersEmails.Contains(UserEmail);

            return isUserSiteOwner || isMainSiteOwner;
        }
    }
}

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using shared.Interfaces;
using shared.Data.Entities;
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
        private List<Website> _userSites;
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

            _userSites = new List<Website>();

            var authState = _authenticationStateProvider.GetAuthenticationStateAsync().Result;
            _user = authState.User;
        }

        public string SiteName
        {
            get
            {
                var domain = GetDomainWithoutProtocol();
                var baseRelativePath = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
                var parts = baseRelativePath.Split('/');

                if (domain == StaticStrings.DefaultDomain || domain == StaticStrings.DefaultDevDomain || domain == StaticStrings.DefaultLocalDomain)
                {
                    if (parts.Length >= 1 && !string.IsNullOrWhiteSpace(parts[0]) && parts[0] != StaticStrings.DefaultEnLang && parts[0] != StaticStrings.DefaultUaLang)
                    {
                        return parts[0];
                    }
                    else
                    {
                        return StaticStrings.DefaultSiteName;
                    }
                }
                else
                {
                    return domain;
                }
            }
        }

        public string Lang
        {
            get
            {
                var domain = GetDomainWithoutProtocol();
                var baseRelativePath = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
                var parts = baseRelativePath.Split('/');

                if (domain == StaticStrings.DefaultDomain || domain == StaticStrings.DefaultDevDomain || domain == StaticStrings.DefaultLocalDomain)
                {
                    if (parts.Length >= 1 && !string.IsNullOrWhiteSpace(parts[0]) && (parts[0] == StaticStrings.DefaultEnLang || parts[0] == StaticStrings.DefaultUaLang))
                    {
                        return parts[0];
                    }
                    else if (parts.Length >= 2 && !string.IsNullOrWhiteSpace(parts[1]) && (parts[1] == StaticStrings.DefaultEnLang || parts[1] == StaticStrings.DefaultUaLang))
                    {
                        return parts[1];
                    }
                    else
                    {
                        return StaticStrings.DefaultEnLang;
                    }
                }
                else
                {
                    return parts.Length >= 1 && !string.IsNullOrWhiteSpace(parts[0]) ? parts[0] : StaticStrings.DefaultEnLang;
                }
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

        public Guid? SiteId
        {
            get
            {
                return _userSites.FirstOrDefault(x => x.Name == SiteName)?.Id;
            }
        }

        public List<string> UserSites 
        {
            get
            {
                if (_user.Identity is null || !_user.Identity.IsAuthenticated)
                {
                    return new List<string>();
                }

                if (_userSites.Count == 0)
                {
                    _userSites = _websiteService.GetUserSites(UserId);

                    //if (_mainSiteOwnersEmails.Contains(UserEmail))
                    //{
                    //    _userSites.Add(StaticStrings.DefaultSiteName);
                    //}
                }

                return _userSites.Select(x => x.Name).ToList();
            }
        }

        public bool CanEditSite()
        {
            return UserSites.Contains(SiteName);
        }

        public void AddUserSite(Website website)
        {
            _userSites.Add(website);
        }

        public void RenameUserSite(Guid siteId, string newName)
        {
            foreach (var site in _userSites)
            {
                if (site.Id == siteId)
                {
                    site.Name = newName;
                }
            }
        }

        public string GetPageUrl(string url, bool showSiteName = true)
        {
            var domain = GetDomainWithoutProtocol();
            if ((domain == StaticStrings.DefaultDomain || domain == StaticStrings.DefaultDevDomain || domain == StaticStrings.DefaultLocalDomain) && showSiteName)
            {
                return $"{SiteName}/{Lang}/{url}";
            }
            else
            {
                return $"{Lang}/{url}";
            }
        }

        public string GetCurrentUrlWithLanguage(string language)
        {
            var currentUrl = _navigationManager.Uri;
            var domain = GetDomainWithoutProtocol();
            var siteAndLang = string.Empty;
            var newSiteAndLang = string.Empty;
            if (domain == StaticStrings.DefaultDomain || domain == StaticStrings.DefaultDevDomain || domain == StaticStrings.DefaultLocalDomain)
            {
                siteAndLang = $"{SiteName}/{Lang}";
                newSiteAndLang = $"{SiteName}/{language}";
            }
            else
            {
                siteAndLang = $"{Lang}";
                newSiteAndLang = $"{language}";
            }

            if (currentUrl.Contains(siteAndLang))
            {
                currentUrl = currentUrl.Replace(siteAndLang, newSiteAndLang);
            }
            else
            {
                currentUrl += newSiteAndLang;
            }

            return currentUrl;
        }

        private string GetDomainWithoutProtocol()
        {
            var domainWithProtocol = _navigationManager.BaseUri;
            var uri = new Uri(domainWithProtocol);
            var domainWithoutProtocol = uri.Host;
            var domainParts = domainWithoutProtocol.Split('.');

            var topLevelDomain = domainParts.LastOrDefault();
            if (topLevelDomain != null && domainParts.Length >= 2)
            {
                var domainWithoutTLD = domainWithoutProtocol.Substring(0, domainWithoutProtocol.Length - topLevelDomain.Length - 1);
                return domainWithoutTLD;
            }

            return domainWithoutProtocol;
        }
    }
}

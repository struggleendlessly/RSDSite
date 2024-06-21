using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using shared.Interfaces;
using shared.Data.Entities;

using System.Globalization;
using System.Security.Claims;

namespace shared.Managers
{
    public class StateManager : IStateManager
    {
        private readonly NavigationManager _navigationManager;

        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IWebsiteService _websiteService;

        private List<Website> _userSites;
        private Dictionary<string, string> _userDomains;
        private readonly ClaimsPrincipal _user;

        public StateManager(
            NavigationManager navigationManager, 
            AuthenticationStateProvider authenticationStateProvider,
            IWebsiteService websiteService)
        {
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
            _websiteService = websiteService;

            _userSites = new List<Website>();
            _userDomains = new Dictionary<string, string>();

            var authState = _authenticationStateProvider.GetAuthenticationStateAsync().Result;
            _user = authState.User;
        }

        public string SiteName
        {
            get
            {
                var siteName = StaticStrings.MainSiteName;
                var domain = GetDomain();
                if (StaticStrings.Domains.Contains(domain))
                {
                    var segments = GetUriSegments();
                    if (segments.Length > 1)
                    {
                        var cleanedFirstSegment = segments[1].Trim('/');
                        if (!string.IsNullOrWhiteSpace(cleanedFirstSegment) && !IsSupportedLanguage(cleanedFirstSegment))
                        {
                            siteName = cleanedFirstSegment;
                        }
                    }
                }
                else
                {
                    var domainWithTld = GetDomain(true);
                    if (_userDomains.ContainsKey(domainWithTld))
                    {
                        return _userDomains[domainWithTld];
                    }
                    
                    siteName = _websiteService.GetWebsiteName(domainWithTld);
                    _userDomains[domainWithTld] = siteName;
                }

                return siteName;
            }
        }

        public CultureInfo Lang
        {
            get
            {
                var segments = GetUriSegments();
                var languageCode = StaticStrings.EnLanguageCode;

                foreach (var segment in segments)
                {
                    var cleanedSegment = segment.Trim('/');
                    if (!string.IsNullOrWhiteSpace(cleanedSegment) && IsSupportedLanguage(cleanedSegment))
                    {
                        languageCode = cleanedSegment;
                        break;
                    }
                }

                return GetCultureInfo(languageCode);
            }
        }

        private string[] GetUriSegments()
        {
            var uri = new Uri(_navigationManager.Uri);
            return uri.Segments;
        }

        private bool IsSupportedLanguage(string languageCode)
        {
            return StaticStrings.Languages.Any(lang => lang.TwoLetterISOLanguageName.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
        }

        private CultureInfo GetCultureInfo(string languageCode)
        {
            var culture = StaticStrings.Languages.FirstOrDefault(lang => lang.TwoLetterISOLanguageName.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
            return culture ?? StaticStrings.DefaultLanguage;
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
                }

                return _userSites.Select(x => x.Name).ToList();
            }
        }

        public bool CanEditSite()
        {
            return UserSites.Contains(SiteName);
        }

        public bool IsCustomDomain()
        {
            var domain = GetDomain();
            return !StaticStrings.Domains.Contains(domain);
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
            var domain = GetDomain();
            var languageSegment = Lang.TwoLetterISOLanguageName;
            var siteNameSegment = showSiteName && StaticStrings.Domains.Contains(domain) ? SiteName : string.Empty;

            return string.IsNullOrEmpty(siteNameSegment)
                ? $"{languageSegment}/{url}"
                : $"{siteNameSegment}/{languageSegment}/{url}";
        }

        public string GetCurrentUrlWithLanguage(string language)
        {
            var domain = GetDomain();
            var baseRelativePath = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
            var languageSegment = Lang.TwoLetterISOLanguageName;
            if (StaticStrings.Languages.Any(x => baseRelativePath.Contains(x.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase)))
            {
                return baseRelativePath.Replace(languageSegment, language);
            }

            return StaticStrings.Domains.Contains(domain)
                ? $"{baseRelativePath}/{SiteName}/{language}"
                : $"{baseRelativePath}/{language}";
        }

        private string GetDomain(bool includeTld = false)
        {
            var uri = new Uri(_navigationManager.BaseUri);
            var domain = uri.Host;

            if (!includeTld)
            {
                var domainParts = domain.Split('.');
                if (domainParts.Length > 1)
                {
                    domain = string.Join('.', domainParts.Take(domainParts.Length - 1));
                }
            }

            return domain;
        }
    }
}

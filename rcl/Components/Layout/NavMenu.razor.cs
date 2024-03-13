using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

using shared;

namespace rcl.Components.Layout
{
    public partial class NavMenu : IDisposable
    {
        private string? currentUrl;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        public string SiteName { get; set; } = string.Empty;

        private string[] Pages { get; set; } = StaticRoutesStrings.GetPagesRoutes();

        protected override void OnInitialized()
        {
            currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            SetSiteName(currentUrl);

            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
            SetSiteName(currentUrl);

            StateHasChanged();
        }

        private string HighlightActiveMenuItem(string url)
        {
            if (url.StartsWith("/"))
            {
                url = url.Substring(1);
            }

            if (currentUrl != url)
            {
                return string.Empty;
            }

            return StaticHtmlStrings.СSSActiveClass;
        }

        private void SetSiteName(string baseRelativePath)
        {
            if (Pages.Contains(baseRelativePath)) 
            {  
                SiteName = string.Empty; 
            }
            else
            {
                string[] parts = baseRelativePath.Split('/');
                SiteName = parts.Length > 0 ? parts[0] : string.Empty;
            }          
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}

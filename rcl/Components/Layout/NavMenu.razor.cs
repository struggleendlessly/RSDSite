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

        protected override void OnInitialized()
        {
            currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
            StateHasChanged();
        }

        private string HighlightActiveMenuItem(string url)
        {
            url = url.Replace("/", "");

            if (currentUrl != url)
            {
                return string.Empty;
            }

            return StaticHtmlStrings.СSSActiveClass;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}

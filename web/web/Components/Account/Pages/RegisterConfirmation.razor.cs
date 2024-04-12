using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using shared.Data.Entities;

namespace web.Components.Account.Pages
{
    public partial class RegisterConfirmation
    {
        [Parameter]
        public string SiteName { get; set; }

        [Parameter]
        public string Lang { get; set; }

        private string? statusMessage;

        [Inject]
        UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        IdentityRedirectManager RedirectManager { get; set; }

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        [SupplyParameterFromQuery]
        private string? Email { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Email is null)
            {
                RedirectManager.RedirectTo("");
            }

            var user = await UserManager.FindByEmailAsync(Email);
            if (user is null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                statusMessage = "Error finding user for unspecified email";
            }
        }
    }
}

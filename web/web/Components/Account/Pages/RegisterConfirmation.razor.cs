using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;
using shared.Data.Entities;

namespace web.Components.Account.Pages
{
    public partial class RegisterConfirmation
    {
        [Parameter]
        public string Lang { get; set; } = string.Empty;

        private string? statusMessage;

        [Inject]
        UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        IdentityRedirectManager RedirectManager { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        [SupplyParameterFromQuery]
        private string? Email { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Email is null)
            {
                RedirectManager.RedirectTo(StaticRoutesStrings.EmptyRoute);
            }

            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);

            var user = await UserManager.FindByEmailAsync(Email);
            if (user is null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                statusMessage = LocalizationModel.Data[StaticStrings.Localization_Account_RegisterConfirmation_Message_ErrorFindingUser_Key];
            }
        }
    }
}

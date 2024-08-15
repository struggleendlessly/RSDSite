using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

using shared;
using shared.Models;
using shared.Interfaces;
using shared.Data.Entities;

namespace web.Components.Account.Pages
{
    public partial class ConfirmEmail
    {
        [Parameter]
        public string Lang { get; set; } = string.Empty;

        [Inject]
        UserManager<User> UserManager { get; set; }

        [Inject] 
        IdentityRedirectManager RedirectManager { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IEmailSender<User> EmailSender { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        private string? statusMessage;

        private bool ShowEmailResendLink { get; set; } = false;

        private bool DisableEmailResendLink { get; set; } = false;

        private User? User { get; set; }

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        [SupplyParameterFromQuery]
        private string? UserId { get; set; }

        [SupplyParameterFromQuery]
        private string? Code { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (UserId is null || Code is null)
            {
                RedirectManager.RedirectTo(StaticRoutesStrings.EmptyRoute);
            }

            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);

            User = await UserManager.FindByIdAsync(UserId);
            if (User is null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                statusMessage = $"{LocalizationModel.Data[StaticStrings.Localization_Account_ConfirmEmail_Message_ErrorLoadingUser_Key]} {UserId}";
            }
            else
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
                var result = await UserManager.ConfirmEmailAsync(User, code);
                if (result.Succeeded)
                {
                    statusMessage = LocalizationModel.Data[StaticStrings.Localization_Account_ConfirmEmail_Message_ConfirmingSuccess_Key];
                }
                else
                {
                    statusMessage = LocalizationModel.Data[StaticStrings.Localization_Account_ConfirmEmail_Message_ErrorConfirmingEmail_Key];
                    ShowEmailResendLink = true;
                }
            }
        }

        public async Task ResendConfirmationEmailAsync()
        {
            DisableEmailResendLink = true;

            var code = await UserManager.GenerateEmailConfirmationTokenAsync(User);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = NavigationManager.GetUriWithQueryParameters(
                NavigationManager.ToAbsoluteUri(StateManager.GetPageUrl(StaticRoutesStrings.AccountConfirmEmailPageUrl, false)).AbsoluteUri,
                new Dictionary<string, object?> { ["userId"] = User.Id, ["code"] = code });

            await EmailSender.SendConfirmationLinkAsync(User, User.Email, HtmlEncoder.Default.Encode(callbackUrl));

            ShowEmailResendLink = false;
            statusMessage = LocalizationModel.Data[StaticStrings.Localization_Account_ConfirmEmail_Message_CheckEmailToConfirm_Key];

            StateHasChanged();
        }
    }
}

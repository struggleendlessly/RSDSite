using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity;
using shared.Data.Entities;
using shared.Interfaces;
using shared;

namespace web.Components.Account.Pages
{
    public partial class ForgotPassword
    {
        [Parameter]
        public string SiteName { get; set; }

        [Parameter]
        public string Lang {  get; set; }

        [Inject]
        UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        IEmailSender<ApplicationUser> EmailSender { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IdentityRedirectManager RedirectManager { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new();

        private async Task OnValidSubmitAsync()
        {
            var user = await UserManager.FindByEmailAsync(Input.Email);
            if (user is null || !(await UserManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                RedirectManager.RedirectTo(GetPageUrl(StaticRoutesStrings.AccountForgotPasswordConfirmationPageUrl));
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await UserManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = NavigationManager.GetUriWithQueryParameters(
                NavigationManager.ToAbsoluteUri("Account/ResetPassword").AbsoluteUri,
                new Dictionary<string, object?> { ["code"] = code });

            await EmailSender.SendPasswordResetLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

            RedirectManager.RedirectTo(GetPageUrl(StaticRoutesStrings.AccountForgotPasswordConfirmationPageUrl));
        }

        public string GetPageUrl(string url)
        {
            return $"{StateManager.SiteName}/{StateManager.Lang}/{url}";
        }

        private sealed class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

using System.Text;
using System.ComponentModel.DataAnnotations;

using shared;
using shared.Interfaces;
using shared.Data.Entities;

namespace web.Components.Account.Pages
{
    public partial class ResetPassword
    {
        [Parameter]
        public string SiteName { get; set; }

        [Parameter]
        public string Lang { get; set; }

        [Inject]
        IdentityRedirectManager RedirectManager { get; set; }

        [Inject]
        UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        private IEnumerable<IdentityError>? identityErrors;

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new();

        [SupplyParameterFromQuery]
        private string? Code { get; set; }

        private string? Message => identityErrors is null ? null : $"Error: {string.Join(", ", identityErrors.Select(error => error.Description))}";

        protected override void OnInitialized()
        {
            if (Code is null)
            {
                RedirectManager.RedirectTo(StateManager.GetPageUrl(StaticRoutesStrings.AccountInvalidPasswordResetPageRoute));
            }

            Input.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
        }

        private async Task OnValidSubmitAsync()
        {
            var user = await UserManager.FindByEmailAsync(Input.Email);
            if (user is null)
            {
                // Don't reveal that the user does not exist
                RedirectManager.RedirectTo(StateManager.GetPageUrl(StaticRoutesStrings.AccountResetPasswordConfirmationPageUrl));
            }

            var result = await UserManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                RedirectManager.RedirectTo(StateManager.GetPageUrl(StaticRoutesStrings.AccountResetPasswordConfirmationPageUrl));
            }

            identityErrors = result.Errors;
        }

        private sealed class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = "";

            [Required]
            public string Code { get; set; } = "";
        }
    }
}

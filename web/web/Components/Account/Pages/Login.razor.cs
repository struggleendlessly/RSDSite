using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authentication;

using System.ComponentModel.DataAnnotations;

using shared;
using shared.Models;
using shared.Interfaces;
using shared.Data.Entities;

namespace web.Components.Account.Pages
{
    public partial class Login
    {
        [Parameter]
        public string Lang { get; set; } = string.Empty;

        [Inject]
        SignInManager<ApplicationUser> SignInManager { get; set; }

        [Inject]
        ILogger<Login> Logger { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IdentityRedirectManager RedirectManager { get; set; }

        [Inject]
        UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        private string? errorMessage;

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new();

        [SupplyParameterFromQuery]
        private string? ReturnUrl { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            if (HttpMethods.IsGet(HttpContext.Request.Method))
            {
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }

            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
        }

        public async Task LoginUser()
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await SignInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                Logger.LogInformation("User logged in.");

                var user = await UserManager.Users
                    .Include(u => u.Websites)
                    .FirstOrDefaultAsync(u => u.Email == Input.Email);

                //StateManager.UserId = user.Id;
                //StateManager.UserEmail = user.Email;
                //StateManager.UserSites = user.Websites.Select(w => w.Name).ToList();

                RedirectManager.RedirectTo(ReturnUrl + user.Websites.FirstOrDefault().Name + $"/{StateManager.Lang}");
            }
            else if (result.RequiresTwoFactor)
            {
                RedirectManager.RedirectTo(
                    "Account/LoginWith2fa",
                    new() { ["returnUrl"] = ReturnUrl, ["rememberMe"] = Input.RememberMe });
            }
            else if (result.IsLockedOut)
            {
                Logger.LogWarning("User account locked out.");
                RedirectManager.RedirectTo("Account/Lockout");
            }
            else if (result.IsNotAllowed)
            {
                errorMessage = "Error: Please confirm your email to Sign in.";
            }
            else
            {
                errorMessage = "Error: Invalid login attempt.";
            }
        }

        private sealed class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
}

using System.Text;
using System.Text.Encodings.Web;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Components.Forms;

using web.Data;

using shared.Interfaces;

namespace web.Components.Account.Pages
{
    public partial class Register
    {
        [Inject]
        UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        IUserStore<ApplicationUser> UserStore { get; set; }

        [Inject]
        SignInManager<ApplicationUser> SignInManager { get; set; }

        [Inject]
        IEmailSender<ApplicationUser> EmailSender { get; set; }

        [Inject]
        ILogger<Register> Logger { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IdentityRedirectManager RedirectManager { get; set; }

        [Inject]
        ISiteCreator SiteCreator { get; set; }

        private IEnumerable<IdentityError>? identityErrors;

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new();

        [SupplyParameterFromQuery]
        private string? ReturnUrl { get; set; }

        private string? Message => identityErrors is null ? null : $"Error: {string.Join(", ", identityErrors.Select(error => error.Description))}";

        public async Task RegisterUser(EditContext editContext)
        {
            var user = CreateUser();

            await UserStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            var emailStore = GetEmailStore();
            await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            user.SiteName = Input.SiteName;
            var result = await UserManager.CreateAsync(user, Input.Password);

            if (!result.Succeeded)
            {
                identityErrors = result.Errors;
                return;
            }

            Logger.LogInformation("User created a new account with password.");

            await SiteCreator.CreateSite(Input.SiteName);

            Logger.LogInformation($"A website named {Input.SiteName} has been created.");

            // string scriptFilePath = @"D:\Work\RemSoftDev\RSDSite\web\web\create-website.ps1";
            // string userEmail = Input.Email;
            // string userPassword = Input.Password;
            // string siteName = Input.SiteName;

            // var parameters = new[]
            // {
            //     ("UserEmail", userEmail),
            //     ("UserPassword", userPassword),
            //     ("SiteName", siteName),
            //     ("PublishDirectory", @"D:\Work\RemSoftDev\RSDSite\web\web\bin\Release\net8.0")
            // };

            // ScriptRunner scriptRunner = new ScriptRunner();
            // scriptRunner.RunPowerShellScript(scriptFilePath, parameters);

            var userId = await UserManager.GetUserIdAsync(user);
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = NavigationManager.GetUriWithQueryParameters(
                NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
                new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = ReturnUrl });

            await EmailSender.SendConfirmationLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

            if (UserManager.Options.SignIn.RequireConfirmedAccount)
            {
                RedirectManager.RedirectTo(
                    "Account/RegisterConfirmation",
                    new() { ["email"] = Input.Email, ["returnUrl"] = ReturnUrl });
            }

            await SignInManager.SignInAsync(user, isPersistent: false);
            RedirectManager.RedirectTo(ReturnUrl);
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!UserManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)UserStore;
        }

        private sealed class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = "";

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = "";

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = "";

            [Required]
            [StringLength(63, MinimumLength = 3)]
            [Display(Name = "Site name")]
            public string SiteName { get; set; } = "";
        }
    }
}

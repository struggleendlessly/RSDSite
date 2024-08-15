using System.Text;
using System.Text.Encodings.Web;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Components.Forms;

using shared;
using shared.Models;
using shared.Interfaces;
using shared.Data.Entities;

namespace web.Components.Account.Pages
{
    public partial class Register
    {
        [Parameter]
        public string Lang { get; set; } = string.Empty;

        [Inject]
        UserManager<User> UserManager { get; set; }

        [Inject]
        IUserStore<User> UserStore { get; set; }

        [Inject]
        SignInManager<User> SignInManager { get; set; }

        [Inject]
        IEmailSender<User> EmailSender { get; set; }

        [Inject]
        ILogger<Register> Logger { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IdentityRedirectManager RedirectManager { get; set; }

        [Inject]
        ISiteCreator SiteCreator { get; set; }

        [Inject]
        IWebsiteService WebsiteService { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        private IEnumerable<IdentityError>? identityErrors;

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new();

        [SupplyParameterFromQuery]
        private string? ReturnUrl { get; set; }

        private string? Message => identityErrors is null ? null : $"Error: {string.Join(", ", identityErrors.Select(error => error.Description))}";

        public bool IsRegistering { get; set; } = false;

        public PageModel LocalizationModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
        }

        public async Task RegisterUser(EditContext editContext)
        {
            IsRegistering = true;
            await Task.Delay(1);

            var existingWebsite = await WebsiteService.GetWebsiteByName(Input.SiteName);
            if (existingWebsite != null)
            {
                identityErrors = new List<IdentityError>
                {
                    new IdentityError
                    {
                        Code = "DuplicateSiteName",
                        Description = "The site name is already taken. Please choose a different one."
                    }
                };

                IsRegistering = false;
                return;
            }

            var user = CreateUser();
            await UserStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            var emailStore = GetEmailStore();
            await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            var result = await UserManager.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                identityErrors = result.Errors;
                IsRegistering = false;

                return;
            }

            Logger.LogInformation("User created a new account with password.");

            var newWebsite = new Website { Name = Input.SiteName };
            await WebsiteService.CreateWebsite(newWebsite, user.Id);

            Logger.LogInformation($"A website named {newWebsite.Name} has been created.");

            await SiteCreator.CreateSite(newWebsite.Name);

            var userId = await UserManager.GetUserIdAsync(user);
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = NavigationManager.GetUriWithQueryParameters(
                NavigationManager.ToAbsoluteUri(StateManager.GetPageUrl(StaticRoutesStrings.AccountConfirmEmailPageUrl, false)).AbsoluteUri,
                new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = ReturnUrl });

            await EmailSender.SendConfirmationLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

            if (UserManager.Options.SignIn.RequireConfirmedAccount)
            {
                var redirectUrl = NavigationManager.GetUriWithQueryParameters(
                    NavigationManager.ToAbsoluteUri(StateManager.GetPageUrl(StaticRoutesStrings.RegisterConfirmationPageUrl, false)).AbsoluteUri,
                    new Dictionary<string, object?> { ["email"] = Input.Email, ["returnUrl"] = ReturnUrl });

                NavigationManager.NavigateTo(redirectUrl);

                //RedirectManager.RedirectTo(
                //    StateManager.GetPageUrl(StaticRoutesStrings.RegisterConfirmationPageUrl, false),
                //    new() { ["email"] = Input.Email, ["returnUrl"] = ReturnUrl });
            }
            else
            {
                await SignInManager.SignInAsync(user, isPersistent: false);
                NavigationManager.NavigateTo(ReturnUrl ?? StaticRoutesStrings.EmptyRoute);
            }

            //await SignInManager.SignInAsync(user, isPersistent: false);
            //RedirectManager.RedirectTo(ReturnUrl);
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!UserManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)UserStore;
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
            [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "The site name can only contain letters and numbers.")]
            public string SiteName { get; set; } = "";
        }
    }
}

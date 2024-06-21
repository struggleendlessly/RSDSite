using shared;
using shared.Emails;
using shared.Models;
using shared.Interfaces;
using shared.Data.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;

namespace web.Components.Account
{
    public class IdentityEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly EmailService _emailService;
        private readonly EmailSenders _emailSenders;
        private readonly ITemplateService _templateService;
        private readonly IPageDataService _pageDataService;
        private readonly NavigationManager _navigationManager;

        public IdentityEmailSender(
            EmailService emailService, 
            EmailSenders emailSenders, 
            ITemplateService templateService, 
            IPageDataService pageDataService,
            NavigationManager navigationManager) 
        {
            _emailService = emailService;
            _emailSenders = emailSenders;
            _templateService = templateService;
            _pageDataService = pageDataService;
            _navigationManager = navigationManager;
        }

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            var localizationModel = await _pageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
            var settingsModel = await _pageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsDataJsonFilePath);

            var placeholders = new Dictionary<string, string>
            {
                { "ConfirmationLink", confirmationLink },
                { "MainLink", _navigationManager.BaseUri },
                { "LogoUrl", settingsModel.Data[StaticStrings.AdminPageDataLogoKey] }
            };

            var htmlContent = await _templateService.GetTemplateHtmlAsync(StaticStrings.EmailTemplatesConfirmEmailDataHtmlFilePath, placeholders);

            var emailModel = new EmailModel
            {
                Subject = localizationModel.Data[StaticStrings.Localization_Email_ConfirmEmail_Subject_Key],
                HtmlContent = htmlContent,
                Recipient = email,
                Sender = _emailSenders.DoNotReply
            };

            await _emailService.Send(emailModel);
        }

        public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            var emailModel = new EmailModel
            {
                Subject = "Reset your password",
                HtmlContent = $"Please reset your password using the following code: {resetCode}",
                Recipient = email,
                Sender = _emailSenders.DoNotReply
            };

            await _emailService.Send(emailModel);
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            var localizationModel = await _pageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
            var settingsModel = await _pageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsDataJsonFilePath);

            var placeholders = new Dictionary<string, string>
            {
                { "ResetLink", resetLink },
                { "MainLink", _navigationManager.BaseUri },
                { "LogoUrl", settingsModel.Data[StaticStrings.AdminPageDataLogoKey] }
            };

            var htmlContent = await _templateService.GetTemplateHtmlAsync(StaticStrings.EmailTemplatesResetPasswordDataHtmlFilePath, placeholders);

            var emailModel = new EmailModel
            {
                Subject = localizationModel.Data[StaticStrings.Localization_Email_PasswordReset_Subject_Key],
                HtmlContent = htmlContent,
                Recipient = email,
                Sender = _emailSenders.DoNotReply
            };

            await _emailService.Send(emailModel);
        }
    }
}

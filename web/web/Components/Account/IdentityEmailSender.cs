using Microsoft.AspNetCore.Identity;

using shared.Emails;
using shared.Data.Entities;

namespace web.Components.Account
{
    public class IdentityEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly EmailService _emailService;
        private readonly EmailSenders _emailSenders;

        public IdentityEmailSender(EmailService emailService, EmailSenders emailSenders) 
        {
            _emailService = emailService;
            _emailSenders = emailSenders;
        }

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            var emailModel = new EmailModel
            {
                Subject = "Confirm your email",
                HtmlContent = $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.",
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
            var emailModel = new EmailModel
            {
                Subject = "Reset your password",
                HtmlContent = $"Please reset your password by <a href='{resetLink}'>clicking here</a>.",
                Recipient = email,
                Sender = _emailSenders.DoNotReply
            };

            await _emailService.Send(emailModel);
        }
    }
}

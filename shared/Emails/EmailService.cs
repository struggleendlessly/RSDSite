using Azure;
using Azure.Communication.Email;

using Microsoft.Extensions.Options;

using shared.ConfigurationOptions;

namespace shared.Emails
{
    public class EmailService
    {
        ArureEmailCummunicationOptions arureEmailCummunicationOptions;
        public EmailService(IOptions<ArureEmailCummunicationOptions> _arureEmailCummunicationOptions)
        {
                arureEmailCummunicationOptions = _arureEmailCummunicationOptions.Value;
        }

        public async Task<EmailSendResult> Send(EmailModel emailModel)
        {
            EmailClient emailClient = new EmailClient(arureEmailCummunicationOptions.ConnectionString);

            var subject = emailModel.Subject;
            var htmlContent = emailModel.HtmlContent;
            var sender = emailModel.Sender;
            var recipient = emailModel.Recipient;

            EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                WaitUntil.Completed,
                sender,
                recipient,
                subject,
                htmlContent);

            EmailSendResult statusMonitor = emailSendOperation.Value;

            return statusMonitor;
        }
    }
}

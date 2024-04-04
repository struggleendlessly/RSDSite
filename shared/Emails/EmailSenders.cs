using Microsoft.Extensions.Options;

using shared.ConfigurationOptions;

namespace shared.Emails
{
    public class EmailSenders
    {
        AzureEmailCommunicationOptions arureEmailCummunicationOptions;
        public EmailSenders(IOptions<AzureEmailCommunicationOptions> _arureEmailCummunicationOptions)
        {
            arureEmailCummunicationOptions = _arureEmailCummunicationOptions.Value;
            var domain = arureEmailCummunicationOptions.Domain;

            DoNotReply = DoNotReply.Replace("{domain}", domain);
            Info = Info.Replace("{domain}", domain);
        }

        public string DoNotReply { get; } = "DoNotReply@{domain}";
        public string Info { get; } = "noreply@{domain}";
    }
}

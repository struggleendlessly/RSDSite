namespace shared.Emails
{
    public record EmailModel
    {
        public string Subject { get; init; }
        public string HtmlContent { get; init; }
        public string Sender { get; init; }
        public string Recipient { get; init; }
    }
}

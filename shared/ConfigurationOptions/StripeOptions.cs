namespace shared.ConfigurationOptions
{
    public class StripeOptions
    {
        public static string SectionName { get; } = "Stripe";
        public string WebhookSigningSecret { get; set; }
    }
}

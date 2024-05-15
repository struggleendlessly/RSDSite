namespace shared.ConfigurationOptions
{
    public class StripeOptions
    {
        public static string SectionName { get; } = "Stripe";
        public string WebhookSigningSecret { get; set; }
        public PricingTable PricingTable { get; set; }
    }

    public class PricingTable
    {
        public string WebsiteSubscriptionPricingTableId { get; set; }
        public string CustomDomainSubscriptionPricingTableId { get; set; }
        public string PublishableKey { get; set; }
    }
}
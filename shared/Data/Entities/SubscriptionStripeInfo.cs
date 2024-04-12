namespace shared.Data.Entities
{
    public class SubscriptionStripeInfo
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ICollection<SubscriptionModule> SubscriptionModules { get; set; } = new List<SubscriptionModule>();

    }
}

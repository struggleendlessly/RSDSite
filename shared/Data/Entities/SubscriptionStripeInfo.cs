namespace shared.Data.Entities
{
    public class SubscriptionStripeInfo
    {
        public Guid Id { get; set; }
        //public ICollection<SubscriptionModule> SubscriptionModule { get; set; } = new List<SubscriptionModule>();
        public string Code { get; set; }
        public string Name { get; set; }

    }
}

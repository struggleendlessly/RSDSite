using Microsoft.AspNetCore.Identity;

namespace shared.Data.Entities
{
    public class SubscriptionModule
    {
        public Guid Id { get; set; }
        //public Subscription SubscriptionId { get; set; } = new Subscription();
        public string Name { get; set; }
        public SubscriptionStripeInfo Stripe { get; set; }
    }
}

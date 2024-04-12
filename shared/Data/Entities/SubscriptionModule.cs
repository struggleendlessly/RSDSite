using Microsoft.AspNetCore.Identity;

namespace shared.Data.Entities
{
    public class SubscriptionModule
    {
        public Guid Id { get; set; }
        public ICollection<Subscription> Subscription { get; set; } = new List<Subscription>();
        public string Name { get; set; }
        public SubscriptionStripeInfo Stripe { get; set; }
    }
}

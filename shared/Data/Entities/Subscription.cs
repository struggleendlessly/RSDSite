using Microsoft.AspNetCore.Identity;

namespace shared.Data.Entities
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public Website Website { get; set; } = new Website();
        public string StripeCustomerId { get; set; }
        public string StripeSubscriptionId { get; set; }
        public bool IsActive { get; set; } = true;

        public SubscriptionModule SubscriptionModule { get; set; } = new SubscriptionModule();
    }
}

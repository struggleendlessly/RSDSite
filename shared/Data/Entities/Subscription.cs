using Microsoft.AspNetCore.Identity;

namespace shared.Data.Entities
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public Website WebsiteId { get; set; } = new Website();
        public SubscriptionModule SubscriptionModule { get; set; } = new SubscriptionModule();

        public bool IsActive { get; set; } = true;
    }
}

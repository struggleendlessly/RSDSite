using System;

namespace shared.Data.Entities
{
    public class Website
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Domain {  get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }= new List<Subscription>();

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public ICollection<ContactUsMessage> ContactUsMessages { get; set; } = new List<ContactUsMessage>();
    }
}

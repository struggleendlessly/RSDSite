using System;

namespace shared.Data.Entities
{
    public class Website
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }= new List<Subscription>();
        public ApplicationUser User { get; set; }

        public ICollection<ContactUsMessage> ContactUsMessages { get; set; }
    }
}

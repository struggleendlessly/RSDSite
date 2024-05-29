using System;

namespace shared.Data.Entities
{
    public class Website
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Domain {  get; set; }
        public bool IsNewDomainInProcess { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }= new List<Subscription>();

        public ICollection<ApplicationUser> Users { get; set; } = [];

        public ICollection<ContactUsMessage> ContactUsMessages { get; set; } = new List<ContactUsMessage>();
    }
}

using System;

namespace shared.Data.Entities
{
    public class Website
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<ContactUsMessage> ContactUsMessages { get; set; }
    }
}

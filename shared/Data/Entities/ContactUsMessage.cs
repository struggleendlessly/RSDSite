using System;

namespace shared.Data.Entities
{
    public class ContactUsMessage
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Details { get; set; }

        public Guid WebsiteId { get; set; }
        public Website Website { get; set; }
    }
}

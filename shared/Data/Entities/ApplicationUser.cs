using Microsoft.AspNetCore.Identity;

namespace shared.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Guid WebsiteId { get; set; }
        public Website Website { get; set; }
    }
}

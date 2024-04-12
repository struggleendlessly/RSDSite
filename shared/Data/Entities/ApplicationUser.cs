using Microsoft.AspNetCore.Identity;

namespace shared.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Website> Websites { get; set; }      
    }
}

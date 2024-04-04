using System.ComponentModel.DataAnnotations;

namespace shared.Models
{
    public class ContactUsMessageModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public string Details { get; set; }
    }
}

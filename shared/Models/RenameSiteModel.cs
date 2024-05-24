using System.ComponentModel.DataAnnotations;

namespace shared.Models
{
    public class RenameSiteModel
    {
        [Required]
        public string SiteName { get; set; } = string.Empty;

        [Required]
        [StringLength(63, MinimumLength = 3)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "The site name can only contain letters and numbers.")]
        public string NewName { get; set; } = string.Empty;
    }
}

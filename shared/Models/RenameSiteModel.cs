using System.ComponentModel.DataAnnotations;

namespace shared.Models
{
    public class RenameSiteModel
    {
        [Required]
        public string SiteName { get; set; } = string.Empty;

        [Required]
        [StringLength(63, MinimumLength = 3)]
        public string NewName { get; set; } = string.Empty;
    }
}

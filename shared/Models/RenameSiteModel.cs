using System.ComponentModel.DataAnnotations;

namespace shared.Models
{
    public class RenameSiteModel
    {
        [Required]
        public string SiteName { get; set; } = string.Empty;

        [Required]
        public string NewName { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace shared.Models
{
    public class CreateSiteModel
    {
        [Required]
        [StringLength(63, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;
    }
}

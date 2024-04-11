using System.ComponentModel.DataAnnotations;

namespace shared.Models
{
    public class CreateSiteModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}

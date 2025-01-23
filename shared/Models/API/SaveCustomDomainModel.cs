using System;

namespace shared.Models.API
{
    public class SaveCustomDomainModel
    {
        public string SiteName { get; set; } = default!;
        public string CustomDomain { get; set; } = default!;
    }
}

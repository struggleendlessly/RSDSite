using System;

namespace shared.Models
{
    public class ServiceItem
    {
        public Dictionary<string, string> ShortDesc { get; set; }
        public Dictionary<string, string> LongDesc { get; set; }
        public Dictionary<string, string> SEO { get; set; }
    }
}

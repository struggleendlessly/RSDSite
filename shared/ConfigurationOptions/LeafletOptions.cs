using System;

namespace shared.ConfigurationOptions
{
    public class LeafletOptions
    {
        public static string SectionName { get; } = "Leaflet";
        public string AccessToken { get; set; } = string.Empty;
    }
}

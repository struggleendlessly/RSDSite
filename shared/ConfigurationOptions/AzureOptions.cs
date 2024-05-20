using System;

namespace shared.ConfigurationOptions
{
    public class AzureOptions
    {
        public static string SectionName { get; } = "Azure";
        public string WebAppName { get; set; } = string.Empty;
        public string WebAppResourceGroup { get; set; } = string.Empty;
    }
}

using System;

namespace shared.ConfigurationOptions
{
    public class ApiOptions
    {
        public static string SectionName { get; } = "Api";
        public string AccessToken { get; set; } = string.Empty;
        public string Url {  get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
    }
}

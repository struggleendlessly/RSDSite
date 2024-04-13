namespace shared.ConfigurationOptions
{
    public class MainSiteOwnersOptions
    {
        public static string SectionName { get; } = "MainSiteOwners";
        public List<string> Emails { get; set; }
    }
}

namespace shared.ConfigurationOptions
{
    public class ArureEmailCummunicationOptions
    {
        public static string SectionName { get; } = "ArureEmailCummunication";
        public string ConnectionString { get; set; }
        public string Domain { get; set; }
    }
}

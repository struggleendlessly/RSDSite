namespace shared.ConfigurationOptions
{
    public class AzureEmailCommunicationOptions
    {
        public static string SectionName { get; } = "AzureEmailCommunication";
        public string ConnectionString { get; set; }
        public string Domain { get; set; }
    }
}

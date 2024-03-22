namespace shared.ConfigurationOptions
{
    public class AzureBlobStorageOptions
    {
        public static string SectionName { get; } = "AzureBlobStorage";
        public string ConnectionString { get; set; }
    }
}

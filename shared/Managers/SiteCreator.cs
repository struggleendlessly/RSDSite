using shared.Interfaces;

namespace shared.Managers
{
    public class SiteCreator : ISiteCreator
    {
        private readonly AzureBlobStorageManager _azureBlobStorageManager;

        public SiteCreator(AzureBlobStorageManager azureBlobStorageManager)
        {
            _azureBlobStorageManager = azureBlobStorageManager;
        }

        public async Task CreateSite(string siteName)
        {
            string sourceDirectory = Path.Combine(StaticStrings.WwwRootPath, @"data\example");
            siteName = siteName.ToLower();

            try
            {
                string[] jsonFiles = Directory.GetFiles(sourceDirectory, "*.json");
                foreach (string jsonFile in jsonFiles)
                {
                    string fileName = Path.GetFileName(jsonFile);

                    using (FileStream fileStream = File.OpenRead(jsonFile))
                    {
                        await _azureBlobStorageManager.UploadFile(siteName, $"data/{fileName}", fileStream);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
    }
}

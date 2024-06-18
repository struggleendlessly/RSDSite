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
            siteName = siteName.ToLower();

            string sourceContainerName = StaticStrings.ExampleContainerName;
            string destinationContainerName = siteName;

            try
            {
                await _azureBlobStorageManager.CopyAllFilesAsync(sourceContainerName, destinationContainerName);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
    }
}

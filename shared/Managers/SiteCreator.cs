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

            string sourceContainerName = "example";
            string sourceFolderPath = "data";
            string destinationContainerName = siteName;
            string destinationFolderPath = "data";

            try
            {
                await _azureBlobStorageManager.CopyFilesFromFolderAsync(sourceContainerName, sourceFolderPath, destinationContainerName, destinationFolderPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
    }
}

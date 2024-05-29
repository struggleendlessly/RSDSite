using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

using shared.ConfigurationOptions;

namespace shared.Managers
{
    public class AzureBlobStorageManager
    {
        private readonly string _connectionString;

        public AzureBlobStorageManager(IOptions<AzureBlobStorageOptions> _azureBlobStorageOptions)
        {
            _connectionString = _azureBlobStorageOptions.Value.ConnectionString;
        }


        // TODO: Send string to UI
        public async Task<string> DownloadFile(string blobContainerName, string blobName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync())
                return string.Empty;

            await EnsureContainerPublicAccessAsync(containerClient);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(memoryStream);
                memoryStream.Position = 0;

                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        public async Task<string> UploadFile(string blobContainerName, string blobName, Stream fileStream)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

            await containerClient.CreateIfNotExistsAsync();

            await EnsureContainerPublicAccessAsync(containerClient);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(fileStream, overwrite: true);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task CopyAllFilesAsync(string sourceContainerName, string destinationContainerName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);

            BlobContainerClient sourceContainerClient = blobServiceClient.GetBlobContainerClient(sourceContainerName);
            BlobContainerClient destinationContainerClient = blobServiceClient.GetBlobContainerClient(destinationContainerName);

            await destinationContainerClient.CreateIfNotExistsAsync();

            await EnsureContainerPublicAccessAsync(sourceContainerClient);
            await EnsureContainerPublicAccessAsync(destinationContainerClient);

            await foreach (BlobItem blobItem in sourceContainerClient.GetBlobsAsync())
            {
                if (blobItem is BlobItem blob)
                {
                    BlobClient sourceBlobClient = sourceContainerClient.GetBlobClient(blob.Name);

                    string destinationBlobName = blob.Name;

                    BlobClient destinationBlobClient = destinationContainerClient.GetBlobClient(destinationBlobName);

                    await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);
                }
            }
        }

        public async Task RenameContainerAsync(string sourceContainerName, string destinationContainerName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);

            BlobContainerClient sourceContainerClient = blobServiceClient.GetBlobContainerClient(sourceContainerName);
            BlobContainerClient destinationContainerClient = blobServiceClient.GetBlobContainerClient(destinationContainerName);

            await destinationContainerClient.CreateIfNotExistsAsync();

            await EnsureContainerPublicAccessAsync(destinationContainerClient);

            await CopyAllFilesAsync(sourceContainerName, destinationContainerName);

            await sourceContainerClient.DeleteAsync();
        }

        private async Task EnsureContainerPublicAccessAsync(BlobContainerClient containerClient)
        {
            BlobContainerAccessPolicy accessPolicy = await containerClient.GetAccessPolicyAsync();
            if (accessPolicy.BlobPublicAccess == PublicAccessType.None)
            {
                await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            }
        }
    }
}

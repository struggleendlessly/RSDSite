using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace shared.Managers
{
    public class AzureBlobStorageManager
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly string _blobEndpointUrl;

        public AzureBlobStorageManager(IConfiguration configuration)
        {
            _connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _containerName = configuration["AzureBlobStorage:ContainerName"];
            _blobEndpointUrl = configuration["AzureBlobStorage:BlobEndpointUrl"];
        }

        public async Task<string> UploadImageAsync(string base64Image, string blobName)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                await blobClient.UploadAsync(memoryStream, true);
            }

            return $"{_blobEndpointUrl}/{_containerName}/{blobName}";
        }
    }
}

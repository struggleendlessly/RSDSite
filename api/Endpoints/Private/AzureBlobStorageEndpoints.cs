using shared;
using shared.Managers;
using shared.Models.API;

namespace api.Endpoints.Private
{
    public static class AzureBlobStorageEndpoints
    {
        public static void MapAzureBlobStorageEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup(StaticStrings.Route_API_AzureBlobStorage);

            group.MapPost("/upload", async (UploadFileModel model, AzureBlobStorageManager blobStorageManager) =>
            {
                using (MemoryStream stream = new MemoryStream(model.FileData))
                return await blobStorageManager.UploadFile(model.SiteName, model.BlobName, stream);

            }).RequireAuthorization();

            group.MapPost("/rename-container", async (RenameContainerModel model, AzureBlobStorageManager blobStorageManager) =>
            {
                await blobStorageManager.RenameContainerAsync(model.SourceContainerName, model.DestinationContainerName);

            }).RequireAuthorization();
        }
    }
}

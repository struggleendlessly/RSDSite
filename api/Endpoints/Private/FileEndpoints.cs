using shared;
using shared.Managers;
using shared.Models.API;

namespace api.Endpoints.Private
{
    public static class FileEndpoints
    {
        public static void MapFileEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup(StaticStrings.Route_API_File);

            group.MapPost("/upload", async (UploadFileModel model, AzureBlobStorageManager blobStorageManager) =>
            {
                using (MemoryStream stream = new MemoryStream(model.FileData))
                return await blobStorageManager.UploadFile(model.SiteName, model.BlobName, stream);

            }).RequireAuthorization();
        }
    }
}

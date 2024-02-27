using shared;

namespace web.Endpoints
{
    public static class FileEndpoint
    {
        public static void MapFileEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/file/{fileName}", async context =>
            {
                var fileName = context.Request.RouteValues["fileName"] as string;

                var directoryPath = StaticStrings.WwwRootPath;

                var fullFilePath = Path.Combine(directoryPath, "data", fileName);

                if (File.Exists(fullFilePath))
                {
                    var fileContent = await File.ReadAllTextAsync(fullFilePath);

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(fileContent);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
            });
        }
    }
}

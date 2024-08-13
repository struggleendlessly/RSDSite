using shared.Models;
using shared.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace api.Endpoints.Public
{
    public static class PageDataEndpoints
    {
        public static void MapPageDataEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("api/pageData");

            group.MapGet("/pageModel", async (
                [FromQuery] string memoryCacheKey, 
                [FromQuery] string siteName, 
                [FromQuery] string lang, 
                [FromQuery] string filePath, 
                [FromQuery] string? blobContainerName, 
                [FromServices] IPageDataService pageDataService) =>
            {
                var data = await pageDataService.GetDataAsync<PageModel>(memoryCacheKey, siteName, lang, filePath, blobContainerName);
                return data is null ? Results.NotFound() : Results.Ok(data);
            });

            group.MapGet("/serviceItems", async (
                [FromQuery] string memoryCacheKey, 
                [FromQuery] string siteName, 
                [FromQuery] string lang, 
                [FromQuery] string filePath, 
                [FromQuery] string? blobContainerName, 
                [FromServices] IPageDataService pageDataService) =>
            {
                var data = await pageDataService.GetDataAsync<List<ServiceItem>>(memoryCacheKey, siteName, lang, filePath, blobContainerName);
                return data is null ? Results.NotFound() : Results.Ok(data);
            });
        }
    }
}

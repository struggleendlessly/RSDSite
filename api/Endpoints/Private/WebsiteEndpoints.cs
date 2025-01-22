using shared;
using shared.Interfaces;
using shared.Data.Entities;

using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

namespace api.Endpoints.Private
{
    public static class WebsiteEndpoints
    {
        public static void MapWebsiteEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup(StaticStrings.Route_API_Website);

            group.MapGet("/", async (HttpContext httpContext, IWebsiteService websiteService) =>
            {
                var idpName = httpContext.User.FindFirstValue(StaticStrings.MSAL_IDP_ClaimType);
                var idpUserIdStr = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (idpName is null || idpUserIdStr is null)
                {
                    return Results.BadRequest("Invalid identity provider or user ID");
                }

                var idpUserIdGuid = Guid.Parse(idpUserIdStr);
                var websites = await websiteService.GetAllOrCreateAsync(idpName, idpUserIdGuid);
                return Results.Ok(websites);
            }).RequireAuthorization();

            group.MapGet("/getByName", async (
                [FromQuery] string siteName,
                [FromServices] IWebsiteService websiteService) =>
            {
                var result = await websiteService.GetWebsiteByName(siteName);
                return Results.Ok(result);
            });

            group.MapGet("/domain", async (
                [FromQuery] string siteName,
                [FromServices] IWebsiteService websiteService) =>
            {
                var result = await websiteService.GetSiteDomainAsync(siteName);
                return Results.Ok(result);
            });

            group.MapPost(string.Empty, async (
                HttpContext httpContext,
                Website model,
                IWebsiteService websiteService) =>
            {
                var idpUserIdStr = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var idpUserIdGuid = Guid.Parse(idpUserIdStr);

                var result = await websiteService.CreateAsync(model, idpUserIdGuid);
                return Results.Ok(result);
            }).RequireAuthorization();

            group.MapPut(string.Empty, async (
                Website model,
                IWebsiteService websiteService) =>
            {
                var result = await websiteService.UpdateAsync(model);
                return Results.Ok(result);
            }).RequireAuthorization();
        }
    }
}

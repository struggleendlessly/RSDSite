using shared;
using shared.Interfaces;

using System.Security.Claims;

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
        }
    }
}

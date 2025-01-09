using shared;
using shared.Interfaces;

using System.Security.Claims;

namespace api.Endpoints.Private
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup(StaticStrings.Route_API_User);

            group.MapGet("/", async (HttpContext httpContext, IUserService userService) =>
            {
                var idpName = httpContext.User.FindFirstValue(StaticStrings.MSAL_IDP_ClaimType);
                var idpUserIdStr = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (idpName is null || idpUserIdStr is null)
                {
                    return Results.BadRequest("Invalid identity provider or user ID");
                }

                var idpUserIdGuid = Guid.Parse(idpUserIdStr);
                var user = await userService.GetOrCreateAsync(idpName, idpUserIdGuid);
                return Results.Ok(user);
            }).RequireAuthorization();
        }
    }
}

using shared;
using shared.Interfaces;
using shared.Models.API;

namespace api.Endpoints.Private
{
    public static class CustomDomainEndpoints
    {
        public static void MapCustomDomainEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup(StaticStrings.Route_API_CustomDomain);

            group.MapPost("/save", async (
                SaveCustomDomainModel model,
                ICustomDomainService customDomainService) =>
            {
                await customDomainService.SaveCustomDomainAsync(model.SiteName, model.CustomDomain);
            }).RequireAuthorization();

            group.MapPost("/check-verification", async (
                CheckCustomDomainVerificationModel model,
                ICustomDomainService customDomainService) =>
            {
                var result = await customDomainService.CheckCustomDomainVerificationAsync(model.SiteName, model.Lang);
                return Results.Ok(result);
            }).RequireAuthorization();
        }
    }
}

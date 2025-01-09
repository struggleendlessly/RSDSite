using shared.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace api.Endpoints.Public
{
    public static class SubscriptionEndpoints
    {
        public static void MapSubscriptionEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("api/subscription");

            group.MapGet("/website", async (
                [FromQuery] string siteName,
                [FromServices] ISubscriptionService subscriptionService) =>
            {
                var result = await subscriptionService.IsWebsiteSubscriptionActiveAsync(siteName);
                return Results.Ok(result);
            });

            group.MapGet("/custom-domain", async (
                [FromQuery] string siteName,
                [FromServices] ISubscriptionService subscriptionService) =>
            {
                var result = await subscriptionService.IsCustomDomainSubscriptionActiveAsync(siteName);
                return Results.Ok(result);
            });
        }
    }
}

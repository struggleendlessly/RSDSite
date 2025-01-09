using shared.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace api.Endpoints.Public
{
    public static class PricingEndpoints
    {
        public static void MapPricingEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("api/pricing");

            group.MapGet(string.Empty, async (
                [FromQuery] string lang,
                [FromServices] IStripeService stripeService) =>
            {
                var result = await stripeService.GetProductsWithPricesAsync(lang);
                return Results.Ok(result);
            });
        }
    }
}

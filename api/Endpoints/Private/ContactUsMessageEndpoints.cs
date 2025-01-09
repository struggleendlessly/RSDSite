using shared;
using shared.Interfaces;
using shared.Data.Entities;

using Microsoft.AspNetCore.Mvc;

namespace api.Endpoints.Private
{
    public static class ContactUsMessageEndpoints
    {
        public static void MapContactUsMessageEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup(StaticStrings.Route_API_ContactUsMessage);

            group.MapGet(string.Empty, async (
                [FromQuery] string siteName,
                IContactUsMessageService contactUsMessageService) =>
            {
                var result = await contactUsMessageService.GetContactUsMessages(siteName);
                return Results.Ok(result);
            }).RequireAuthorization();

            group.MapPost(string.Empty, async (
                ContactUsMessage model,
                IContactUsMessageService contactUsMessageService) =>
            {
                var result = await contactUsMessageService.CreateContactUsMessage(model);
                return Results.Ok(result);
            });
        }
    }
}

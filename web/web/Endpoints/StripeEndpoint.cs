using shared.DTO.Stripe;

using Stripe;

using System.Text.Json;

namespace web.Endpoints
{
    public static class StripeEndpoint
    {
        const string endpointSecret = "whsec_64abfe30f633f312ea2dd4b41ab7a135f762e1a5d3294e296b5c5831259aad4d";
        public static void MapStripeEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/webhook/stripe", async (HttpContext context) =>
            {
                var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
                try
                {
                    var stripeEvent = EventUtility.ConstructEvent(json,
                        context.Request.Headers["Stripe-Signature"], endpointSecret);

                    string jsonString = JsonSerializer.Serialize(stripeEvent);

                    System.IO.File.WriteAllText(@$"H:\jsonString-{stripeEvent.Type}.json", jsonString);
                    // Handle the event
                    if (stripeEvent.Type == Events.CustomerCreated)
                    {
                        var data = JsonSerializer.Deserialize<CustomerCreated.Rootobject>(stripeEvent.Data.ToJson());
                        var email = data._object.email;
                        var customer = data._object.id; // customer stripe id field

                    }
                    if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                    {
                        var data = JsonSerializer.Deserialize<CheckoutSessionCompleted.Rootobject>(stripeEvent.Data.ToJson());
                        var siteName = data._object.custom_fields[0].text.value;
                        var email = data._object.customer_details.email;
                        var customer = data._object.customer; // customer stripe id field
                        var subscription = data._object.subscription; // subscription stripe id field
                        //TODO: if subscription exists - do nothing, else:
                        //Create user with email
                        //create website with sitename
                        //send a welcome email with password reset link
                        //send email confirmation link
                    }
                    else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted &&
                             stripeEvent.Type == Events.CustomerSubscriptionPaused)
                    {

                    }
                    else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
                    {
                        var data = JsonSerializer.Deserialize<CustomerSubscriptionUpdated.Rootobject>(stripeEvent.Data.ToJson());
                        var subscriptionUpdatedType = data._object._object;// = "subscription";

                        if (subscriptionUpdatedType.Equals("subscriptions"))
                        {
                            //TODO: 
                            //update subscription status in db
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    }

                    return Results.Ok();
                }
                catch (StripeException e)
                {
                    return Results.BadRequest();
                }
            });
        }
    }
}

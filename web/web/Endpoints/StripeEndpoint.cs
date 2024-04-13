using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

using shared;
using shared.Managers;
using shared.DTO.Stripe;
using shared.Interfaces;
using shared.Data.Entities;

using Stripe;

using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using shared.ConfigurationOptions;

namespace web.Endpoints
{
    public static class StripeEndpoint
    {
        private static IServiceProvider _serviceProvider;

        public static void MapStripeEndpoint(
            this IEndpointRouteBuilder endpoints, 
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider;

            endpoints.MapPost("/webhook/stripe", async (HttpContext context, IOptions<StripeOptions> stripeOptions) =>
            {
                using var scope = _serviceProvider.CreateScope();

                var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
                try
                {
                    var stripeEvent = EventUtility.ConstructEvent(json,
                        context.Request.Headers["Stripe-Signature"], stripeOptions.Value.WebhookSigningSecret);

                    string jsonString = JsonSerializer.Serialize(stripeEvent);

                    //System.IO.File.WriteAllText(@$"H:\jsonString-{stripeEvent.Type}.json", jsonString);
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

                        var stateManager = scope.ServiceProvider.GetRequiredService<IStateManager>();
                        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender<ApplicationUser>>();

                        //TODO: if subscription exists - do nothing, else:

                        //Create user with email
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var user = new ApplicationUser { UserName = email, Email = email };
                        var password = PasswordGenerator.GeneratePassword(8);
                        var result = await userManager.CreateAsync(user, password);

                        //create website with sitename
                        var websiteService = scope.ServiceProvider.GetRequiredService<IWebsiteService>();
                        var siteCreator = scope.ServiceProvider.GetRequiredService<ISiteCreator>();

                        var website = new Website { UserId = user.Id, Name = siteName };

                        await websiteService.CreateWebsite(website);
                        await siteCreator.CreateSite(website.Name);

                        //send a welcome email with password reset link
                        var passwordResetCode = await userManager.GeneratePasswordResetTokenAsync(user);
                        passwordResetCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordResetCode));

                        var passwordResetPageUrl = $"{stateManager.SiteName}/{stateManager.Lang}/{StaticRoutesStrings.AccountResetPasswordPageUrl}";
                        var passwordResetCallbackUrl = $"{context.Request.Scheme}://{context.Request.Host.Value}/{passwordResetPageUrl}?code={passwordResetCode}";

                        await emailSender.SendPasswordResetLinkAsync(user, email, HtmlEncoder.Default.Encode(passwordResetCallbackUrl));

                        //send email confirmation link
                        var emailConfirmationCode = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        emailConfirmationCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationCode));

                        var emailConfirmationPageUrl = $"{stateManager.SiteName}/{stateManager.Lang}/{StaticRoutesStrings.AccountConfirmEmailPageUrl}";
                        var emailConfirmationCallbackUrl = $"{context.Request.Scheme}://{context.Request.Host.Value}/{emailConfirmationPageUrl}?userId={user.Id}&code={emailConfirmationCode}";

                        await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(emailConfirmationCallbackUrl));
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

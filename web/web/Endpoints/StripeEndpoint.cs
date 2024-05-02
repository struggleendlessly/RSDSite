using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

using shared;
using shared.Data;
using shared.Managers;
using shared.DTO.Stripe;
using shared.Interfaces;
using shared.Data.Entities;
using shared.ConfigurationOptions;

using Stripe;

using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace web.Endpoints
{
    public static class StripeEndpoint
    {
        public static void MapStripeEndpoint(
            this IEndpointRouteBuilder endpoints
            )
        {
            endpoints.MapPost("/webhook/stripe",
                async (
                    HttpContext context,
                    IOptions<StripeOptions> stripeOptions,
                    ApplicationDbContext dbContext,
                    IEmailSender<ApplicationUser> emailSender,
                    UserManager<ApplicationUser> userManager,
                    IWebsiteService websiteService,
                    ISiteCreator siteCreator
                    ) =>
            {
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
                        var stripeCustomer = data._object.customer; // customer stripe id field
                        var stripeSubscription = data._object.subscription; // subscription stripe id field

                        var subscription = await dbContext.Subscriptions.FirstOrDefaultAsync(s => s.StripeSubscriptionId == stripeSubscription);
                        if (subscription is not null)
                        {
                            return Results.Ok();
                        }

                        //TODO: if subscription exists - do nothing, else:

                        var userExsist = await dbContext.Users.FirstOrDefaultAsync(s => s.Email == email);
                        if (userExsist is not null)
                        {
                            return Results.Ok();
                        }

                        //Create user with email
                        var user = new ApplicationUser { UserName = email, Email = email };
                        var password = PasswordGenerator.GeneratePassword(8);
                        var result = await userManager.CreateAsync(user, password);

                        //create website with sitename
                        var existingWebsite = await websiteService.GetWebsiteByName(siteName);
                        if (existingWebsite != null)
                        {
                            var dateTimeNow = DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss");
                            siteName = $"{siteName}-{dateTimeNow}";
                        }

                        var website = new Website 
                        { 
                            User = dbContext.Users.FirstOrDefault(x => x.Id == user.Id), 
                            Name = siteName 
                        };

                        dbContext.Websites.Add(website);
                        dbContext.SaveChanges();

                        await siteCreator.CreateSite(website.Name);

                        subscription = new shared.Data.Entities.Subscription()
                        {
                            StripeCustomerId = stripeCustomer,
                            StripeSubscriptionId = stripeSubscription,
                            IsActive = true,
                            Website =  dbContext.Websites.FirstOrDefault(x => x.Id == website.Id),
                            SubscriptionModule = null
                        };

                        dbContext.Subscriptions.Add(subscription);
                        dbContext.SaveChanges();

                        //send a welcome email with password reset link
                        var passwordResetCode = await userManager.GeneratePasswordResetTokenAsync(user);
                        passwordResetCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordResetCode));

                        var passwordResetPageUrl = $"{StaticStrings.DefaultSiteName}/{StaticStrings.DefaultEnLang}/{StaticRoutesStrings.AccountResetPasswordPageUrl}";
                        var passwordResetCallbackUrl = $"{context.Request.Scheme}://{context.Request.Host.Value}/{passwordResetPageUrl}?code={passwordResetCode}";

                        await emailSender.SendPasswordResetLinkAsync(user, email, HtmlEncoder.Default.Encode(passwordResetCallbackUrl));

                        //send email confirmation link
                        var emailConfirmationCode = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        emailConfirmationCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationCode));

                        var emailConfirmationPageUrl = $"{StaticStrings.DefaultSiteName}/{StaticStrings.DefaultEnLang}/{StaticRoutesStrings.AccountConfirmEmailPageUrl}";
                        var emailConfirmationCallbackUrl = $"{context.Request.Scheme}://{context.Request.Host.Value}/{emailConfirmationPageUrl}?userId={user.Id}&code={emailConfirmationCode}";

                        await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(emailConfirmationCallbackUrl));


                    }
                    else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted ||
                             stripeEvent.Type == Events.CustomerSubscriptionPaused)
                    {

                    }
                    else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
                    {
                        var data = JsonSerializer.Deserialize<CustomerSubscriptionUpdated.Rootobject>(stripeEvent.Data.ToJson());
                        var subscriptionUpdatedType = data._object._object;// = "subscription";
                        var stripeSubscriptionId = data._object.id;

                        if (subscriptionUpdatedType.Equals("subscription"))
                        {
                            var subscription = dbContext.Subscriptions.FirstOrDefault(s => s.StripeSubscriptionId == stripeSubscriptionId);

                            if (subscription is not null)
                            {
                                var isSubscriptionActive = data._object.status == "active" || data._object.status == "trialing";
                                subscription.IsActive = isSubscriptionActive;

                                dbContext.SaveChanges();
                            }
                        }
                    }
                    else if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
                    {
                        var data = JsonSerializer.Deserialize<CustomerSubscriptionCreated.Rootobject>(stripeEvent.Data.ToJson());
                        var subscriptionUpdatedType = data._object._object;// = "subscription";
                        var stripeSubscriptionId = data._object.id;
                        var stripeSubscribedProductId = data._object.items.data[0].plan.product;

                        if (subscriptionUpdatedType.Equals("subscription"))
                        {
                            var subscription = dbContext.Subscriptions.FirstOrDefault(s => s.StripeSubscriptionId == stripeSubscriptionId);

                            if (subscription is not null)
                            {
                                var module = dbContext.SubscriptionStripeInfos.FirstOrDefault(m => m.Code == stripeSubscribedProductId).SubscriptionModules.FirstOrDefault();
                                subscription.SubscriptionModule = module;

                                dbContext.SaveChanges();
                            }
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

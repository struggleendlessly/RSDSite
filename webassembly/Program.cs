using webassembly;
using webassembly.Handlers;

using shared.Managers;
using shared.Interfaces;
using shared.Managers.Api;
using shared.Interfaces.Api;
using shared.ConfigurationOptions;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection(ApiOptions.SectionName));

builder.Services.AddTransient<CustomAuthorizationMessageHandler>();
builder.Services.AddScoped<IStateManager, StateManager>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IApiPageDataService, ApiPageDataService>();
builder.Services.AddScoped<IApiSubscriptionService, ApiSubscriptionService>();
builder.Services.AddScoped<IApiAzureBlobStorageService, ApiAzureBlobStorageService>();

var apiUrl = builder.Configuration["Api:Url"];
var apiScope = builder.Configuration["Api:Scope"];

builder.Services.AddHttpClient("api", client => client.BaseAddress = new Uri(apiUrl))
    .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(apiScope);
});

await builder.Build().RunAsync();

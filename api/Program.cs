using shared;
using shared.Data;
using shared.Managers;
using shared.Interfaces;
using shared.Models.API;
using shared.ConfigurationOptions;

using api.Endpoints.Public;
using api.Endpoints.Private;

using System.Text.Json.Serialization;

using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddAuthorization();

builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins("http://localhost:5000", "https://localhost:7042")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

builder.Services.AddMemoryCache();

builder.Services.AddScoped<ScriptRunner>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWebsiteService, WebsiteService>();
builder.Services.AddScoped<AzureBlobStorageManager>();
builder.Services.AddScoped<IPageDataService, PageDataService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection(ApiOptions.SectionName));
builder.Services.Configure<AzureBlobStorageOptions>(builder.Configuration.GetSection(AzureBlobStorageOptions.SectionName));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.UseCors("wasm");

//app.UseMiddleware<TokenAuthorizationMiddleware>();

app.MapUserEndpoints();
app.MapWebsiteEndpoints();
app.MapPageDataEndpoints();
app.MapSubscriptionEndpoints();

app.MapGet("/", () => "Hello World!");

app.MapPost(StaticRoutesStrings.APIRunPowerShellScriptRoute, async (RunPowerShellScriptModel model, ScriptRunner scriptRunner) =>
{
    var result = await scriptRunner.RunPowerShellScriptAsync(model);
    if (result.Output.Contains("BadRequest"))
    {
        return new RunPowerShellScriptResponseModel { Success = false };
    }
    else
    {
        return new RunPowerShellScriptResponseModel { Success = true };
    }
});

app.Run();

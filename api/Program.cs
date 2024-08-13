using shared;
using api.Middleware;
using shared.Managers;
using shared.Models.API;
using shared.ConfigurationOptions;
using api.Endpoints.Public;
using shared.Interfaces;
using shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using shared.Data.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

IdentityModelEventSource.ShowPII = true;

//builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.SameSite = SameSiteMode.None;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//});

//builder.Services.AddAuthorizationBuilder();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowBlazorClient", builder =>
//    {
//        builder.WithOrigins("https://localhost:7237")
//               .AllowAnyHeader()
//               .AllowAnyMethod()
//               .AllowCredentials();
//    });
//});

builder.Services.AddScoped<ScriptRunner>();

builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection(ApiOptions.SectionName));

builder.Services.AddMemoryCache();

builder.Services.Configure<AzureBlobStorageOptions>(builder.Configuration.GetSection(AzureBlobStorageOptions.SectionName));

builder.Services.AddScoped<AzureBlobStorageManager>();
builder.Services.AddScoped<IPageDataService, PageDataService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Transient);

//builder.Services.AddIdentityCore<ApplicationUser>()
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddApiEndpoints();

builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:5001",
            builder.Configuration["FrontendUrl"] ?? "https://localhost:5002"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

//builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

//app.MapIdentityApi<ApplicationUser>();

app.UseCors("wasm");

//app.UseHttpsRedirection();
//app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.UseMiddleware<TokenAuthorizationMiddleware>();

app.MapPageDataEndpoints();

app.MapGet("/hello", () => "Hello World!").RequireAuthorization();

//app.MapGet("/roles", (ClaimsPrincipal user) =>
//{
//    if (user.Identity is not null && user.Identity.IsAuthenticated)
//    {
//        var identity = (ClaimsIdentity)user.Identity;
//        var roles = identity.FindAll(identity.RoleClaimType)
//            .Select(c =>
//                new
//                {
//                    c.Issuer,
//                    c.OriginalIssuer,
//                    c.Type,
//                    c.Value,
//                    c.ValueType
//                });

//        return TypedResults.Json(roles);
//    }

//    return Results.Unauthorized();
//}).RequireAuthorization();

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

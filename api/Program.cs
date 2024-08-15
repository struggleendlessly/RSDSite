using shared;
using shared.Data;
using shared.Managers;
using shared.Interfaces;
using shared.Models.API;
using shared.ConfigurationOptions;

using api.Endpoints.Private;

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

builder.Services.AddScoped<ScriptRunner>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection(ApiOptions.SectionName));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseCors("wasm");

//app.UseMiddleware<TokenAuthorizationMiddleware>();

app.MapUserEndpoints();

app.MapGet("/", () => "Hello World!");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (HttpContext httpContext) =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.RequireAuthorization();

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

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
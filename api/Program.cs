using shared;
using api.Middleware;
using shared.Managers;
using shared.Models.API;
using shared.ConfigurationOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ScriptRunner>();

builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection(ApiOptions.SectionName));

var app = builder.Build();

app.UseMiddleware<TokenAuthorizationMiddleware>();

app.MapGet("/", () => "Hello World!");

app.MapPost(StaticRoutesStrings.APIRunPowerShellScriptRoute, async (RunPowerShellScriptModel model, ScriptRunner scriptRunner) =>
{
    var result = await scriptRunner.RunPowerShellScriptAsync(model);
    var response = new RunPowerShellScriptResponseModel()
    {
        Success = true
    };

    return response;
});

app.Run();

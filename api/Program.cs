using shared;
using shared.Managers;
using shared.Models.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ScriptRunner>();

var app = builder.Build();

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

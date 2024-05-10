using shared.Models.API;

namespace shared.Interfaces
{
    public interface IScriptRunner
    {
        Task<RunPowerShellScriptResponseModel> RunPowerShellScriptAsync(RunPowerShellScriptModel model);
    }
}

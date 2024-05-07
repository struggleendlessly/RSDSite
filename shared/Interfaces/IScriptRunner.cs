using System;

namespace shared.Interfaces
{
    public interface IScriptRunner
    {
        Task<string> RunPowerShellScriptAsync(string scriptFilePath, params (string, string)[] parameters);
    }
}

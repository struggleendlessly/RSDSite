using System;

namespace shared.Models.API
{
    public class RunPowerShellScriptModel
    {
        public string ScriptName { get; set; } = string.Empty;
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}

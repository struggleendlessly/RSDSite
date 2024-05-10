using System;

namespace shared.Models.API
{
    public class RunPowerShellScriptResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public string Output { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}

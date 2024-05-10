using shared.Interfaces;
using shared.Models.API;
using System.Diagnostics;

namespace shared.Managers
{
    public class ScriptRunner : IScriptRunner
    {
        public async Task<RunPowerShellScriptResponseModel> RunPowerShellScriptAsync(RunPowerShellScriptModel model)
        {
            var scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, StaticStrings.PowerShellScriptsPath, model.ScriptName);
            string arguments = $"-File \"{scriptPath}\"";

            foreach (var parameter in model.Parameters)
            {
                arguments += $" -{parameter.Key} \"{parameter.Value}\"";
            }

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = psi;
                process.Start();

                Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                Task<string> errorTask = process.StandardError.ReadToEndAsync();

                await Task.WhenAll(outputTask, errorTask);

                string output = outputTask.Result;
                string error = errorTask.Result;

                Console.WriteLine("Output: " + output);
                Console.WriteLine("Error: " + error);

                var response = new RunPowerShellScriptResponseModel
                {
                    Output = output,
                    Error = error
                };

                return response;
            }
        }
    }
}

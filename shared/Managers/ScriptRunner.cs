using shared.Interfaces;
using System.Diagnostics;

namespace shared.Managers
{
    public class ScriptRunner : IScriptRunner
    {
        public async Task<string> RunPowerShellScriptAsync(string scriptFilePath, params (string, string)[] parameters)
        {
            // Create argument string dynamically
            string arguments = $"-File \"{scriptFilePath}\"";

            // Add parameters to the argument string
            foreach (var parameter in parameters)
            {
                arguments += $" -{parameter.Item1} \"{parameter.Item2}\"";
            }

            // Create a process to run PowerShell
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            // Start the process asynchronously
            using (Process process = new Process())
            {
                process.StartInfo = psi;
                process.Start();

                // Read output and error streams asynchronously
                Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                Task<string> errorTask = process.StandardError.ReadToEndAsync();

                await Task.WhenAll(outputTask, errorTask);

                string output = outputTask.Result;
                string error = errorTask.Result;

                // Display output and error messages
                Console.WriteLine("Output: " + output);
                Console.WriteLine("Error: " + error);

                return output;
            }
        }
    }
}

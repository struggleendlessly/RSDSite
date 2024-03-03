using System.Diagnostics;

namespace shared.Managers
{
    public class ScriptRunner
    {
        public void RunPowerShellScript(string scriptFilePath, params (string, string)[] parameters)
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

            // Start the process
            using (Process process = Process.Start(psi))
            {
                if (process != null)
                {
                    // Read output and error streams
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    // Display output and error messages
                    Console.WriteLine("Output: " + output);
                    Console.WriteLine("Error: " + error);
                }
            }
        }
    }
}

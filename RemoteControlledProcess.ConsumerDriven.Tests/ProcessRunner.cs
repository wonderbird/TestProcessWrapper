using System.Diagnostics;
using Xunit.Abstractions;

namespace RemoteControlledProcess.ConsumerDriven.Tests
{
    public static class ProcessRunner
    {
        public static string RunProcess(string processName, string arguments, ITestOutputHelper testOutputHelper)
        {
            var processStartInfo = new ProcessStartInfo(processName)
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = arguments
            };

            var process = new Process { StartInfo = processStartInfo };
            process.Start();
            process.WaitForExit(30000);

            var output = process.StandardOutput.ReadToEnd();
            testOutputHelper.WriteLine($"Process produced the following output: \"{output}\"");

            return output;
        }
    }
}
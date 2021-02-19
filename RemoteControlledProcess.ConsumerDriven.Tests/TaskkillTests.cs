using System.Diagnostics;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace RemoteControlledProcess.ConsumerDriven.Tests
{
    public class TaskkillTests
    {
        private ITestOutputHelper _testOutputHelper;

        public TaskkillTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;
        
        [Fact]
        public void RunTaskkillProcess_OnWindows_ShowsProcessOutput()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _testOutputHelper.WriteLine($"Non-Windows OS detected. Skipping test RunTaskkillProcess_OnWindows_ShowsProcessOutput.");
                return;
            }

            var processName = "taskkill";
            var arguments = "/?";

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
            _testOutputHelper.WriteLine($"Process produced the following output: \"{output}\"");
            Assert.Contains("TASKKILL", output);
            Assert.Contains("/PID", output);
        }
    }
}

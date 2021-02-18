using System.Diagnostics;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace RemoteControlledProcess.Taskkill.Tests
{
    public class TaskkillTests
    {
        private ITestOutputHelper _testOutputHelper;

        public TaskkillTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Fact]
        public void RunTaskkillProcess_OnWindows_ShowsProcessOutput()
        {
            string processName;
            string arguments;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                processName = "taskkill";
                arguments = "/?";
            }
            else
            {
                processName = "kill";
                arguments = "-l";
            }

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

            _testOutputHelper.WriteLine($"Process produced the following output: \"{process.StandardOutput.ReadToEnd()}\"");
        }
    }
}

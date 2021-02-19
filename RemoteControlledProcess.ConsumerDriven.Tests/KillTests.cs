using System.Diagnostics;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace RemoteControlledProcess.ConsumerDriven.Tests
{
    public class KillTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public KillTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Fact]
        public void RunKillProcess_NotOnWindows_ShowsProcessOutput()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _testOutputHelper.WriteLine($"Windows OS detected. Skipping test RunKillProcess_NotOnWindows_ShowsProcessOutput.");
                return;
            }

            var processName = "kill";
            var arguments = "-l";

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
            Assert.Contains("term", output);
        }
    }
}

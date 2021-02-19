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

            var output = ProcessRunner.RunProcess("kill", "-l", _testOutputHelper);
            
            Assert.Contains("term", output);
        }
    }
}

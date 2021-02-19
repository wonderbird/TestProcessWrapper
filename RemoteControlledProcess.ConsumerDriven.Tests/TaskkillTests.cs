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

            var output = ProcessRunner.RunProcess("taskkill", "/?", _testOutputHelper);

            Assert.Contains("TASKKILL", output);
            Assert.Contains("/PID", output);
        }
    }
}

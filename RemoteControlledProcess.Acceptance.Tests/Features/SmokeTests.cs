using Xunit;
using Xunit.Abstractions;

namespace RemoteControlledProcess.Acceptance.Tests.Features
{
    public class SmokeTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SmokeTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        /// <summary>
        ///     SmokeTest used to verify that the NuGet package has been created correctly.
        /// </summary>
        /// <remarks>
        ///     This test is also used by smoketest.sh in the solution folder
        ///     RemoteControlledProcess.Nupkg.Tests
        /// </remarks>
        [Fact]
        public void SmokeTest()
        {
            var processWrapper = new ProcessWrapper("RemoteControlledProcess.Application", false);
            processWrapper.Start();
            processWrapper.ShutdownGracefully();
            processWrapper.ForceTermination();

            var output = processWrapper.ReadOutput();
            _testOutputHelper.WriteLine($"Process produced the following output: \"{output}\"");
            Assert.Contains("Process ID", output);
        }
    }
}
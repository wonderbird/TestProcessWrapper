using RemoteControlledProcess;
using Xunit;

namespace katarabbitmq.bdd.tests.Features
{
    public class SmokeTests
    {
        /// <summary>
        /// SmokeTest used to verify that the NuGet package has been created correctly.
        /// </summary>
        /// <remarks>
        /// This test is also used by smoketest.sh in the solution folder
        /// RemoteControlledProcess.Nupkg.Tests
        /// </remarks>
        [Fact]
        public void SmokeTest()
        {
            var processWrapper = new ProcessWrapper("RemoteControlledProcess.Application", false);
            processWrapper.Start();
            processWrapper.ShutdownGracefully();
            processWrapper.ForceTermination();
            var output = processWrapper.ReadOutput();
            Assert.Contains("STOP", output);
        }
    }
}
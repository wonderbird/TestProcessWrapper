using Xunit;

namespace TestProcessWrapper.Acceptance.Tests.Features
{
    public class SmokeTests
    {
        /// <summary>
        ///     SmokeTest used to verify that the NuGet package has been created correctly.
        /// </summary>
        /// <remarks>
        ///     This test is also used by smoketest.sh in the solution folder
        ///     TestProcessWrapper.Nupkg.Tests
        /// </remarks>
        [Fact]
        public void SmokeTest()
        {
            var processWrapper = new TestProcessWrapper(
                "TestProcessWrapper.LongLived.Application",
                false
            );
            processWrapper.Start();
            processWrapper.ShutdownGracefully();
            processWrapper.ForceTermination();
            Assert.Contains("STOP", processWrapper.RecordedOutput);
        }
    }
}

using Xunit;

namespace RemoteControlledProcess.Unit.Tests
{
    public class ProcessWrapperTest
    {
        [Fact]
        public void Start_CustomReadinessCheckReturnsFalse_RepeatsReadinessCheck()
        {
            var processWrapper = new ProcessWrapper("someProject", false);
            processWrapper.Start();
        }
    }
}
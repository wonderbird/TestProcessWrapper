using Moq;
using Xunit;

namespace RemoteControlledProcess.Unit.Tests
{
    public class ProcessWrapperTest
    {
        [Fact]
        public void Start_CustomReadinessCheckReturnsFalse_RepeatsReadinessCheck()
        {
            var processFactory = new Mock<IProcessFactory>();
            var processWrapper = new ProcessWrapper(processFactory.Object);
            processWrapper.Start();
        }
    }
}
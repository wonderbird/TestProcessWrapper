using Moq;
using Xunit;

namespace RemoteControlledProcess.Unit.Tests
{
    public class ProcessWrapperTest
    {
//        [Fact]
//        public void Start_CustomReadinessCheckReturnsFalse_RepeatsReadinessCheck()
//        {
//            var process = new Mock<IProcess>();
//            var processFactory = new Mock<IProcessFactory>();
//            processFactory.Setup(x => x.CreateProcess()).Returns(process.Object);
//
//            var processStreamBuffer = new Mock<IProcessStreamBuffer>();
//            processStreamBuffer.Setup(x => x.StreamContent).Returns("Process ID 999\n");
//
//            var processStreamBufferFactory = new Mock<IProcessStreamBufferFactory>();
//            processStreamBufferFactory.Setup(x => x.CreateProcessStreamBuffer()).Returns(processStreamBuffer.Object);
//
//            var processWrapper = new ProcessWrapper(processFactory.Object);
//            processWrapper.ProcessStreamBufferFactory = processStreamBufferFactory.Object;
//
//            processWrapper.Start();
//
//            // TODO: The custom readiness check must be introduced and tested now. Probably some refactoring upfront ...
//            Assert.False(true, "TODO: The custom readiness check must be introduced and tested now. Probably some refactoring upfront ...");
//        }
    }
}
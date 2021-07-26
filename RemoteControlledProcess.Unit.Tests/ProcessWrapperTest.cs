using Moq;
using Xunit;

namespace RemoteControlledProcess.Unit.Tests
{
    public class ProcessWrapperTest
    {
        private class FirstFailingThenSucceedingReadinessCheck
        {
            public int NumberOfCalls { get; private set; }

            public bool Execute(string _)
            {
                NumberOfCalls++;
                return NumberOfCalls != 1;
            }
        }

        [Fact]
        public void Start_CustomReadinessCheckReturnsFalse_RepeatsReadinessCheck()
        {
            // TODO make test more readable
            var process = new Mock<IProcess>();
            var processFactory = new Mock<IProcessFactory>();
            processFactory.Setup(x => x.CreateProcess()).Returns(process.Object);

            var processStreamBuffer = new Mock<IProcessStreamBuffer>();
            processStreamBuffer.Setup(x => x.StreamContent).Returns("Process ID 999\n");

            var processStreamBufferFactory = new Mock<IProcessStreamBufferFactory>();
            processStreamBufferFactory.Setup(x => x.CreateProcessStreamBuffer()).Returns(processStreamBuffer.Object);

            var processWrapper = new TestProcessWrapper(processFactory.Object, processStreamBufferFactory.Object);

            var customReadinessCheck = new FirstFailingThenSucceedingReadinessCheck();
            processWrapper.AddReadinessCheck(processOutput => customReadinessCheck.Execute(processOutput));

            processWrapper.Start();

            Assert.Equal(2, customReadinessCheck.NumberOfCalls);
        }
    }
}
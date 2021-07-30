using Moq;
using Xunit;

namespace RemoteControlledProcess.Unit.Tests
{
    public class ProcessWrapperTest
    {
        [Fact]
        public void Start_EnvironmentVariablesConfigured_AllVariablesPassedToProcess()
        {
            var process = new Mock<IProcess>();
            var processFactory = new Mock<IProcessFactory>();
            processFactory.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<bool>())).Returns(process.Object);

            var processOutputRecorder = new Mock<IProcessOutputRecorder>();
            processOutputRecorder.Setup(x => x.Output).Returns("Process ID 999\n");

            var processOutputRecorderFactory = new Mock<IProcessOutputRecorderFactory>();
            processOutputRecorderFactory.Setup(x => x.Create()).Returns(processOutputRecorder.Object);

            var processWrapper = new TestProcessWrapper(processFactory.Object, processOutputRecorderFactory.Object);

            processWrapper.Start();

            process.Verify(p => p.AddEnvironmentVariable(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(0));
        }

        [Fact]
        public void Start_CustomReadinessCheckReturnsFalse_RepeatsReadinessCheck()
        {
            var process = new Mock<IProcess>();
            var processFactory = new Mock<IProcessFactory>();
            processFactory.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<bool>())).Returns(process.Object);

            var processOutputRecorder = new Mock<IProcessOutputRecorder>();
            processOutputRecorder.Setup(x => x.Output).Returns("Process ID 999\n");

            var processOutputRecorderFactory = new Mock<IProcessOutputRecorderFactory>();
            processOutputRecorderFactory.Setup(x => x.Create()).Returns(processOutputRecorder.Object);

            var processWrapper = new TestProcessWrapper(processFactory.Object, processOutputRecorderFactory.Object);

            var customReadinessCheck = new FirstFailingThenSucceedingReadinessCheck();
            processWrapper.AddReadinessCheck(processOutput => customReadinessCheck.Execute(processOutput));

            processWrapper.Start();

            Assert.Equal(2, customReadinessCheck.NumberOfCalls);
        }

        private class FirstFailingThenSucceedingReadinessCheck
        {
            public int NumberOfCalls { get; private set; }

            public bool Execute(string _)
            {
                NumberOfCalls++;
                return NumberOfCalls != 1;
            }
        }
    }
}
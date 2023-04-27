using Moq;
using Xunit;

namespace RemoteControlledProcess.Unit.Tests
{
    public class ProcessWrapperTest
    {
        private readonly Mock<IProcess> _process;
        private readonly Mock<IProcessFactory> _processFactory;
        private readonly Mock<IProcessOutputRecorderFactory> _processOutputRecorderFactory;

        public ProcessWrapperTest()
        {
            _process = new Mock<IProcess>();

            _processFactory = new Mock<IProcessFactory>();
            _processFactory
                .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(_process.Object);

            var processOutputRecorder = new Mock<IProcessOutputRecorder>();
            processOutputRecorder.Setup(x => x.Output).Returns("Process ID 999\n");

            _processOutputRecorderFactory = new Mock<IProcessOutputRecorderFactory>();
            _processOutputRecorderFactory
                .Setup(x => x.Create())
                .Returns(processOutputRecorder.Object);
        }

        [Fact]
        public void Start_NoEnvironmentVariablesConfigured_NoVariablePassedToProcess()
        {
            var processWrapper = new TestProcessWrapper(
                _processFactory.Object,
                _processOutputRecorderFactory.Object
            );

            processWrapper.Start();

            _process.Verify(
                p => p.AddEnvironmentVariable(It.IsAny<string>(), It.IsAny<string>()),
                Times.Exactly(0)
            );
        }

        [Fact]
        public void Start_CustomReadinessCheckReturnsFalse_RepeatsReadinessCheck()
        {
            var processWrapper = new TestProcessWrapper(
                _processFactory.Object,
                _processOutputRecorderFactory.Object
            );

            var customReadinessCheck = new FirstFailingThenSucceedingReadinessCheck();
            processWrapper.AddReadinessCheck(
                processOutput => customReadinessCheck.Execute(processOutput)
            );

            processWrapper.Start();

            Assert.Equal(2, customReadinessCheck.NumberOfCalls);
        }

        private sealed class FirstFailingThenSucceedingReadinessCheck
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

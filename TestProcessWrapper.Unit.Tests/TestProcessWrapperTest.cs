using Moq;
using Xunit;

namespace TestProcessWrapper.Unit.Tests;

public class TestProcessWrapperTest
{
    private readonly Mock<IProcessOutputRecorderFactory> _processOutputRecorderFactory;
    private readonly Mock<ITestProcessBuilderFactory> _testProcessBuilderFactory;

    public TestProcessWrapperTest()
    {
        var process = new Mock<ITestProcess>();

        var testProcessBuilder = new Mock<TestProcessBuilder>();
        testProcessBuilder.Setup(x => x.Build()).Returns(process.Object);

        _testProcessBuilderFactory = new Mock<ITestProcessBuilderFactory>();
        _testProcessBuilderFactory
            .Setup(
                x =>
                    x.CreateBuilder(
                        It.IsAny<string>(),
                        It.IsAny<BuildConfiguration>(),
                        It.IsAny<bool>()
                    )
            )
            .Returns(testProcessBuilder.Object);

        var processOutputRecorder = new Mock<IProcessOutputRecorder>();
        processOutputRecorder.Setup(x => x.Output).Returns("Process ID 999\n");

        _processOutputRecorderFactory = new Mock<IProcessOutputRecorderFactory>();
        _processOutputRecorderFactory.Setup(x => x.Create()).Returns(processOutputRecorder.Object);
    }

    [Fact]
    public void Start_CustomReadinessCheckReturnsFalse_RepeatsReadinessCheck()
    {
        var processWrapper = new TestProcessWrapper(
            _testProcessBuilderFactory.Object,
            _processOutputRecorderFactory.Object
        );

        var customReadinessCheck = new FirstFailingThenSucceedingReadinessCheck();
        processWrapper.AddReadinessCheck(processOutput => customReadinessCheck.Execute());

        processWrapper.Start();

        Assert.Equal(2, customReadinessCheck.NumberOfCalls);
    }

    private sealed class FirstFailingThenSucceedingReadinessCheck
    {
        public int NumberOfCalls { get; private set; }

        public bool Execute()
        {
            NumberOfCalls++;
            return NumberOfCalls != 1;
        }
    }
}

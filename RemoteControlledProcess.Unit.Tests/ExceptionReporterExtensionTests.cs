using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace RemoteControlledProcess.Unit.Tests
{
    public class ExceptionReporterExtensionTests
    {
        private const string ExceptionMessage = "Exception generated for test purpose";
        private ITestOutputHelper _testOutputHelper;

        public ExceptionReporterExtensionTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Theory]
        [InlineData(0,
            @"Unhandled exception in .*RemoteControlledProcess\.Unit\.Tests[\/\\]ExceptionReporterExtensionTests\.cs:[0-9]+")]
        [InlineData(1, ExceptionMessage)]
        public void Write_CalledInCatchBlock_WrittenMessagesMatch(int invocationIndex, string expectedMessageRegex)
        {
            // Arrange
            var writerMock = new Mock<TextWriter>();

            try
            {
                throw new UnitTestException(ExceptionMessage);
            }
            catch (UnitTestException e)
            {
                // Act
                e.Write(writerMock.Object);
            }

            // Assert
            var actualMessage = (string)writerMock.Invocations[invocationIndex].Arguments[0];

            _testOutputHelper.WriteLine("===== Expected Message Regex:");
            _testOutputHelper.WriteLine($"\"{expectedMessageRegex}\"");
            _testOutputHelper.WriteLine("===== Actual Message:");
            _testOutputHelper.WriteLine($"\"{actualMessage}\"");

            Assert.Matches(expectedMessageRegex, actualMessage);
        }

        [Fact]
        public void Log_CalledInCatchBlock_LogsCriticalMessageWithExpectedRegex()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();

            try
            {
                throw new UnitTestException(ExceptionMessage);
            }
            catch (UnitTestException e)
            {
                // Act
                e.Log(loggerMock.Object);
            }

            // Assert
            var expectedMessageRegex =
                @"Unhandled exception in .*RemoteControlledProcess\.Unit\.Tests[\/\\]ExceptionReporterExtensionTests\.cs:[0-9]+";
            var actualLogLevel = (LogLevel)loggerMock.Invocations[0].Arguments[0];
            var actualMessage = loggerMock.Invocations[0].Arguments[2].ToString();

            _testOutputHelper.WriteLine("===== Expected Message Regex:");
            _testOutputHelper.WriteLine($"\"{expectedMessageRegex}\"");
            _testOutputHelper.WriteLine("===== Actual Message:");
            _testOutputHelper.WriteLine($"\"{actualMessage}\"");

            _testOutputHelper.WriteLine("===== Expected LogLevel:");
            _testOutputHelper.WriteLine($"\"{LogLevel.Critical}\"");
            _testOutputHelper.WriteLine("===== Actual LogLevel:");
            _testOutputHelper.WriteLine($"\"{actualLogLevel}\"");

            Assert.Equal(LogLevel.Critical, actualLogLevel);
            Assert.Matches(expectedMessageRegex, actualMessage);
        }
    }
}
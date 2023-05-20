using System.IO;
using MELT;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TestProcessWrapper.Unit.Tests;

public class ExceptionReporterExtensionTest
{
    private const string ExceptionMessage = "Exception generated for test purpose";

    [Theory]
    [InlineData(
        0,
        "Unhandled exception in .*TestProcessWrapper.Unit.Tests/ExceptionReporterExtensionTest.cs:[0-9]+"
    )]
    [InlineData(1, ExceptionMessage)]
    public void Write_CalledInCatchBlock_WrittenMessagesMatch(
        int invocationIndex,
        string expectedMessageRegex
    )
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
        Assert.Matches(expectedMessageRegex, actualMessage);
    }

    [Fact]
    public void Log_CalledInCatchBlock_LogsCriticalMessageWithExpectedRegex()
    {
        // Arrange
        var loggerFactory = TestLoggerFactory.Create();
        var logger = loggerFactory.CreateLogger<ExceptionReporterExtensionTest>();

        try
        {
            throw new UnitTestException(ExceptionMessage);
        }
        catch (UnitTestException e)
        {
            // Act
            e.Log(logger);
        }

        // Assert
        var expectedMessageRegex =
            "Unhandled exception in .*TestProcessWrapper.Unit.Tests/ExceptionReporterExtensionTest.cs:[0-9]+";
        var log = Assert.Single(loggerFactory.Sink.LogEntries);
        Assert.Equal(LogLevel.Critical, log.LogLevel);
        Assert.Matches(expectedMessageRegex, log.Message);
    }
}

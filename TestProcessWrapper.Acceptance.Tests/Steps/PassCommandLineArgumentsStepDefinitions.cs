using TechTalk.SpecFlow;
using TestProcessWrapper.Acceptance.Tests.Steps.Common;
using Xunit;

namespace TestProcessWrapper.Acceptance.Tests.Steps;

[Binding]
public class StepDefinitions
{
    private const string AllowedCharactersForArgument = "[-a-zA-Z0-9]*";
    private string _outputWhenReady;

    [Given($"the command line argument '({AllowedCharactersForArgument})=({AllowedCharactersForArgument})' has been configured")]
    public void GivenTheCommandLineArgumentHasBeenConfigured(string argument, string value)
    {
        var client = SingleProcessControlStepDefinitions.Client;
        client.AddCommandLineArgument(argument, value);
        client.AddReadinessCheck(CaptureOutput);
    }

    [Given($"the command line argument '({AllowedCharactersForArgument})' has been configured")]
    public void GivenTheCommandLineArgumentHasBeenConfigured(string argument)
    {
        var client = SingleProcessControlStepDefinitions.Client;
        client.AddCommandLineArgument(argument);
        client.AddReadinessCheck(CaptureOutput);
    }

    [Then($"the application has received the command line argument '({AllowedCharactersForArgument})' with value '({AllowedCharactersForArgument})'")]
    public void ThenTheApplicationHasReceivedTheCommandLineArgumentWithValue(string argument, string value)
    {
        Assert.Contains($"Received the command line argument '{argument}={value}'", _outputWhenReady);
    }

    [Then($"the application has received the command line argument '({AllowedCharactersForArgument})'")]
    public void ThenTheApplicationHasReceivedTheCommandLineArgument(string argument)
    {
        Assert.Contains($"Received the command line argument '{argument}'", _outputWhenReady);
    }

    private bool CaptureOutput(string output)
    {
        _outputWhenReady = output;
        return true;
    }
}

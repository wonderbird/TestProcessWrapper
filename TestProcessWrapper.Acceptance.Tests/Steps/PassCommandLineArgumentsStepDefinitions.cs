using TechTalk.SpecFlow;
using TestProcessWrapper.Acceptance.Tests.Steps.Common;
using Xunit;

namespace TestProcessWrapper.Acceptance.Tests.Steps;

[Binding]
public class StepDefinitions
{
    private string _outputWhenReady;

    [Given(@"the command line argument '([-a-zA-Z0-9]*)=([-a-zA-Z0-9]*)' has been configured")]
    public void GivenTheCommandLineArgumentHasBeenConfigured(string argument, string value)
    {
        var client = SingleProcessControlStepDefinitions.Client;
        client.AddCommandLineArgument(argument, value);
        client.AddReadinessCheck(output =>
        {
            _outputWhenReady = output;
            return true;
        });
    }

    [Given(@"the command line argument '([-a-zA-Z0-9]*)' has been configured")]
    public void GivenTheCommandLineArgumentHasBeenConfigured(string argument)
    {
        var client = SingleProcessControlStepDefinitions.Client;
        client.AddCommandLineArgument(argument);
        client.AddReadinessCheck(output =>
        {
            _outputWhenReady = output;
            return true;
        });
    }

    [Then(@"the application has received the command line argument '([-a-zA-Z0-9]*)' with value '([-a-zA-Z0-9]*)'")]
    public void ThenTheApplicationHasReceivedTheCommandLineArgumentWithValue(string argument, string value)
    {
        Assert.Contains($"Received the command line argument '{argument}={value}'", _outputWhenReady);
    }

    [Then(@"the application has received the command line argument '([-a-zA-Z0-9]*)'")]
    public void ThenTheApplicationHasReceivedTheCommandLineArgument(string argument)
    {
        Assert.Contains($"Received the command line argument '{argument}'", _outputWhenReady);
    }
}

using TechTalk.SpecFlow;
using TestProcessWrapper.Acceptance.Tests.Steps.Common;
using Xunit;

namespace TestProcessWrapper.Acceptance.Tests.Steps;

[Binding]
public class StepDefinitions
{
    private string _outputWhenReady;

    [Given(@"the command line argument '(.*)=(.*)' has been configured")]
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

    [Then(@"the application has received the command line argument")]
    public void ThenTheApplicationHasReceivedTheCommandLineArgument()
    {
        Assert.Contains("Received the command line argument '--help=true'", _outputWhenReady);
    }
}

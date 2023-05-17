using TechTalk.SpecFlow;
using TestProcessWrapper.Acceptance.Tests.Steps.Common;
using Xunit;

namespace TestProcessWrapper.Acceptance.Tests.Steps;

[Binding]
public sealed class MatchBuildConfigurationStepDefinitions
{
    private string _outputWhenReady;

    [Given(@"the build configuration '(.*)' has been configured")]
    public void GivenTheBuildConfigurationHasBeenConfigured(string configuration)
    {
        var client = SingleProcessControlStepDefinitions.Client;
        client.SelectBuildConfiguration(
            configuration == "Debug" ? BuildConfiguration.Debug : BuildConfiguration.Release
        );
        client.AddReadinessCheck(CaptureOutput);
    }

    [Then(@"the application was launched from the '(.*)' folder")]
    public void ThenTheApplicationWasLaunchedFromTheFolder(string configuration)
    {
        Assert.Contains($"Build configuration: '{configuration}'", _outputWhenReady);
    }

    private bool CaptureOutput(string output)
    {
        _outputWhenReady = output;
        return true;
    }
}

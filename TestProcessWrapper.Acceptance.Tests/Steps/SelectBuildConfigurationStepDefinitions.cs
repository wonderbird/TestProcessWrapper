using TechTalk.SpecFlow;
using TestProcessWrapper.Acceptance.Tests.Steps.Common;
using Xunit;

namespace TestProcessWrapper.Acceptance.Tests.Steps;

[Binding]
public sealed class SelectBuildConfigurationStepDefinitions
{
    private string _outputWhenReady;

    [Given(@"the build configuration '(Debug|Release)' has been selected")]
    public void GivenTheBuildConfigurationHasBeenSelected(BuildConfiguration buildConfiguration)
    {
        var client = SingleProcessControlStepDefinitions.Client;
        client.SelectBuildConfiguration(buildConfiguration);
        client.AddReadinessCheck(CaptureOutput);
    }

    [Then(@"the application was launched from the '(Debug|Release)' folder")]
    public void ThenTheApplicationWasLaunchedFromTheFolder(string configurationString)
    {
        Assert.Contains($"Build configuration: '{configurationString}'", _outputWhenReady);
    }

    private bool CaptureOutput(string output)
    {
        _outputWhenReady = output;
        return true;
    }
}

using TechTalk.SpecFlow;

namespace TestProcessWrapper.Acceptance.Tests.Steps;

[Binding]
public sealed class MatchBuildConfigurationStepDefinitions
{
    [Given(@"the build configuration '(.*)' has been configured")]
#pragma warning disable CA1822
    public void GivenTheBuildConfigurationHasBeenConfigured(string release)
#pragma warning restore CA1822
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"the application was launched from the '(.*)' folder")]
#pragma warning disable CA1822
    public void ThenTheApplicationWasLaunchedFromTheFolder(string release)
#pragma warning restore CA1822
    {
        ScenarioContext.StepIsPending();
    }
}

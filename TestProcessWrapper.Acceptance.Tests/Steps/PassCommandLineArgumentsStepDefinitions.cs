using TechTalk.SpecFlow;

namespace TestProcessWrapper.Acceptance.Tests.Steps;

[Binding]
public class StepDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public StepDefinitions(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

    [Given(@"the command line argument '(.*)' has been configured")]
    public void GivenTheCommandLineArgumentHasBeenConfigured(string commandLineArgument)
    {
        _scenarioContext.Pending();
    }

    [Then(@"the application has received the command line argument")]
    public void ThenTheApplicationHasReceivedTheCommandLineArgument(string expectedPhrase)
    {
        _scenarioContext.Pending();
    }
}

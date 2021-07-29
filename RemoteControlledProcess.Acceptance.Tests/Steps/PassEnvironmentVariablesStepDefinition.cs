using TechTalk.SpecFlow;

namespace RemoteControlledProcess.Acceptance.Tests.Steps
{
    [Binding]
    public class PassEnvironmentVariablesStepDefinition
    {
        private readonly ScenarioContext _scenarioContext;

        public PassEnvironmentVariablesStepDefinition(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        [Given(@"environment variables have been configured")]
        public void GivenEnvironmentVariablesHaveBeenConfigured()
        {
            _scenarioContext.Pending();
        }

        [Then(@"the application has received the configured environment variables")]
        public void ThenTheApplicationHasReceivedTheConfiguredEnvironmentVariables()
        {
            _scenarioContext.Pending();
        }
    }
}
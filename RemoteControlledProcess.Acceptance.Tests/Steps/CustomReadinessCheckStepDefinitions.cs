using TechTalk.SpecFlow;

namespace RemoteControlledProcess.Acceptance.Tests.Steps
{
    [Binding]
    public class CustomReadinessCheckStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public CustomReadinessCheckStepDefinitions(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

        [Given]
        public void GivenAnApplicationWasStartedWithACustomReadinessCheck()
        {
            _scenarioContext.Pending();
        }

        [When]
        public void WhenTheApplicationIsReady()
        {
            _scenarioContext.Pending();
        }

        [Then]
        public void ThenTheCustomReadinessCheckWasExecutedSuccessfully()
        {
            _scenarioContext.Pending();
        }
    }}
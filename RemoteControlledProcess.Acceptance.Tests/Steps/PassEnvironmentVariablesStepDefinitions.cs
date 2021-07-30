using RemoteControlledProcess.Acceptance.Tests.Steps.SharedStepDefinitions;
using TechTalk.SpecFlow;
using Xunit;

namespace RemoteControlledProcess.Acceptance.Tests.Steps
{
    [Binding]
    public class PassEnvironmentVariablesStepDefinitions
    {
        private string _outputWhenReady;
        private const string EnvironmentVariableName = "CONFIGURED_ENVIRONMENT_VARIABLE";
        const string EnvironmentVariableValue = "Test configured environment variable";

        [Given(@"environment variables have been configured")]
        public void GivenEnvironmentVariablesHaveBeenConfigured()
        {
            var client = SingleProcessControlStepDefinitions.Client;
            client.AddEnvironmentVariable(EnvironmentVariableName, EnvironmentVariableValue);
            client.AddReadinessCheck(output =>
            {
                _outputWhenReady = output;
                return true;
            });
        }

        [Then(@"the application has received the configured environment variables")]
        public void ThenTheApplicationHasReceivedTheConfiguredEnvironmentVariables()
        {
            Assert.True(_outputWhenReady.Contains(EnvironmentVariableValue), "Value of environment variable was not printed.");
        }
    }
}
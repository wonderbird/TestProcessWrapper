using System.Collections.Generic;
using TechTalk.SpecFlow;
using TestProcessWrapper.Acceptance.Tests.Steps.Common;
using Xunit;

namespace TestProcessWrapper.Acceptance.Tests.Steps
{
    [Binding]
    public class PassEnvironmentVariablesStepDefinitions
    {
        private readonly Dictionary<string, string> _environmentVariables =
            new()
            {
                {
                    "CONFIGURED_ENVIRONMENT_VARIABLE_1",
                    "Test FIRST configured environment variable"
                },
                {
                    "CONFIGURED_ENVIRONMENT_VARIABLE_2",
                    "Test SECOND configured environment variable"
                }
            };

        private string _outputWhenReady;

        [Given(@"two environment variables have been configured")]
        public void GivenTwoEnvironmentVariablesHaveBeenConfigured()
        {
            var client = SingleProcessControlStepDefinitions.Client;

            foreach (var (name, value) in _environmentVariables)
            {
                client.AddEnvironmentVariable(name, value);
            }

            client.AddReadinessCheck(output =>
            {
                _outputWhenReady = output;
                return true;
            });
        }

        [Then(@"the application has received the configured environment variables")]
        public void ThenTheApplicationHasReceivedTheConfiguredEnvironmentVariables()
        {
            foreach (var (name, value) in _environmentVariables)
            {
                Assert.True(
                    _outputWhenReady.Contains(value),
                    $"Value of environment variable {name} was not printed."
                );
            }
        }
    }
}

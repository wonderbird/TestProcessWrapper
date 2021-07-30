using System;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace RemoteControlledProcess.Acceptance.Tests.Steps
{
    [Binding]
    public class PassEnvironmentVariablesStepDefinitions
    {
        [Given(@"environment variables have been configured")]
        public static void GivenEnvironmentVariablesHaveBeenConfigured()
        {
        }

        [Then(@"the application has received the configured environment variables")]
        public static void ThenTheApplicationHasReceivedTheConfiguredEnvironmentVariables()
        {
            // TODO: Implement the assertion
            Assert.True(false, "Test is not implemented yet.");
        }
    }
}
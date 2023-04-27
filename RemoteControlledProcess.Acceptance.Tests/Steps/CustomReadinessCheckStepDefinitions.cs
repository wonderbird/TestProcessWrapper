using RemoteControlledProcess.Acceptance.Tests.Steps.SharedStepDefinitions;
using TechTalk.SpecFlow;
using Xunit;

namespace RemoteControlledProcess.Acceptance.Tests.Steps
{
    [Binding]
    public class CustomReadinessCheckStepDefinitions
    {
        private bool _isCustomCheckExecuted;

        [Given(@"a custom readiness check was configured")]
        public void GivenACustomReadinessCheckWasConfigured()
        {
            SingleProcessControlStepDefinitions.Client.AddReadinessCheck(_ =>
            {
                _isCustomCheckExecuted = true;
                return true;
            });
        }

        [Then]
        public void ThenTheCustomReadinessCheckWasExecutedSuccessfully()
        {
            Assert.True(_isCustomCheckExecuted, "Custom readiness check has not been executed.");
        }
    }
}

using TechTalk.SpecFlow;
using Xunit;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class DockerShutdownStepDefinitions
    {
        [Given("the server is running")]
        public void GivenTheServerIsRunning()
        {
            Assert.True(Processes.Robot.IsRunning);
        }

        [When("a TERM signal is sent")]
        public void WhenATermSignalIsSent()
        {
            Processes.Robot.SendTermSignal();
        }

        [Then("the application shuts down.")]
        public void TheApplicationShutsDown()
        {
            Assert.True(Processes.Robot.HasExited);
        }
    }
}
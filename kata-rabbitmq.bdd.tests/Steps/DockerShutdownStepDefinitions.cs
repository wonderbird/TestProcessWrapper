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
            Assert.True(RobotProcess.Process.IsRunning);
        }

        [When("a TERM signal is sent")]
        public void WhenATermSignalIsSent()
        {
            RobotProcess.Process.SendTermSignal();
        }

        [Then("the application shuts down.")]
        public void TheApplicationShutsDown()
        {
            Assert.True(RobotProcess.Process.HasExited);
        }
    }
}
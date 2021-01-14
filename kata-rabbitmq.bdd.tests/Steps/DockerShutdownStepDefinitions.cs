using katarabbitmq.bdd.tests.Helpers;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class DockerShutdownStepDefinitions
    {
        private readonly RemoteControlledProcess _robot = new("kata-rabbitmq.robot.app");
        private readonly RemoteControlledProcess _client = new("kata-rabbitmq.client.app");

        public DockerShutdownStepDefinitions(ITestOutputHelper testOutputHelper)
        {
            _robot.TestOutputHelper = testOutputHelper;
            _client.TestOutputHelper = testOutputHelper;
        }

        [Given]
        public void GivenTheServerAndClientAreRunning()
        {
            _robot.Start();
            _client.Start();
        }

        [When]
        public void WhenATermSignalIsSentToBothServerAndClient()
        {
            _robot.ShutdownGracefully();
            _client.ShutdownGracefully();
        }

        [Then]
        public void ThenBothApplicationsShutDown()
        {
            Assert.True(_robot.HasExited);
            Assert.True(_client.HasExited);
        }

        [AfterScenario("DockerShutdown")]
        public void StopProcesses()
        {
            _robot?.ForceTermination();
            _client?.ForceTermination();
        }
    }
}
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class DockerShutdownStepDefinitions
    {
        [Given("the server is running")]
        public void GivenTheServerIsRunning()
        {
            Assert.True(RobotProcess.IsRunning);
        }

        [When("a TERM signal is sent")]
        public void WhenATermSignalIsSent()
        {
            RobotProcess.SendTermSignal();
        }

        [Then("the application shuts down.")]
        public void TheApplicationShutsDown()
        {
            Assert.True(RobotProcess.HasExited);
        }
    }
}
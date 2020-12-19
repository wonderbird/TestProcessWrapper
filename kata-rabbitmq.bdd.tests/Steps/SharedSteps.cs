using katarabbitmq.bdd.tests.Helpers;
using TechTalk.SpecFlow;
using Xunit;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class SharedSteps
    {
        [Given("the server and client are running")]
        public static void GivenTheServerAndClientAreRunning()
        {
            Assert.True(Processes.Robot.IsRunning);
            Assert.True(Processes.Client.IsRunning);
        }
    }
}
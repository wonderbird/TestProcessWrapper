using System.Linq;
using katarabbitmq.bdd.tests.Helpers;
using TechTalk.SpecFlow;
using Xunit;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class SharedSteps
    {
        [Given(@"the server and (.*) client are running")]
        [Given(@"the server and (.*) clients are running")]
        public static void GivenTheServerAndClientAreRunning(int numberOfClients)
        {
            for (var clientIndex = 0; clientIndex < numberOfClients - 1; clientIndex++)
            {
                var client = new RemoteControlledProcess("kata-rabbitmq.client.app");
                Processes.Clients.Add(client);
                client.Start();
            }

            Assert.Equal(numberOfClients - 1, Processes.Clients.Count);
            Assert.True(Processes.Clients.All(c => c.IsRunning));

            Assert.True(Processes.Robot.IsRunning);
            Assert.True(Processes.Client.IsRunning);
        }
    }
}
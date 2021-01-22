using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class NoUnhandledExceptionsStepDefinitions
    {
        [When]
        public static void WhenATERMSignalIsSentToAllApplications()
        {
            SharedStepDefinitions.ShutdownProcessesGracefully();
        }

        [Then]
        public static void ThenAllApplicationsShutDown()
        {
            Assert.True(SharedStepDefinitions.Robot.HasExited);
            Assert.True(SharedStepDefinitions.Clients.All(c => c.HasExited));
        }

        [Then]
        public static void ThenTheLogIsFreeOfExceptionMessages()
        {
            Assert.DoesNotContain("exception", SharedStepDefinitions.Robot.ReadOutput(), StringComparison.CurrentCultureIgnoreCase);
            foreach (var client in SharedStepDefinitions.Clients)
            {
                Assert.DoesNotContain("exception", client.ReadOutput(), StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}
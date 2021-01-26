using System;
using System.Collections.Generic;
using System.Linq;
using katarabbitmq.bdd.tests.Helpers;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class NoUnhandledExceptionsStepDefinitions : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private bool _isDisposed;

        public NoUnhandledExceptionsStepDefinitions(ITestOutputHelper testOutputHelper) =>
            _testOutputHelper = testOutputHelper;

        public static List<RemoteControlledProcess> Clients { get; } = new();
        public static RemoteControlledProcess Robot { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [When]
        public static void WhenATERMSignalIsSentToAllApplications()
        {
            ShutdownProcessesGracefully();
        }

        [Then]
        public static void ThenAllApplicationsShutDown()
        {
            Assert.True(Robot.HasExited);
            Assert.True(Clients.All(c => c.HasExited));
        }

        [Then]
        public static void ThenTheLogIsFreeOfExceptionMessages()
        {
            Assert.DoesNotContain("exception", Robot.ReadOutput(),
                StringComparison.CurrentCultureIgnoreCase);
            foreach (var client in Clients)
            {
                Assert.DoesNotContain("exception", client.ReadOutput(), StringComparison.CurrentCultureIgnoreCase);
            }
        }

        [Given(@"the robot and (.*) client are running")]
        [Given(@"the robot and (.*) clients are running")]
        public void GivenTheRobotAndClientsAreRunning(int numberOfClients)
        {
            for (var clientIndex = 0; clientIndex < numberOfClients; clientIndex++)
            {
                var client = new RemoteControlledProcess("kata-rabbitmq.client.app");
                client.TestOutputHelper = _testOutputHelper;
                client.Start();

                Clients.Add(client);
            }

            Assert.True(Clients.All(c => c.IsRunning));

            Robot = new RemoteControlledProcess("kata-rabbitmq.robot.app");
            Robot.TestOutputHelper = _testOutputHelper;
            Robot.Start();

            Assert.True(Robot.IsRunning);
        }

        public static void ShutdownProcessesGracefully()
        {
            Robot.ShutdownGracefully();

            foreach (var client in Clients)
            {
                client.ShutdownGracefully();
            }
        }

        [AfterScenario]
        public static void ForceProcessTermination()
        {
            Robot.ForceTermination();
            Robot.Dispose();
            Robot = null;

            foreach (var client in Clients)
            {
                client.ForceTermination();
                client.Dispose();
            }

            Clients.Clear();
        }

        ~NoUnhandledExceptionsStepDefinitions()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                Robot?.Dispose();
                foreach (var client in Clients)
                {
                    client?.Dispose();
                }
            }

            _isDisposed = true;
        }
    }
}
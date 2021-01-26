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
    public class SharedStepDefinitions : IDisposable
    {
        public static List<RemoteControlledProcess> Clients { get;  } = new();
        private readonly ITestOutputHelper _testOutputHelper;
        public static RemoteControlledProcess Robot { get; private set; }

        private bool _isDisposed;

        public SharedStepDefinitions(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

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
            SharedStepDefinitions.Robot.ShutdownGracefully();

            foreach (var client in SharedStepDefinitions.Clients)
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SharedStepDefinitions()
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
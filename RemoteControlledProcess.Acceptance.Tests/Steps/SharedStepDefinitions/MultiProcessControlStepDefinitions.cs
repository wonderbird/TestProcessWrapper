using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace RemoteControlledProcess.Acceptance.Tests.Steps.SharedStepDefinitions
{
    [Binding]
    public class MultiProcessControlStepDefinitions : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private bool _isDisposed;

        public MultiProcessControlStepDefinitions(ITestOutputHelper testOutputHelper) =>
            _testOutputHelper = testOutputHelper;

        public static List<TestProcessWrapper> Clients { get; } = new();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [When(@"all applications are shut down gracefully")]
        public static void WhenAllApplicationsAreShutDownGracefully()
        {
            ShutdownProcessesGracefully();
        }

        [Then]
        public static void ThenAllApplicationsShutDown()
        {
            Assert.True(Clients.All(c => c.HasExited));
        }

        [Given(@"(.*) application is running with coverlet '(.*)'")]
        [Given(@"(.*) applications are running with coverlet '(.*)'")]
        public void GivenApplicationsAreRunning(int numberOfClients, bool isCoverletEnabled)
        {
            for (var clientIndex = 0; clientIndex < numberOfClients; clientIndex++)
            {
                var client = new TestProcessWrapper(
                    "RemoteControlledProcess.Application",
                    isCoverletEnabled
                );
                client.TestOutputHelper = _testOutputHelper;
                client.Start();

                Clients.Add(client);
            }

            Assert.True(Clients.All(c => c.IsRunning));
        }

        private static void ShutdownProcessesGracefully()
        {
            foreach (var client in Clients)
            {
                client.ShutdownGracefully();
            }
        }

        [AfterScenario]
        public static void ForceProcessTermination()
        {
            foreach (var client in Clients)
            {
                client.ForceTermination();
                client.Dispose();
            }

            Clients.Clear();
        }

        ~MultiProcessControlStepDefinitions()
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
                foreach (var client in Clients)
                {
                    client?.Dispose();
                }
            }

            _isDisposed = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using RemoteControlledProcess;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class CorrectUsageStepDefinitions : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private bool _isDisposed;

        public CorrectUsageStepDefinitions(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        public static List<ProcessWrapper> Clients { get; } = new();

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
            Assert.True(Clients.All(c => c.HasExited));
        }

        [Then]
        public static void ThenTheLogIsFreeOfExceptionMessages()
        {
            foreach (var client in Clients)
            {
                Assert.DoesNotContain("Unhandled exception", client.ReadOutput(), StringComparison.CurrentCultureIgnoreCase);
            }
        }

        [Then]
        public static void ThenEachLogShowsAnExceptionMessage()
        {
            const string expectedExceptionMessage = "Unhandled exception.";

            foreach (var client in Clients)
            {
                var output = client.ReadOutput();
                Assert.Contains(expectedExceptionMessage, output, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        [Given(@"(.*) application is running with coverlet '(.*)'")]
        [Given(@"(.*) applications are running with coverlet '(.*)'")]
        public void GivenApplicationsAreRunning(int numberOfClients, bool isCoverletEnabled)
        {
            for (var clientIndex = 0; clientIndex < numberOfClients; clientIndex++)
            {
                var client = new ProcessWrapper("RemoteControlledProcess.Application", isCoverletEnabled);
                client.TestOutputHelper = _testOutputHelper;
                client.Start();

                Clients.Add(client);
            }

            Assert.True(Clients.All(c => c.IsRunning));
        }

        public static void ShutdownProcessesGracefully()
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

        ~CorrectUsageStepDefinitions()
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
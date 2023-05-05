using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace RemoteControlledProcess.Acceptance.Tests.Steps.SharedStepDefinitions
{
    [Binding]
    public sealed class MultiProcessControlStepDefinitions : IDisposable
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

        [When(@"all applications had enough time to finish")]
        public static void WhenAllApplicationsHadEnoughTimeToFinish()
        {
            foreach (var client in Clients)
            {
                client.WaitForProcessExit();
            }
        }

        [Then]
        public static void ThenAllApplicationsShutDown()
        {
            Assert.True(Clients.All(c => c.HasExited));
        }

        [Given(@"(.*) '(long|short)' lived application is running with coverlet '(enabled|disabled)'")]
        [Given(@"(.*) '(long|short)' lived applications are running with coverlet '(enabled|disabled)'")]
        public void GivenLivedApplicationsAreRunningWithCoverlet(int numberOfClients, bool isLongLived, bool isCoverletEnabled)
        {
            var appProjectName = isLongLived
                ? "RemoteControlledProcess.Application"
                : "RemoteControlledProcess.ShortLived.Application";

            CreateAndStartAllApplications(numberOfClients, isCoverletEnabled, appProjectName);

            if (isLongLived)
            {
                Assert.True(Clients.All(c => c.IsRunning));
            }
        }

        private void CreateAndStartAllApplications(int numberOfClients, bool isCoverletEnabled, string appProjectName)
        {
            for (var clientIndex = 0; clientIndex < numberOfClients; clientIndex++)
            {
                var client = new TestProcessWrapper(
                    appProjectName,
                    isCoverletEnabled
                );
                client.TestOutputHelper = _testOutputHelper;
                client.Start();

                Clients.Add(client);
            }
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

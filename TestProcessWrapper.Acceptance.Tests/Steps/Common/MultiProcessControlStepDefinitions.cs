using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace TestProcessWrapper.Acceptance.Tests.Steps.Common
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

        [Given(@"(.*) long lived application is running with coverlet '(enabled|disabled)'")]
        [Given(@"(.*) long lived applications are running with coverlet '(enabled|disabled)'")]
        public void GivenLongLivedApplicationsAreRunningWithCoverlet(
            int numberOfClients,
            bool isCoverletEnabled
        )
        {
            const string appProjectName = "TestProcessWrapper.LongLived.Application";

            CreateAndStartAllApplications(numberOfClients, isCoverletEnabled, appProjectName);

            Assert.True(Clients.All(c => c.IsRunning));
        }

        [Given(@"(.*) short lived application is running with coverlet '(enabled|disabled)'")]
        [Given(@"(.*) short lived applications are running with coverlet '(enabled|disabled)'")]
        public void GivenShortLivedApplicationsAreRunningWithCoverlet(
            int numberOfClients,
            bool isCoverletEnabled
        )
        {
            const string appProjectName = "TestProcessWrapper.ShortLived.Application";

            CreateAndStartAllApplications(numberOfClients, isCoverletEnabled, appProjectName);
        }

        [When(@"all applications had enough time to finish")]
        public static void WhenAllApplicationsHadEnoughTimeToFinish()
        {
            foreach (var client in Clients)
            {
                client.WaitForProcessExit();
            }
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

        private void CreateAndStartAllApplications(
            int numberOfClients,
            bool isCoverletEnabled,
            string appProjectName
        )
        {
            for (var clientIndex = 0; clientIndex < numberOfClients; clientIndex++)
            {
                var client = new TestProcessWrapper(
                    appProjectName,
                    isCoverletEnabled,
                    BuildConfiguration.Debug
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

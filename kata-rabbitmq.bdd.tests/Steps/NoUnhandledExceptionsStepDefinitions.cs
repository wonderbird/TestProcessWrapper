using System;
using System.Threading.Tasks;
using katarabbitmq.bdd.tests.Helpers;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class NoUnhandledExceptionsStepDefinitions : IDisposable
    {
        private RemoteControlledProcess _client;
        private RemoteControlledProcess _robot;
        private readonly ITestOutputHelper _testOutputHelper;
        private bool _isDisposed;

        public NoUnhandledExceptionsStepDefinitions(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Given]
        public void GivenTheServerAndClientAreRunning()
        {
            _robot = new("kata-rabbitmq.robot.app");
            _robot.TestOutputHelper = _testOutputHelper;
            _robot.Start();

            _client = new("kata-rabbitmq.client.app");
            _client.TestOutputHelper = _testOutputHelper;
            _client.Start();
        }

        [When]
        public void WhenATermSignalIsSentToBothServerAndClient()
        {
            _robot.ShutdownGracefully();
            _client.ShutdownGracefully();
        }

        [Then]
        public void ThenBothApplicationsShutDown()
        {
            Assert.True(_robot.HasExited);
            Assert.True(_client.HasExited);
        }

        [Then]
        public void ThenTheLogIsFreeOfExceptionMessages()
        {
            Task.Delay(TimeSpan.FromMilliseconds(500));

            Assert.DoesNotContain("exception", _robot.ReadOutput(), StringComparison.CurrentCultureIgnoreCase);
            Assert.DoesNotContain("exception", _client.ReadOutput(), StringComparison.CurrentCultureIgnoreCase);
        }

        [AfterScenario("NoUnhandledExceptions")]
        public void StopProcesses()
        {
            _robot?.ForceTermination();
            _client?.ForceTermination();
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
                _robot?.Dispose();
                _client?.Dispose();
            }

            _isDisposed = true;
        }
    }
}
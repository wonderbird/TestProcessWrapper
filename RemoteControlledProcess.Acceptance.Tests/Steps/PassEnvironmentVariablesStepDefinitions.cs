using System;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace RemoteControlledProcess.Acceptance.Tests.Steps
{
    [Binding]
    public class PassEnvironmentVariablesStepDefinitions : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private TestProcessWrapper _client;
        private bool _isDisposed;
        private readonly ScenarioContext _scenarioContext;

        public PassEnvironmentVariablesStepDefinitions(ScenarioContext scenarioContext,
            ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _scenarioContext = scenarioContext;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PassEnvironmentVariablesStepDefinitions()
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
                _client?.Dispose();
            }

            _isDisposed = true;
        }

        [Given(@"environment variables have been configured")]
        public void GivenEnvironmentVariablesHaveBeenConfigured()
        {
            _scenarioContext.Pending();
        }

        [Given(@"the application has been started")]
        public void GivenTheApplicationHasBeenStarted()
        {
            _client = new TestProcessWrapper("RemoteControlledProcess.Application", false);
            _client.TestOutputHelper = _testOutputHelper;
            _client.Start();
        }

        [When]
        public void WhenTheApplicationIsReady()
        {
            _client.ShutdownGracefully();
            _client.ForceTermination();
            _client.Dispose();
        }

        [Then(@"the application has received the configured environment variables")]
        public void ThenTheApplicationHasReceivedTheConfiguredEnvironmentVariables()
        {
            _scenarioContext.Pending();
        }
    }
}
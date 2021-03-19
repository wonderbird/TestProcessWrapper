using System;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace RemoteControlledProcess.Acceptance.Tests.Steps
{
    [Binding]
    public class CustomReadinessCheckStepDefinitions : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private ProcessWrapper _client;
        private bool _isDisposed;
        private bool _isCustomCheckExecuted;

        public CustomReadinessCheckStepDefinitions(ScenarioContext scenarioContext, ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        ~CustomReadinessCheckStepDefinitions()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        [Given]
        public void GivenAnApplicationWasStartedWithACustomReadinessCheck()
        {
            _client = new ProcessWrapper("RemoteControlledProcess.Application", false);
            _client.TestOutputHelper = _testOutputHelper;
            _client.ReadinessChecks.Add(() =>
            {
                _isCustomCheckExecuted = true;
                return true;
            });
            _client.Start();
        }

        [When]
        public void WhenTheApplicationIsReady()
        {
            _client.ShutdownGracefully();
            _client.ForceTermination();
            _client.Dispose();
        }

        [Then]
        public void ThenTheCustomReadinessCheckWasExecutedSuccessfully()
        {
            Assert.True(_isCustomCheckExecuted, "Custom readiness check has not been executed.");
        }
    }}
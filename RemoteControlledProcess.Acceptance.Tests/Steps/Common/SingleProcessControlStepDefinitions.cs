using System;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace RemoteControlledProcess.Acceptance.Tests.Steps.Common
{
    [Binding]
    public sealed class SingleProcessControlStepDefinitions : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private bool _isDisposed;

        public SingleProcessControlStepDefinitions(
            ScenarioContext scenarioContext,
            ITestOutputHelper testOutputHelper
        ) => _testOutputHelper = testOutputHelper;

        public static TestProcessWrapper Client { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SingleProcessControlStepDefinitions()
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
                Client?.Dispose();
            }

            _isDisposed = true;
        }

        [Given(@"An application was wrapped into TestProcessWrapper")]
        public void GivenAnApplicationWasWrappedIntoTestProcessWrapper()
        {
            Client = new TestProcessWrapper("RemoteControlledProcess.LongLived.Application", false);
            Client.TestOutputHelper = _testOutputHelper;
        }

        [When]
        public static void WhenTheApplicationIsReady()
        {
            Client.Start();
        }

        [AfterScenario]
        public static void ShutdownClient()
        {
            if (Client == null)
            {
                return;
            }

            Client.ShutdownGracefully();
            Client.ForceTermination();
            Client.Dispose();
            Client = null; // Prevent Client from being shutdown twice when running multiple tests, because Client is static.
        }
    }
}

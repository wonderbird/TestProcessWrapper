using System;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace TestProcessWrapper.Acceptance.Tests.Steps.Common
{
    [Binding]
    public sealed class SingleProcessControlStepDefinitions : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private bool _isDisposed;
        private const BuildConfiguration DefaultBuildConfiguration =
#if DEBUG
        BuildConfiguration.Debug;
#else
        BuildConfiguration.Release;
#endif

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

        [Given(@"A long lived application was wrapped into TestProcessWrapper")]
        public void GivenALongLivedApplicationWasWrappedIntoTestProcessWrapper()
        {
            Client = new TestProcessWrapper(
                "TestProcessWrapper.LongLived.Application",
                false,
                DefaultBuildConfiguration
            );
            Client.TestOutputHelper = _testOutputHelper;
        }

        [Given(@"A short lived application was wrapped into TestProcessWrapper")]
        public void GivenAShortLivedApplicationWasWrappedIntoTestProcessWrapper()
        {
            Client = new TestProcessWrapper(
                "TestProcessWrapper.ShortLived.Application",
                false,
                DefaultBuildConfiguration
            );
            Client.TestOutputHelper = _testOutputHelper;
        }

        [Given(@"coverlet has been enabled")]
        public static void GivenCoverletHasBeenEnabled()
        {
            Client.EnableCoverlet();
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using RemoteControlledProcess;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class CorrectUsageStepDefinitions : IDisposable
    {
        private static readonly Regex LineCoverageRegex =
            new(@"\|\sTotal\s*\|\s*([0-9\.]*)\%\s*\|\s*[0-9\.]*%\s*\|\s*[0-9\.]*%\s*\|", RegexOptions.Multiline);

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

        [Then(@"the reported total line coverage (is greater|equals) (.*)%")]
        public void ThenTheReportedTotalLineCoverageIsGreater(string comparisonString,
            int expectedLineCoveragePercent)
        {
            var comparison = GetComparisonByName(comparisonString);

            var clientsWithCoverlet = Clients.Where(c => c.IsCoverletEnabled);
            foreach (var client in clientsWithCoverlet)
            {
                var actualLineCoveragePercent = GetLineCoverageFromCoverletOutput(client.ReadOutput());
                Assert.True(comparison(actualLineCoveragePercent, expectedLineCoveragePercent),
                    $"Mismatch: line coverage of {actualLineCoveragePercent}% {comparisonString} {expectedLineCoveragePercent}%");
            }
        }

        private static Func<double, double, bool> GetComparisonByName(string comparisonString)
        {
            Dictionary<string, Func<double, double, bool>> comparisonMap = new()
            {
                { "is greater", (actual, expected) => actual > expected },
                { "equals", (actual, expected) => Math.Abs(actual - expected) < 0.001 }
            };
            var comparison = comparisonMap[comparisonString];
            return comparison;
        }

        private double GetLineCoverageFromCoverletOutput(string coverletOutput)
        {
            var lineCoverageMatch = LineCoverageRegex.Match(coverletOutput);
            var lineCoveragePercentString = lineCoverageMatch.Groups[1].Value;

            _testOutputHelper?.WriteLine($"Extracted linecoverage string: \"{lineCoveragePercentString}\"");

            var lineCoveragePercent = double.Parse(lineCoveragePercentString, CultureInfo.InvariantCulture);
            return lineCoveragePercent;
        }

        [Then]
        public static void ThenTheLogIsFreeOfExceptionMessages()
        {
            foreach (var client in Clients)
            {
                Assert.DoesNotContain("exception", client.ReadOutput(), StringComparison.CurrentCultureIgnoreCase);
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
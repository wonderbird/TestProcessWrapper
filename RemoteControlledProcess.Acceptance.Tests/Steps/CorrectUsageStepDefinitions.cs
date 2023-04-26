using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using RemoteControlledProcess.Acceptance.Tests.Steps.SharedStepDefinitions;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace RemoteControlledProcess.Acceptance.Tests.Steps
{
    [Binding]
    public class CorrectUsageStepDefinitions
    {
        // TODO: Fix Warning: [SYSLIB1045] Use 'GeneratedRegexAttribute' to generate the regular expression implementation at compile-time.
        #pragma warning disable SYSLIB1045
        private static readonly Regex LineCoverageRegex =
            new(@"\|\sTotal\s*\|\s*([0-9\.]*)\%\s*\|\s*[0-9\.]*%\s*\|\s*[0-9\.]*%\s*\|", RegexOptions.Multiline);
        #pragma warning restore SYSLIB1045

        private readonly ITestOutputHelper _testOutputHelper;

        public CorrectUsageStepDefinitions(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Then(@"the reported total line coverage (is greater|equals) (.*)%")]
        public void ThenTheReportedTotalLineCoverageIsGreater(string comparisonString,
            int expectedLineCoveragePercent)
        {
            var comparison = GetComparisonByName(comparisonString);

            var clientsWithCoverlet = MultiProcessControlStepDefinitions.Clients.Where(c => c.IsCoverletEnabled);
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
            foreach (var client in MultiProcessControlStepDefinitions.Clients)
            {
                Assert.DoesNotContain("exception", client.ReadOutput(), StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}
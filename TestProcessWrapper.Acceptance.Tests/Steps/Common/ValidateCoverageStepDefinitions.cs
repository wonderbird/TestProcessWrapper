using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace TestProcessWrapper.Acceptance.Tests.Steps.Common;

[Binding]
public partial class ValidateCoverageStepDefinitions
{
    // If your IDE reports an CS8795 error "Partial method 'Regex LineCoverageRegex()' must have an implementation
    // part because it has accessibility modifiers.", then restart your IDE.
    // Most probably you are facing a caching problem and the compile is actually working.
    // See: https://www.reddit.com/r/csharp/comments/yrkl90/generatedregex_in_static_class/
    [GeneratedRegex(
        @"\|\sTotal\s*\|\s*([0-9\.]*)\%\s*\|\s*[0-9\.]*%\s*\|\s*[0-9\.]*%\s*\|",
        RegexOptions.Multiline
    )]
    private static partial Regex LineCoverageRegex();

    private readonly ITestOutputHelper _testOutputHelper;

    public ValidateCoverageStepDefinitions(ITestOutputHelper testOutputHelper) =>
        _testOutputHelper = testOutputHelper;

    [Then(@"the reported total line coverage (is greater|equals) (.*)%")]
    public void ThenTheReportedTotalLineCoverageIsGreaterEquals(
        string comparisonString,
        int expectedLineCoveragePercent
    )
    {
        var comparison = GetComparisonByName(comparisonString);

        var clientsWithCoverlet = MultiProcessControlStepDefinitions.Clients.Where(c =>
            c.IsCoverletEnabled
        );
        foreach (var client in clientsWithCoverlet)
        {
            var actualLineCoveragePercent = GetLineCoverageFromCoverletOutput(
                client.RecordedOutput
            );
            Assert.True(
                comparison(actualLineCoveragePercent, expectedLineCoveragePercent),
                $"Mismatch: line coverage of {actualLineCoveragePercent}% {comparisonString} {expectedLineCoveragePercent}%"
            );
        }
    }

    private static Func<double, double, bool> GetComparisonByName(string comparisonString)
    {
        Dictionary<string, Func<double, double, bool>> comparisonMap =
            new()
            {
                { "is greater", (actual, expected) => actual > expected },
                { "equals", (actual, expected) => Math.Abs(actual - expected) < 0.001 }
            };
        var comparison = comparisonMap[comparisonString];
        return comparison;
    }

    private double GetLineCoverageFromCoverletOutput(string coverletOutput)
    {
        var lineCoverageMatch = LineCoverageRegex().Match(coverletOutput);
        var lineCoveragePercentString = lineCoverageMatch.Groups[1].Value;

        _testOutputHelper?.WriteLine(
            $"Extracted linecoverage string: \"{lineCoveragePercentString}\""
        );

        var lineCoveragePercent = double.Parse(
            lineCoveragePercentString,
            CultureInfo.InvariantCulture
        );
        return lineCoveragePercent;
    }
}

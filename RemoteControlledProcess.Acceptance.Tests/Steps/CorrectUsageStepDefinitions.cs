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
    public partial class CorrectUsageStepDefinitions
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CorrectUsageStepDefinitions(ITestOutputHelper testOutputHelper) =>
            _testOutputHelper = testOutputHelper;

        [Then]
        public static void ThenTheLogIsFreeOfExceptionMessages()
        {
            foreach (var client in MultiProcessControlStepDefinitions.Clients)
            {
                Assert.DoesNotContain("exception", client.RecordedOutput, StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}

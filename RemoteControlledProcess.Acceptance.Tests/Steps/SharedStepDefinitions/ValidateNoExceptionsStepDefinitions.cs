using System;
using TechTalk.SpecFlow;
using Xunit;

namespace RemoteControlledProcess.Acceptance.Tests.Steps.SharedStepDefinitions;

[Binding]
public static class ValidateNoExceptionsStepDefinitions
{
    [Then]
    public static void ThenTheLogIsFreeOfExceptionMessages()
    {
        foreach (var client in MultiProcessControlStepDefinitions.Clients)
        {
            Assert.DoesNotContain("exception", client.RecordedOutput, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
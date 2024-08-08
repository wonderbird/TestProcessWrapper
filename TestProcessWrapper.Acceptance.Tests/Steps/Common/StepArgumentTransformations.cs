using System;
using TechTalk.SpecFlow;

namespace TestProcessWrapper.Acceptance.Tests.Steps.Common
{
    [Binding]
    public static class StepArgumentTransformations
    {
        [StepArgumentTransformation(@"(enabled|disabled)")]
        public static bool TransformHumanReadableBooleanSwitchExpression(string expression) =>
            expression.Equals("enabled", StringComparison.OrdinalIgnoreCase);

        [StepArgumentTransformation(@"(Debug|Release)")]
        public static BuildConfiguration TransformHumanReadableBuildConfiguration(
            string buildConfiguration
        ) =>
            buildConfiguration.Equals("Release", StringComparison.OrdinalIgnoreCase)
                ? BuildConfiguration.Release
                : BuildConfiguration.Debug;
    }
}

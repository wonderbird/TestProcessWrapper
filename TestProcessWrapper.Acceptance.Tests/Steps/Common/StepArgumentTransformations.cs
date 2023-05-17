using System.Globalization;
using TechTalk.SpecFlow;

namespace TestProcessWrapper.Acceptance.Tests.Steps.Common
{
    [Binding]
    public static class StepArgumentTransformations
    {
        [StepArgumentTransformation(@"(enabled|disabled)")]
        public static bool TransformHumanReadableBooleanSwitchExpression(string expression) =>
            "ENABLED" == expression.ToUpper(CultureInfo.CurrentCulture);

        [StepArgumentTransformation(@"(Debug|Release)")]
        public static BuildConfiguration TransformHumanReadableBuildConfiguration(
            string buildConfiguration
        ) =>
            "RELEASE" == buildConfiguration.ToUpperInvariant()
                ? BuildConfiguration.Release
                : BuildConfiguration.Debug;
    }
}

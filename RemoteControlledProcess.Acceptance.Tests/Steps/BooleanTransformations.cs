using System.Globalization;
using TechTalk.SpecFlow;

namespace RemoteControlledProcess.Acceptance.Tests.Steps
{
    [Binding]
    public static class BooleanTransformations
    {
        [StepArgumentTransformation(@"(enabled|disabled)")]
        public static bool TransformHumanReadableBooleanSwitchExpression(string expression) =>
            "ENABLED" == expression.ToUpper(CultureInfo.CurrentCulture);

        [StepArgumentTransformation(@"(long|short)")]
        public static bool TransformIsLongLivedBooleanSwitchExpression(string expression) =>
            "LONG" == expression.ToUpper(CultureInfo.CurrentCulture);
    }
}

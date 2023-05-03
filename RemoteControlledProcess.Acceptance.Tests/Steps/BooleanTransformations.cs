using System.Globalization;
using TechTalk.SpecFlow;

namespace RemoteControlledProcess.Acceptance.Tests.Steps
{
    [Binding]
    public static class BooleanTransformations
    {
        [StepArgumentTransformation]
        public static bool TransformHumanReadableBooleanSwitchExpression(string expression) =>
            "ENABLED" == expression.ToUpper(CultureInfo.CurrentCulture);

        [StepArgumentTransformation]
        public static bool TransformIsLongLivedBooleanSwitchExpression(string expression) =>
            "LONG" == expression.ToUpper(CultureInfo.CurrentCulture);
    }
}

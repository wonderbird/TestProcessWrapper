using System.Globalization;
using TechTalk.SpecFlow;

namespace RemoteControlledProcess.Acceptance.Tests.Steps.Common
{
    [Binding]
    public static class BooleanTransformations
    {
        [StepArgumentTransformation(@"(enabled|disabled)")]
        public static bool TransformHumanReadableBooleanSwitchExpression(string expression) =>
            "ENABLED" == expression.ToUpper(CultureInfo.CurrentCulture);
    }
}

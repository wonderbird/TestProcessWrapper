using System.Globalization;
using TechTalk.SpecFlow;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class BooleanTransformations
    {
        [StepArgumentTransformation]
        public static bool TransformHumanReadableBooleanSwitchExpression(string expression) =>
            "ENABLED" == expression.ToUpper(CultureInfo.CurrentCulture);
    }
}
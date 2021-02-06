using TechTalk.SpecFlow;

namespace katarabbitmq.bdd.tests.Steps
{
    public class SpecifyReportDirectoryStepDefinition
    {
        private readonly ScenarioContext _scenarioContext;

        public SpecifyReportDirectoryStepDefinition(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        [Given(@"the number of coverage reports in the TestResults folder is known")]
        public void GivenTheNumberOfCoverageReportsInTheTestResultsFolderIsKnown()
        {
            _scenarioContext.Pending();
        }

        [Then(@"an additional coverage report exists in the TestResults folder")]
        public void ThenAnAdditionalCoverageReportExistsInTheTestResultsFolder()
        {
            _scenarioContext.Pending();
        }
    }
}
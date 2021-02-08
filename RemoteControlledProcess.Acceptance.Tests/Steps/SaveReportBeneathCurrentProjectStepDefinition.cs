using System.IO;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class SpecifyReportDirectoryStepDefinition
    {
        private int _initialNumberOfCoverageReports;

        [Given(@"the number of coverage reports in the TestResults folder is known")]
        public void GivenTheNumberOfCoverageReportsInTheTestResultsFolderIsKnown()
        {
            _initialNumberOfCoverageReports = CountTestResultFiles();
        }

        private static int CountTestResultFiles()
        {
            try
            {
                var directory = Path.Join("..", "..", "..", "TestResults");
                var searchPattern = "RemoteControlledProcess.Application.*.xml";

                return Directory.EnumerateFiles(directory, searchPattern, SearchOption.TopDirectoryOnly)
                    .Count();
            }
            catch (DirectoryNotFoundException)
            {
                return 0;
            }
        }

        [Then(@"an additional coverage report exists in the TestResults folder")]
        public void ThenAnAdditionalCoverageReportExistsInTheTestResultsFolder()
        {
            var actualNumberOfCoverageReports = CountTestResultFiles();
            Assert.Equal(_initialNumberOfCoverageReports + 1, actualNumberOfCoverageReports);
        }
    }
}
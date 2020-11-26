using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class SetupAndTearDownClientProcess
    {
        public SetupAndTearDownClientProcess(ITestOutputHelper testOutputHelper)
        {
            ClientProcess.TestOutputHelper = testOutputHelper;
        }

        [BeforeScenario]
        public void StartClientProcess()
        {
            ClientProcess.Start();
        }

        [AfterScenario]
        public void StopClientProcess()
        {
            ClientProcess.SendTermSignal();
            ClientProcess.Kill();
        }
    }
}
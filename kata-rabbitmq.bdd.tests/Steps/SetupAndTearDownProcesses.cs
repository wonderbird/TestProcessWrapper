using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class SetupAndTearDownRobotProcess
    {
        public SetupAndTearDownRobotProcess(ITestOutputHelper testOutputHelper)
        {
            Processes.Robot.TestOutputHelper = testOutputHelper;
            Processes.Client.TestOutputHelper = testOutputHelper;
        }

        [BeforeScenario]
        public void StartProcesses()
        {
            Processes.Robot.Start();
            Processes.Client.Start();
        }

        [AfterScenario]
        public void StopProcesses()
        {
            Processes.Robot.SendTermSignal();
            Processes.Robot.Kill();

            Processes.Client.SendTermSignal();
            Processes.Client.Kill();
        }
    }
}

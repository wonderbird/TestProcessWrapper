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
        }

        [BeforeScenario]
        public void StartRobotProcess()
        {
            Processes.Robot.Start();
        }

        [AfterScenario]
        public void StopRobotProcess()
        {
            Processes.Robot.SendTermSignal();
            Processes.Robot.Kill();
        }
    }
}
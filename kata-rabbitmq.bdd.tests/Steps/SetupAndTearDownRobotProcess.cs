using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class SetupAndTearDownRobotProcess
    {
        public SetupAndTearDownRobotProcess(ITestOutputHelper testOutputHelper)
        {
            RobotProcess.Process.TestOutputHelper = testOutputHelper;
        }

        [BeforeScenario]
        public void StartRobotProcess()
        {
            RobotProcess.Process.Start();
        }

        [AfterScenario]
        public void StopRobotProcess()
        {
            RobotProcess.Process.SendTermSignal();
            RobotProcess.Process.Kill();
        }
    }
}
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class SetupAndTearDownRobotProcess
    {
        public SetupAndTearDownRobotProcess(ITestOutputHelper testOutputHelper)
        {
            RobotProcess.TestOutputHelper = testOutputHelper;
        }

        [BeforeScenario]
        public void StartRobotProcess()
        {
            RobotProcess.Start();
        }

        [AfterScenario]
        public void StopRobotProcess()
        {
            RobotProcess.SendTermSignal();
            RobotProcess.Kill();
        }
    }
}
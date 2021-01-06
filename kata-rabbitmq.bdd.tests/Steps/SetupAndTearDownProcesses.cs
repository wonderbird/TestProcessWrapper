using katarabbitmq.bdd.tests.Helpers;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Steps
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
        public static void StartProcesses()
        {
            Processes.Robot.Start();
            Processes.Client.Start();
        }

        [AfterScenario]
        public static void StopProcesses()
        {
            Processes.Robot.ShutdownGracefully();
            Processes.Robot.ForceTermination();

            Processes.Client.ShutdownGracefully();
            Processes.Client.ForceTermination();
        }
    }
}
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace katarabbitmq.bdd.tests.Steps
{
    [Binding]
    public class LightSensorReadingsStepDefinitions
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private int _countReceivedSensorReadings;
        private bool _isSensorQueuePresent;

        public LightSensorReadingsStepDefinitions(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Given("the robot app is started")]
        public static void GivenTheRobotAppIsStarted()
        {
            Assert.True(Processes.Robot.IsRunning);
        }
        
        [Given("the client app is started")]
        public static void GivenTheClientAppIsStarted()
        {
            Assert.True(Processes.Client.IsRunning);
        }

        [When("the robot and client app have been connected for (.*) seconds")]
        public async Task WhenTheRobotAndClientAppHasBeenConnectedForSeconds(double seconds)
        {
            await WaitUntilProcessConnectedToRabbitMq(Processes.Robot);
            await WaitUntilProcessConnectedToRabbitMq(Processes.Client);

            await WaitForSeconds(seconds);

            ParseSensorDataFromClientProcess();
        }

        private static async Task WaitUntilProcessConnectedToRabbitMq(RemoteControlledProcess remoteControlledProcess)
        {
            while (!remoteControlledProcess.IsConnectionEstablished)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(1.0));
            }
        }

        private async Task WaitForSeconds(double seconds)
        {
            var stopwatch = Stopwatch.StartNew();
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            stopwatch.Stop();
            _testOutputHelper?.WriteLine($"Waited for {stopwatch.ElapsedMilliseconds / 1000.0} seconds");
        }

        private void ParseSensorDataFromClientProcess()
        {
            var output = Processes.Client.ReadOutput();
            var lines = output.Split('\n').ToList();
            _countReceivedSensorReadings = lines.Count(l => l.Contains("Sensor data"));
        }

        [Then("the client app received at least (.*) sensor values")]
        public void ThenTheClientAppReceivedAtLeastSensorValues(int expectedSensorValuesCount)
        {
            _testOutputHelper.WriteLine($"Received {_countReceivedSensorReadings} values");
            Assert.True(_countReceivedSensorReadings >= expectedSensorValuesCount,
                $"Client app must receive at least {expectedSensorValuesCount} sensor value(s). It actually received {_countReceivedSensorReadings} values");
        }
    }
}

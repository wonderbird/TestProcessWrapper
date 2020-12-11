using System;
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

        [When("the sensor queue is checked")]
        public void WhenTheSensorQueueIsChecked()
        {
            try
            {
                _testOutputHelper.WriteLine("Testing whether robot:sensors exists ...");
                RabbitMq.Channel.ExchangeDeclarePassive("robot");
                RabbitMq.Channel.QueueDeclarePassive("sensors");

                _testOutputHelper.WriteLine("robot:sensors exists");
                _isSensorQueuePresent = true;
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine($"robot:sensors does not exist. Exception: {e.Message}");
                _isSensorQueuePresent = false;
            }
        }

        [Then("the sensor queue exists")]
        public void ThenTheSensorsQueueExists()
        {
            Assert.True(_isSensorQueuePresent);
        }

        [Given("the client app is started")]
        public static void GivenTheClientAppIsStarted()
        {
            Assert.True(Processes.Client.IsRunning);
        }

        [When("the robot and client app have been connected for 1 second")]
        public async Task WhenTheRobotAndClientAppHasBeenConnectedFor1Second()
        {
            await WaitUntilProcessConnectedToRabbitMq(Processes.Robot);
            await WaitUntilProcessConnectedToRabbitMq(Processes.Client);

            await Task.Delay(TimeSpan.FromSeconds(1.0));

            ParseSensorDataFromClientProcess();
        }

        private static async Task WaitUntilProcessConnectedToRabbitMq(RemoteControlledProcess remoteControlledProcess)
        {
            while (!remoteControlledProcess.IsConnectionEstablished)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(1.0));
            }
        }

        private void ParseSensorDataFromClientProcess()
        {
            var output = Processes.Client.ReadOutput();
            var lines = output.Split('\n').ToList();
            _countReceivedSensorReadings = lines.Count(l => l.Contains("Sensor data"));
        }

        [Then("the client app received at least 10 sensor values")]
        public void ThenTheClientAppReceivedAtLeast10SensorValues()
        {
            Assert.True(_countReceivedSensorReadings >= 10,
                $"Client app must receive at least 10 sensor values. It actually received {_countReceivedSensorReadings} values");
        }
    }
}

using System;
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
        private bool _isSensorQueuePresent;
        private int _countReceivedSensorReadings;

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

        [When("the client app has run for 1 second")]
        public void WhenTheClientAppHasRunFor1Second()
        {
            Task.Delay(TimeSpan.FromSeconds(1.0));
            
            while (Processes.Client.StandardOutput.Peek() != 0)
            {
                var currentLine = Processes.Client.StandardOutput.ReadLine();
                if (currentLine != null && currentLine.Contains("Sensor reading"))
                {
                    ++_countReceivedSensorReadings;
                }
            }
        }

        [Then("the client app received at least 10 sensor values")]
        public void ThenTheClientAppReceivedAtLeast10SensorValues()
        {
            Assert.True(_countReceivedSensorReadings >= 10);
        }
    }
}

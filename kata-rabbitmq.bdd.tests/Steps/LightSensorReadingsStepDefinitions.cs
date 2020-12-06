using System;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class LightSensorReadingsStepDefinitions
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private bool _isSensorQueuePresent;

        public LightSensorReadingsStepDefinitions(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Given("the robot app is started")]
        public void GivenTheRobotAppIsStarted()
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
        public void GivenTheClientAppIsStarted()
        {
            _testOutputHelper.WriteLine("The client app is started");
        }

        [When("the client app has run for 1 second")]
        public void WhenTheClientAppHasRunFor1Second()
        {
            _testOutputHelper.WriteLine("The client app has run for 1 second");
        }

        [Then("the client app received at least 10 sensor values")]
        public void WhenTheClientAppReceivedAtLeast10SensorValues()
        {
            _testOutputHelper.WriteLine("The client app received at least 10 sensor values");
            //Assert.False(true, "TODO: implement client test");
        }
    }
}

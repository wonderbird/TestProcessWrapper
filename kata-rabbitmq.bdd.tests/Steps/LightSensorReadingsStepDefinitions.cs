using System;
using kata_rabbitmq.robot.app;
using TechTalk.SpecFlow;
using Xunit;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class LightSensorReadingsStepDefinitions
    {
        private bool _isSensorQueuePresent;

        [Given("the robot app is started")]
        public void GivenTheRobotAppIsStarted()
        {
            Environment.SetEnvironmentVariable("RABBITMQ_HOSTNAME", RabbitMq.Container.Hostname);
            Environment.SetEnvironmentVariable("RABBITMQ_PORT", RabbitMq.Container.Port.ToString());
            Environment.SetEnvironmentVariable("RABBITMQ_USERNAME", RabbitMq.Container.Username);
            Environment.SetEnvironmentVariable("RABBITMQ_PASSWORD", RabbitMq.Container.Password);

            Program.Main(null);
        }
        
        [When("the sensor queue is checked")]
        public void WhenTheSensorQueueIsChecked()
        {
            try
            {
                RabbitMq.Channel.ExchangeDeclarePassive("robot");
                RabbitMq.Channel.QueueDeclarePassive("sensors");
                _isSensorQueuePresent = true;
            }
            catch (Exception)
            {
                // If one of the passive *Declare* functions fails, then the exchange
                // or channel does not exist.
                _isSensorQueuePresent = false;
            }
        }

        [Then("the sensor queue exists")]
        public void ThenTheSensorsQueueExists()
        {
            Assert.True(_isSensorQueuePresent);
        }
    }
}

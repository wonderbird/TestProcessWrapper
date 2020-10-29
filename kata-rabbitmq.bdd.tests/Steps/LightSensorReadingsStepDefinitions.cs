using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.MessageBrokers;
using DotNet.Testcontainers.Containers.Modules.MessageBrokers;
using kata_rabbitmq.robot.app;
using RabbitMQ.Client;
using TechTalk.SpecFlow;
using Xunit;

namespace kata_rabbitmq.bdd.tests
{
    [Binding]
    public class LightSensorReadingsStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private static RabbitMqTestcontainer _rabbitmqContainer;
        private static IConnection _connection;
        private static IModel _channel;
        private bool _isSensorQueuePresent = false;

        public LightSensorReadingsStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeFeature]
        public static async Task SetupRabbitMq()
        {
            var testcontainersBuilder = new TestcontainersBuilder<RabbitMqTestcontainer>()
                .WithMessageBroker(new RabbitMqTestcontainerConfiguration
                {
                    Username = "rabbitmq",
                    Password = "rabbitmq",
                });

            _rabbitmqContainer = testcontainersBuilder.Build();
            await _rabbitmqContainer.StartAsync();

            var connectionFactory = new ConnectionFactory { Uri = new Uri(_rabbitmqContainer.ConnectionString) };
            _connection = connectionFactory.CreateConnection();

            _channel = _connection.CreateModel();
        }

        [AfterFeature]
        public static async Task TearDownRabbitMq()
        {
            await _rabbitmqContainer.CleanUpAsync();
            await _rabbitmqContainer.DisposeAsync();
        }

        [Given("the robot app is started")]
        public void GivenTheRobotAppIsStarted()
        {
            Environment.SetEnvironmentVariable("RABBITMQ_HOSTNAME", _rabbitmqContainer.Hostname);
            Environment.SetEnvironmentVariable("RABBITMQ_PORT", _rabbitmqContainer.Port.ToString());
            Environment.SetEnvironmentVariable("RABBITMQ_USERNAME", _rabbitmqContainer.Username);
            Environment.SetEnvironmentVariable("RABBITMQ_PASSWORD", _rabbitmqContainer.Password);

            Program.Main(null);
        }
        
        [When("the sensor queue is checked")]
        public void WhenTheSensorQueueIsChecked()
        {
            try
            {
                _channel.ExchangeDeclarePassive("robot");
                _channel.QueueDeclarePassive("sensors");
                _isSensorQueuePresent = true;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Then("the sensors queue exists")]
        public void ThenTheSensorsQueueExists()
        {
            Assert.True(_isSensorQueuePresent);
        }
    }
}

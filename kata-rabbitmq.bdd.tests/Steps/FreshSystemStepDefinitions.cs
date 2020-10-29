using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.MessageBrokers;
using DotNet.Testcontainers.Containers.Modules.MessageBrokers;
using RabbitMQ.Client;
using TechTalk.SpecFlow;
using Xunit;

namespace kata_rabbitmq.bdd.tests
{
    [Binding]
    public class FreshSystemStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private static RabbitMqTestcontainer _rabbitmqContainer;
        private static IConnection _connection;
        private static IModel _channel;
        private bool _isRobotExchangePresent = false;

        public FreshSystemStepDefinitions(ScenarioContext scenarioContext)
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

        [Given("a fresh system is installed")]
        public void GivenAFreshSystemIsInstalled()
        {
        }
        
        [Then("the RabbitMQ channel is open")]
        public void TheRabbitMqChannelIsOpen()
        {
            Assert.True(_channel.IsOpen);
        }
        
        [When("robot exchanges are queried")]
        public void WhenRobotExchangesAreQueried()
        {
            try
            {
                _channel.ExchangeDeclarePassive("robot");
                _isRobotExchangePresent = true;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Then("the robot exchanges do not exist")]
        public void TheRobotExchangesDoNotExist()
        {
            Assert.False(_isRobotExchangePresent);
        }
    }
}

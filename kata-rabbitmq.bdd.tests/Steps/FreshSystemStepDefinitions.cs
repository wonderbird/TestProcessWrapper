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

        FreshSystemStepDefinitions(ScenarioContext scenarioContext)
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

        [When("exchanges are queried")]
        public void WhenExchangesAreQueried()
        {
        }

        [Then("there should exist only the RabbitMQ default exchanges")]
        public async Task ThenThereShouldExistOnlyTheRabbitMQDefaultExchanges()
        {
            Assert.True(_connection.IsOpen);
        }
    }
}

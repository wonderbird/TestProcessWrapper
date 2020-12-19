using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.MessageBrokers;
using DotNet.Testcontainers.Containers.Modules.MessageBrokers;
using katarabbitmq.bdd.tests.Helpers;
using RabbitMQ.Client;
using TechTalk.SpecFlow;

namespace katarabbitmq.bdd.tests.Steps
{
    // Comment out the following [Binding] attribute to make the tests use
    // an existing RabbitMq service
    //
    // When commenting out this [Binding] attribute, please uncomment the
    // [Binding] attribute on class
    // SetupAndTearDownRabbitMqWithoutTestContainer.

    [Binding]
    public class SetupAndTearDownRabbitMq
    {
        private static RabbitMqTestcontainer RabbitMqContainer;

        [BeforeTestRun]
        public static async Task StartRabbitMqContainer()
        {
            var testcontainersBuilder =
                new TestcontainersBuilder<RabbitMqTestcontainer>()
                    .WithMessageBroker(
                        new RabbitMqTestcontainerConfiguration { Username = "rabbitmq", Password = "rabbitmq" });

            RabbitMqContainer = testcontainersBuilder.Build();
            await RabbitMqContainer.StartAsync();
        }

        [BeforeFeature]
        public static void ConnectToRabbitMq()
        {
            var connectionFactory =
                new ConnectionFactory { Uri = new Uri(RabbitMqContainer.ConnectionString) };
            RabbitMq.Connection = connectionFactory.CreateConnection();
            RabbitMq.Channel = RabbitMq.Connection.CreateModel();

            RabbitMq.Hostname = RabbitMqContainer.Hostname;
            RabbitMq.Port = RabbitMqContainer.Port;
            RabbitMq.Username = RabbitMqContainer.Username;
            RabbitMq.Password = RabbitMqContainer.Password;
        }

        [AfterFeature]
        public static void DisconnectFromRabbitMq()
        {
            RabbitMq.Channel.Close();
            RabbitMq.Connection.Close();
        }

        [AfterTestRun]
        public static async Task ShutdownRabbitMqContainer()
        {
            await RabbitMqContainer.CleanUpAsync();
            await RabbitMqContainer.DisposeAsync();
        }
    }
}
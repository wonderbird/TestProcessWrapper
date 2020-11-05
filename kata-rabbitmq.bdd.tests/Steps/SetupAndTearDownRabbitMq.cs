using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.MessageBrokers;
using DotNet.Testcontainers.Containers.Modules.MessageBrokers;
using RabbitMQ.Client;
using TechTalk.SpecFlow;

namespace kata_rabbitmq.bdd.tests.Steps
{
    [Binding]
    public class SetupAndTearDownRabbitMq
    {
        [BeforeTestRun]
        public static async Task StartRabbitMqContainer()
        {
            var testcontainersBuilder = new TestcontainersBuilder<RabbitMqTestcontainer>()
                .WithMessageBroker(new RabbitMqTestcontainerConfiguration
                {
                    Username = "rabbitmq",
                    Password = "rabbitmq",
                });

            RabbitMq.Container = testcontainersBuilder.Build();
            await RabbitMq.Container.StartAsync();
        }
        
        [BeforeFeature]
        public static void ConnectToRabbitMq()
        {
            var connectionFactory = new ConnectionFactory { Uri = new Uri(RabbitMq.Container.ConnectionString) };
            RabbitMq.Connection = connectionFactory.CreateConnection();
            RabbitMq.Channel = RabbitMq.Connection.CreateModel();
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
            await RabbitMq.Container.CleanUpAsync();
            await RabbitMq.Container.DisposeAsync();
        }
    }
}
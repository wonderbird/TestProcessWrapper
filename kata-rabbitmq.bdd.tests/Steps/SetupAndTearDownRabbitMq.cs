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
        public static async Task BeforeTestRun()
        {
            var testcontainersBuilder = new TestcontainersBuilder<RabbitMqTestcontainer>()
                .WithMessageBroker(new RabbitMqTestcontainerConfiguration
                {
                    Username = "rabbitmq",
                    Password = "rabbitmq",
                });

            RabbitMq.Container = testcontainersBuilder.Build();
            await RabbitMq.Container.StartAsync();

            var connectionFactory = new ConnectionFactory { Uri = new Uri(RabbitMq.Container.ConnectionString) };
            RabbitMq.Connection = connectionFactory.CreateConnection();

            RabbitMq.Channel = RabbitMq.Connection.CreateModel();
        }

        [AfterTestRun]
        public static async Task AfterTestRun()
        {
            await RabbitMq.Container.CleanUpAsync();
            await RabbitMq.Container.DisposeAsync();
        }
    }
}
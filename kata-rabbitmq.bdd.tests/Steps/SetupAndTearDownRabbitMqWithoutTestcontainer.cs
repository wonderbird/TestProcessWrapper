using katarabbitmq.infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Globalization;
using TechTalk.SpecFlow;
using Xunit;

namespace katarabbitmq.bdd.tests.Steps
{
    // Uncomment the following [Binding] attribute to make the tests use
    // an existing RabbitMq service
    //
    // When uncommenting this [Binding] attribute, please comment out the
    // [Binding] attribute on class SetupAndTearDownRabbitMq.

    //[Binding]
    public class SetupAndTearDownRabbitMqWithoutTestcontainer
    {
        [BeforeFeature]
        public static void ConnectToRabbitMq()
        {
            RabbitMq.Hostname = "localhost";
            RabbitMq.Port = 5672;
            RabbitMq.Username = "guest";
            RabbitMq.Password = "guest";

            var configuration = new Dictionary<string, string>
            {
                ["RabbitMq:HostName"] = RabbitMq.Hostname,
                ["RabbitMq:Port"] = RabbitMq.Port.ToString(CultureInfo.InvariantCulture),
                ["RabbitMq:UserName"] = RabbitMq.Username,
                ["RabbitMq:Password"] = RabbitMq.Password
            };
            var configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(configuration).Build();

            var logger = new NullLogger<RabbitMqConnection>();

            var rabbitMqConnection = new RabbitMqConnection(logger, configurationRoot);
            rabbitMqConnection.TryConnect();
            Assert.True(rabbitMqConnection.IsConnected, "failed to connect to RabbitMQ");

            RabbitMq.Channel = rabbitMqConnection.Channel;
            RabbitMq.Connection = rabbitMqConnection.Connection;
        }

        [AfterFeature]
        public static void DisconnectFromRabbitMq()
        {
            RabbitMq.Channel.Close();
            RabbitMq.Connection.Close();
        }
    }
}

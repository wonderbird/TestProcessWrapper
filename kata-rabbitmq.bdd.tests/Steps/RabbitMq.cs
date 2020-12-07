using DotNet.Testcontainers.Containers.Modules.MessageBrokers;
using RabbitMQ.Client;

namespace katarabbitmq.bdd.tests.Steps
{
    public static class RabbitMq
    {
        public static RabbitMqTestcontainer Container { get; set; }
        public static IConnection Connection { get; set; }
        public static IModel Channel { get; set; }
    }
}

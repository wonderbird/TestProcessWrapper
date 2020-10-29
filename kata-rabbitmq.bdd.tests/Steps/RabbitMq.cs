using DotNet.Testcontainers.Containers.Modules.MessageBrokers;
using RabbitMQ.Client;

namespace kata_rabbitmq.bdd.tests.Steps
{
    public static class RabbitMq
    {
        public static RabbitMqTestcontainer Container { get; set; }
        public static IConnection Connection { get; set; }
        public static IModel Channel { get; set; }
    }
}
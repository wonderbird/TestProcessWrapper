using RabbitMQ.Client;

namespace katarabbitmq.infrastructure
{
    public interface IRabbitMqConnection
    {
        public bool IsConnected { get; }

        public IModel Channel { get; }

        public string QueueName { get; }

        void TryConnect();

        void Disconnect();
    }
}
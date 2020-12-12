using RabbitMQ.Client;

namespace katarabbitmq.infrastructure
{
    public interface IRabbitMqConnection
    {
        public bool IsConnected { get; }

        public IModel Channel { get; }

        void TryConnect();

        void Disconnect();
    }
}

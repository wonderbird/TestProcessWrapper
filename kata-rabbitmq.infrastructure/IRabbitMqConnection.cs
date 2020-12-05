namespace kata_rabbitmq.infrastructure
{
    public interface IRabbitMqConnection
    {
        public bool IsConnected { get; }
        
        void TryConnect();
        
        void Disconnect();
    }
}
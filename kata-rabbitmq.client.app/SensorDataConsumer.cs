using kata_rabbitmq.infrastructure;
using Microsoft.Extensions.Logging;

namespace kata_rabbitmq.client.app
{
    public class SensorDataConsumer : RabbitMqConnectedService
    {
        public SensorDataConsumer(IRabbitMqConnection rabbit, ILogger<SensorDataConsumer> logger)
            : base(rabbit, logger)
        {
        }
    }
}

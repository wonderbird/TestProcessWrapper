using katarabbitmq.infrastructure;
using Microsoft.Extensions.Logging;

namespace katarabbitmq.client.app
{
    public class SensorDataConsumer : RabbitMqConnectedService
    {
        public SensorDataConsumer(IRabbitMqConnection rabbit, ILogger<SensorDataConsumer> logger)
            : base(rabbit, logger)
        {
        }
    }
}

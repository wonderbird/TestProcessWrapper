using kata_rabbitmq.infrastructure;
using Microsoft.Extensions.Logging;

namespace kata_rabbitmq.robot.app
{
    public class SensorDataSender : RabbitMqConnectedService
    {
        public SensorDataSender(IRabbitMqConnection rabbit, ILogger<SensorDataSender> logger)
            : base(rabbit, logger)
        {
        }
    }
}
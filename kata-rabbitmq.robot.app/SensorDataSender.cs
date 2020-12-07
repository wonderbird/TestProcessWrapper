using katarabbitmq.infrastructure;
using Microsoft.Extensions.Logging;

namespace katarabbitmq.robot.app
{
    public class SensorDataSender : RabbitMqConnectedService
    {
        public SensorDataSender(IRabbitMqConnection rabbit, ILogger<SensorDataSender> logger)
            : base(rabbit, logger)
        {
        }
    }
}

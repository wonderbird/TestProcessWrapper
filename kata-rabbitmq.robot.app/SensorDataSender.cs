using katarabbitmq.infrastructure;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace katarabbitmq.robot.app
{
    public class SensorDataSender : RabbitMqConnectedService
    {
        private ILogger<SensorDataSender> _logger;

        public SensorDataSender(IRabbitMqConnection rabbit, ILogger<SensorDataSender> logger)
            : base(rabbit, logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteSensorLoopBody(CancellationToken stoppingToken)
        {
            await base.ExecuteSensorLoopBody(stoppingToken);

            if (Rabbit.IsConnected)
            {
                TODO: Routing Key muss noch an die Exchange gebunden werden
                    
                var message = "Es ist heiß hier.";
                var body = Encoding.UTF8.GetBytes(message);
                Rabbit.Channel.BasicPublish("robot", "sensors", null, body);
                
                _logger.LogInformation("Sent a message");
            }
        }
    }
}

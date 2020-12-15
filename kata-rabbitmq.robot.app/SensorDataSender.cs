using katarabbitmq.infrastructure;
using katarabbitmq.model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System;

namespace katarabbitmq.robot.app
{
    public class SensorDataSender : RabbitMqConnectedService
    {
        private ILogger<SensorDataSender> _logger;

        public SensorDataSender(IRabbitMqConnection rabbit, ILogger<SensorDataSender> logger)
            : base(rabbit, logger)
        {
            _logger = logger;
            DelayAfterEachLoop = TimeSpan.FromMilliseconds(50);
        }

        protected override async Task ExecuteSensorLoopBody(CancellationToken stoppingToken)
        {
            await base.ExecuteSensorLoopBody(stoppingToken);

            if (Rabbit.IsConnected)
            {
                var measurement = new LightSensorValue {ambient = 7};
                var message = JsonConvert.SerializeObject(measurement, Formatting.None);
                var body = Encoding.UTF8.GetBytes(message);
                
                Rabbit.Channel.BasicPublish("", "sensors", null, body);
                
                _logger.LogInformation($"Sent '{message}'");
            }
        }
    }
}

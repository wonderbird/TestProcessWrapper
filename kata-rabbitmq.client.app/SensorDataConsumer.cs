using katarabbitmq.infrastructure;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace katarabbitmq.client.app
{
    public class SensorDataConsumer : RabbitMqConnectedService
    {
        private EventingBasicConsumer _consumer;
        private string _consumerTag;
        private ILogger<SensorDataConsumer> _logger;

        public SensorDataConsumer(IRabbitMqConnection rabbit, ILogger<SensorDataConsumer> logger)
            : base(rabbit, logger)
        {
            _logger = logger;
            DelayAfterEachLoop = TimeSpan.FromMilliseconds(50);
        }

        protected override async Task ExecuteSensorLoopBody(CancellationToken stoppingToken)
        {
            await base.ExecuteSensorLoopBody(stoppingToken);

            //Logger.LogInformation("Sensor data: 1");
        }

        private void receiveSensorData(object sender, BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();
            _logger.LogInformation($"Sensor data: {body.Length} bytes");
        }
    }
}

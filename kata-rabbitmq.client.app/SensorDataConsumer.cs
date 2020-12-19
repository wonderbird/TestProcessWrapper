using katarabbitmq.infrastructure;
using katarabbitmq.model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace katarabbitmq.client.app
{
    public class SensorDataConsumer : RabbitMqConnectedService
    {
        private EventingBasicConsumer _consumer;
        private readonly ILogger<SensorDataConsumer> _logger;

        public SensorDataConsumer(IRabbitMqConnection rabbit, ILogger<SensorDataConsumer> logger)
            : base(rabbit, logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteSensorLoopBody(CancellationToken stoppingToken)
        {
            await base.ExecuteSensorLoopBody(stoppingToken);

            if (Rabbit.IsConnected && _consumer == null)
            {
                ConnectEventingConsumer();
            }

            if (!Rabbit.IsConnected && _consumer != null)
            {
                _consumer = null;
            }
        }

        private void ConnectEventingConsumer()
        {
            _consumer = new EventingBasicConsumer(Rabbit.Channel);
            _consumer.Received += ReceiveSensorData;
            Rabbit.Channel.BasicConsume(_consumer, "sensors");
        }

        private void ReceiveSensorData(object sender, BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var measurement = JsonConvert.DeserializeObject<LightSensorValue>(message);
            
            _logger.LogInformation($"Sensor data: {measurement}");
        }
    }
}

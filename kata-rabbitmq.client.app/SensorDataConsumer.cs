using System.Text;
using System.Threading;
using System.Threading.Tasks;
using katarabbitmq.infrastructure;
using katarabbitmq.model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace katarabbitmq.client.app
{
    public class SensorDataConsumer : RabbitMqConnectedService
    {
        private readonly ILogger<SensorDataConsumer> _logger;
        private EventingBasicConsumer _consumer;

        public SensorDataConsumer(IRabbitMqConnection rabbit, ILogger<SensorDataConsumer> logger)
            : base(rabbit, logger) =>
            _logger = logger;

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
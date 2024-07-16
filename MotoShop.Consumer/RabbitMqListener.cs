using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MotoShop.Domain.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Consumer
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IOptions<RabbitMqSettings> _options;

        public RabbitMqListener(IOptions<RabbitMqSettings> options)
        {
            _options = options;
            var factory = new ConnectionFactory() { HostName="localhost"};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(_options.Value.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null); 
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (obj, basicDeliver) =>
            {
                var body = basicDeliver.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                Debug.WriteLine("Received message: {0}", message);

                _channel.BasicAck(basicDeliver.DeliveryTag, false);
            };

            _channel.BasicConsume(_options.Value.QueueName,false, consumer);
            
            return Task.CompletedTask;
           
        }
        public override void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
            base.Dispose(); 
        }
    }
}

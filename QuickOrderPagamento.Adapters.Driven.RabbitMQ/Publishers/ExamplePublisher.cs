using QuickOrderPagamento.Adapters.Driven.RabbitMQ.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickOrderPagamento.Adapters.Driven.RabbitMQ.Publishers
{
    public class ExamplePublisher
    {
        private readonly RabbitMQSettings _settings;

        public ExamplePublisher(RabbitMQSettings settings)
        {
            _settings = settings;
        }

        public void Publish(string message)
        {
            var factory = new ConnectionFactory() { HostName = _settings.HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _settings.QueueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: _settings.QueueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}




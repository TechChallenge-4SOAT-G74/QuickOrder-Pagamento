using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace QuickOrderPagamento.Infra.MQ
{
    public class RabbitMqPub<T> : IRabbitMqPub<T> where T : class
    {
        
        private readonly IModel _channel;

        public RabbitMqPub(IOptions<RabbitMqSettings> configuration)
        {
            var factory = new ConnectionFactory
            {
                // "guest"/"guest" by default, limited to localhost connections
                UserName = configuration.Value.UserName,
                Password = configuration.Value.Password,
                VirtualHost = "/",
                HostName = configuration.Value.Host,
                Port = Int32.Parse(configuration.Value.Port)
            };

            IConnection connection = factory.CreateConnection();

            _channel = connection.CreateModel();
        }

        public void Publicar(T obj, string routingKey, string queue)
        {
            _channel.QueueDeclare(queue: queue,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            _channel.ExchangeDeclare(exchange: "pagamento", type: ExchangeType.Direct);

            _channel.QueueBind(queue: queue,
             exchange: "pagamento",
             routingKey: routingKey);

            string mensagem = JsonSerializer.Serialize(obj);
            var body = Encoding.UTF8.GetBytes(mensagem);

            _channel.BasicPublish(exchange: "pagamento",
                routingKey: routingKey,
                basicProperties: null,
                body: body
                );
        }
    }
}

using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.DataBase.RabbitMQ
{
    public class RabbitMQ
    {
        public interface IRabbitMQService
        {
            void Publish(string queueName, string message);
            public void PublishWithExchange(string exchangeName, string routingKey, string message);
            public void CreateQueue(string queueName, bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null);
            public QueueDeclareOk GetQueueInfo(string queueName);
            public void DeleteQueue(string queueName);
            public void PurgeQueue(string queueName);
            public void Dispose();
        }
        public class RabbitMQService : IRabbitMQService
        {
            private readonly IConfiguration Configuration;
            private readonly IConnection _connection;
            private readonly IModel _channel;

            public RabbitMQService(IConfiguration configuration)
            {
                Configuration = configuration;

                // تنظیم RabbitMQ ConnectionFactory با خواندن مقادیر از IConfiguration
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["PublicFunction:DataBase:RabbitMQ:HostName"],
                    VirtualHost = Configuration["PublicFunction:DataBase:RabbitMQ:VirtualHost"],
                    UserName = Configuration["PublicFunction:DataBase:RabbitMQ:UserName"],
                    Password = Configuration["PublicFunction:DataBase:RabbitMQ:Password"],
                    Port = int.Parse(Configuration["PublicFunction:DataBase:RabbitMQ:Port"])
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();


                // ایجاد صف (Queue) در صورت عدم وجود
                _channel.QueueDeclare(queue: "logQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            }

            public void Publish(string queueName, string message)
            {
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }

            public void PublishWithExchange(string exchangeName, string routingKey, string message)
            {
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);
            }

            public void CreateQueue(string queueName, bool durable = false, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null)
            {
                _channel.QueueDeclare(queue: queueName, durable: durable, exclusive: exclusive, autoDelete: autoDelete, arguments: arguments);
            }

            public QueueDeclareOk GetQueueInfo(string queueName)
            {
                return _channel.QueueDeclarePassive(queueName);
            }

            public void DeleteQueue(string queueName)
            {
                _channel.QueueDelete(queue: queueName);
            }

            public void PurgeQueue(string queueName)
            {
                _channel.QueuePurge(queueName);
            }

            public void Dispose()
            {
                _channel?.Close();
                _connection?.Close();
            }
        }
    }
}

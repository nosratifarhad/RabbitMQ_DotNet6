using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQApplication.MessageBroker.Contracts;
using System.Text;

namespace RabbitMQApplication.MessageBroker;

public class MessageProducer : IMessageProducer
{
    public MessageProducer()
    {
    }

    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "http://localhost:15672/",
            UserName = "guest",
            Password = "guest",
        };

        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare("students", exclusive: false);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: "students", body: body);
    }
}

using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQApplication.MessageBroker.Contracts;
using System.Text;

namespace RabbitMQApplication.MessageBroker;

public class MessageProducer : IMessageProducer
{
    public void SendMessage<T>(T message)
    {
        ConnectionFactory factory = new ConnectionFactory();
        factory.Port = AmqpTcpEndpoint.UseDefaultPort;
        try
        {
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("product", exclusive: false);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: "product", body: body);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

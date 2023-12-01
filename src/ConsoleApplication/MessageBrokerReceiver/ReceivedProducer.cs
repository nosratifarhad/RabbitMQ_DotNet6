using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReceiveRabbitMQConsoleApp.MessageBrokerReceiver.Contracts;
using System.Text;

namespace ReceiveRabbitMQConsoleApp.MessageBrokerReceiver;

public class ReceivedProducer : IReceivedProducer
{
    #region Fields
    private ConnectionFactory _factory;
    #endregion Fields

    #region Ctor

    public ReceivedProducer(ConnectionFactory factory)
    {
        _factory = factory;
        _factory.Port = AmqpTcpEndpoint.UseDefaultPort;
    }

    #endregion Ctor

    #region Methods

    public async Task ReceivedMessageProducer()
    {
        try
        {
            var connection = _factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("product", exclusive: false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Product message received: {0}", message);
            };

            channel.BasicConsume(queue: "product", autoAck: true, consumer: consumer);

            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Exception Message : {0}", ex.Message);
            throw new Exception(ex.Message);
        }

    }

    #endregion Methods
}

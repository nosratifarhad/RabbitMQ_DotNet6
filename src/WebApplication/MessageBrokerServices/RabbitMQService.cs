using RabbitMQ.Client;
using RabbitMQApplication.Extensions;
using RabbitMQApplication.MessageBrokerServices.Contracts;

namespace RabbitMQApplication.MessageBrokerServices;

public class RabbitMQService : IRabbitMQService
{
    #region Fields
    private ConnectionFactory _factory;
    #endregion Fields

    #region Ctor
    public RabbitMQService(ConnectionFactory factory)
    {
        //Console.WriteLine("Ctor: {0}", Thread.CurrentThread.ManagedThreadId);
        this._factory = factory;
        this._factory.Port = AmqpTcpEndpoint.UseDefaultPort;
    }
    #endregion Ctor

    #region Methods

    public void PublishMessage<T>(T message)
    {
        try
        {
            var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            var body = message.SerializeObjectExtension();

            channel.QueueDeclare("product", exclusive: false);
            channel.BasicPublish(exchange: "", routingKey: "product", body: body);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception Message : {0}", ex.Message);
            throw new Exception(ex.Message);
        }
    }

    #endregion Methods

    #region Dispose

    public void Dispose()
    {
        //Console.WriteLine("Dispose : {0}", Thread.CurrentThread.ManagedThreadId);
        GC.Collect();
    }

    #endregion Dispose

}
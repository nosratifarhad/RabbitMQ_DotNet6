namespace RabbitMQApplication.MessageBroker.Contracts;

public interface IMessageProducer
{
    void SendMessage<T>(T message);

}

namespace RabbitMQApplication.MessageBrokerServices.Contracts;

public interface IRabbitMQService : IDisposable
{
    void PublishMessage<T>(T message);

}
namespace ReceiveRabbitMQConsoleApp.MessageBrokerReceiver.Contracts;

public interface IReceivedProducer
{
    Task ReceivedMessageProducer();
}

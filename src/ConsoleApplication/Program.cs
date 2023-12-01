using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using ReceiveRabbitMQConsoleApp.MessageBrokerReceiver;
using ReceiveRabbitMQConsoleApp.MessageBrokerReceiver.Contracts;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(async (hostContext, services) =>
            {
                #region AddServices

                services.AddScoped<ConnectionFactory>();

                services.AddTransient<IReceivedProducer, ReceivedProducer>();

                var receivedProducer =
                services.BuildServiceProvider()
                .GetService<IReceivedProducer>();

                await Task.Run(
                     async
                     ()
                     => await receivedProducer.ReceivedMessageProducer());

                #endregion AddServices
            });
}
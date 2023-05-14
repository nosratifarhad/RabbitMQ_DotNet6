using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using ReceiveRabbitMQConsoleApp.MessageBrokerReceiver;
using ReceiveRabbitMQConsoleApp.MessageBrokerReceiver.Contracts;

public class Program
{
    private static IServiceProvider serviceProvider;

    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args);

        //ConfigureServices();

        host.Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(async (hostContext, services) =>
            {
                #region AddServices
                
                services.AddScoped<ConnectionFactory>();

                services.AddTransient<IReceivedProducer, ReceivedProducer>();
                
                var receivedProducer =
                services.BuildServiceProvider().GetService<IReceivedProducer>();

                Task.Run(async () => await receivedProducer.ReceivedMessageProducer());

                #endregion AddServices
            });


    //private static void ConfigureServices()
    //{
    //    var services = new ServiceCollection();


    //}

}
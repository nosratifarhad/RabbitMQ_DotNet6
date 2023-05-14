using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReceiveRabbitMQConsoleApp;
using System;
using System.ComponentModel;
using static System.Formats.Asn1.AsnWriter;

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
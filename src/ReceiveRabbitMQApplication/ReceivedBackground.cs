
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ReceiveRabbitMQApplication;

public class ReceivedBackground : BackgroundService
{
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Task.Delay(2000).Wait();

            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.VirtualHost = "/";
            factory.HostName = "localhost";
            factory.Port = AmqpTcpEndpoint.UseDefaultPort; 
            
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            channel.QueueDeclare("students", exclusive: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message received: {message}");
            };

            channel.BasicConsume(queue: "students", autoAck: true, consumer: consumer);

        }
        await Task.Run(() => Task.CompletedTask);

    }

}

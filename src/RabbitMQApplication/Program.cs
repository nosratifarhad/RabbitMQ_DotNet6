using RabbitMQ.Client;
using RabbitMQApplication.Domain;
using RabbitMQApplication.Infra.Repositories.ReadRepositories.ProductReadRepositories;
using RabbitMQApplication.Infra.Repositories.WriteRepositories.ProductWriteRepositories;
using RabbitMQApplication.MessageBrokerServices;
using RabbitMQApplication.MessageBrokerServices.Contracts;
using RabbitMQApplication.Services;
using RabbitMQApplication.Services.Contract;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Dependency Injection

builder.Services.AddScoped<ConnectionFactory>();

builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
builder.Services.AddScoped<IProductReadRepository, ProductReadRepository>();

#endregion Dependency Injection


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

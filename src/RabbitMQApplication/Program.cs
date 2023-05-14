using RabbitMQApplication.Controllers;
using RabbitMQApplication.Domain;
using RabbitMQApplication.Infra.Repositories.ReadRepositories.ProductReadRepositories;
using RabbitMQApplication.Infra.Repositories.WriteRepositories.ProductWriteRepositories;
using RabbitMQApplication.MessageBroker;
using RabbitMQApplication.MessageBroker.Contracts;
using RabbitMQApplication.Services;
using RabbitMQApplication.Services.Contract;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region DIC

builder.Services.AddScoped<IMessageProducer, MessageProducer>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
builder.Services.AddScoped<IProductReadRepository, ProductReadRepository>();

#endregion DIC


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

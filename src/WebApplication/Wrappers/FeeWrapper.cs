using Newtonsoft.Json;
using RabbitMQApplication.Domain.Entitys;
using RabbitMQApplication.MessageBroker.Contracts;
using RabbitMQApplication.Wrappers.Contracts;
using System.Text;

namespace RabbitMQApplication.Wrappers
{
    public class FeeWrapper : IFeeWrapper
    {
        private readonly IMessageProducer _messagePublisher;

        public FeeWrapper(IMessageProducer messagePublisher)
        {
            this._messagePublisher = messagePublisher;
        }

        public async Task CreateProductAsync(Product product)
        {
            string json = JsonConvert.SerializeObject(product);
            byte[] message = Encoding.UTF8.GetBytes(json);

            _messagePublisher.SendMessage(message);
        }

        public async Task DeleteProductAsync(Product product)
        {
            string json = JsonConvert.SerializeObject(product);
            byte[] message = Encoding.UTF8.GetBytes(json);

            _messagePublisher.SendMessage(message);
        }

        public async Task DeleteProductAsync(int productId)
        {
            string json = JsonConvert.SerializeObject(productId);
            byte[] message = Encoding.UTF8.GetBytes(json);

            _messagePublisher.SendMessage(message);
        }

        public async Task UpdateProductAsync(Product product)
        {
            string json = JsonConvert.SerializeObject(product);
            byte[] message = Encoding.UTF8.GetBytes(json);

            _messagePublisher.SendMessage(message);
        }
    }
}

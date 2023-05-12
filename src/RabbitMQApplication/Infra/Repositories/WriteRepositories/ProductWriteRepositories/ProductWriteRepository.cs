using RabbitMQApplication.Domain;
using RabbitMQApplication.Domain.Entitys;

namespace RabbitMQApplication.Infra.Repositories.WriteRepositories.ProductWriteRepositories
{
    public class ProductWriteRepository : IProductWriteRepository
    {
        public async Task<int> CreateProductAsync(Product product)
        {
            await Task.Delay(1000);
            return Random.Shared.Next(1, 100);
        }

        public async Task DeleteProductAsync(int productId)
        {
            await Task.Delay(1000);
        }
         
        public async Task UpdateProductAsync(Product product)
        {
            await Task.Delay(1000);
        }
    }
}

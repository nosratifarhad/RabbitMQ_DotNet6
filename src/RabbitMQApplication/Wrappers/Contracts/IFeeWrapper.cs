using RabbitMQApplication.Domain.Entitys;

namespace RabbitMQApplication.Wrappers.Contracts
{
    public interface IFeeWrapper
    {
        Task CreateProductAsync(Product product);

        Task UpdateProductAsync(Product product);

        Task DeleteProductAsync(Product product);

        Task DeleteProductAsync(int productId);
    }
}

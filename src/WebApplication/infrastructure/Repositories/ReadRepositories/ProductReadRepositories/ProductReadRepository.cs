using RabbitMQApplication.Domain;
using RabbitMQApplication.Domain.Entitys;

namespace RabbitMQApplication.Infrastructure.Repositories.ReadRepositories.ProductReadRepositories
{
    public class ProductReadRepository : IProductReadRepository
    {
        public async Task<Product> GetProductAsync(int productId)
        {
            return await Task.Run(() => _mockProduct);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await Task.FromResult(new List<Product>()
            {
                _mockProduct,
            });

        }

        public async Task<bool> IsExistProductAsync(int productId)
        {
            return await Task.Run(() => false);
        }


        #region [ Private ]

        private Product _mockProduct
        {
            get
            {
                return new Product("inputModel.ProductName", "inputModel.ProductTitle", "inputModel.ProductDescription",
            "inputModel.MainImageName", "inputModel.MainImageTitle", "inputModel.MainImageUri", true,
            true, 0);
            }
        }

        #endregion [Private]

    }
}

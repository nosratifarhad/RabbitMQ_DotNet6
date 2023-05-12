using RabbitMQApplication.Domain;
using RabbitMQApplication.Domain.Entitys;
using RabbitMQApplication.InputModels.ProductInputModels;
using RabbitMQApplication.MessageBroker.Contracts;
using RabbitMQApplication.Services.Contract;
using RabbitMQApplication.ViewModels.ProductViewModels;

namespace RabbitMQApplication.Services
{
    public class ProductService : IProductService
    {
        #region Fields
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;

        private readonly IMessageProducer _messagePublisher;

        #endregion Fields

        #region Ctor

        public ProductService(
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
                    IMessageProducer messagePublisher)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _messagePublisher = messagePublisher;
        }

        #endregion Ctor

        #region Implement

        public async ValueTask<ProductViewModel> GetProductAsync(int productId)
        {
            if (productId <= 0)
                throw new NullReferenceException("Product Id Is Invalid");

            var product = await _productReadRepository.GetProductAsync(productId).ConfigureAwait(false);
            if (product == null)
                return new ProductViewModel();

            var productViewModel = CreateProductViewModelFromProduct(product);

            return productViewModel;
        }

        public async ValueTask<IEnumerable<ProductViewModel>> GetProductsAsync()
        {
            var products = await _productReadRepository.GetProductsAsync().ConfigureAwait(false);

            if (products == null || products.Count() == 0)
                return Enumerable.Empty<ProductViewModel>();

            var productViewModels = CreateProductViewModelsFromProducts(products);

            return productViewModels;
        }

        public async Task<int> CreateProductAsync(CreateProductInputModel inputModel)
        {
            if (inputModel == null)
                throw new NullReferenceException("Product Id Is Invalid");

            ValidateProductName(inputModel.ProductName);

            ValidateProductTitle(inputModel.ProductTitle);

            var product = CreateProductEntityFromInputModel(inputModel);

            int productId = await _productWriteRepository.CreateProductAsync(product).ConfigureAwait(false);

            product.setProductId(productId);

            OnCreated(product);

            return productId;
        }

        public async Task UpdateProductAsync(UpdateProductInputModel inputModel)
        {
            if (inputModel.ProductId <= 0)
                throw new NullReferenceException("ProductId Is Invalid.");

            ValidateProductName(inputModel.ProductName);

            ValidateProductTitle(inputModel.ProductTitle);

            await IsExistProduct(inputModel.ProductId).ConfigureAwait(false);

            var product = CreateProductEntityFromInputModel(inputModel);

            await _productWriteRepository.UpdateProductAsync(product).ConfigureAwait(false);

            OnUpdate(product);

        }

        public async Task DeleteProductAsync(int productId)
        {
            if (productId <= 0)
                throw new NullReferenceException("ProductId Is Invalid.");

            await IsExistProduct(productId).ConfigureAwait(false);

            await _productWriteRepository.DeleteProductAsync(productId).ConfigureAwait(false);

            OnDelete(productId);
        }

        #endregion Implement

        #region Private

        private async Task IsExistProduct(int productId)
        {
            var isExistProduct = await _productReadRepository.IsExistProductAsync(productId).ConfigureAwait(false);
            if (isExistProduct == false)
                throw new NullReferenceException("ProductId Is Not Found.");
        }

        private Product CreateProductEntityFromInputModel(CreateProductInputModel inputModel)
            => new Product(inputModel.ProductName, inputModel.ProductTitle, inputModel.ProductDescription, inputModel.MainImageName, inputModel.MainImageTitle, inputModel.MainImageUri, inputModel.IsExisting, inputModel.IsFreeDelivery, inputModel.Weight);

        private Product CreateProductEntityFromInputModel(UpdateProductInputModel inputModel)
            => new Product(inputModel.ProductId, inputModel.ProductName, inputModel.ProductTitle, inputModel.ProductDescription, inputModel.MainImageName, inputModel.MainImageTitle, inputModel.MainImageUri, inputModel.IsExisting, inputModel.IsFreeDelivery, inputModel.Weight);

        private ProductViewModel CreateProductViewModelFromProduct(Product product)
            => new ProductViewModel()
            {
                Id = product.ProductId.ToString(),
                ProductName = product.ProductName,
                ProductTitle = product.ProductTitle,
                ProductDescription = product.ProductDescription,
                MainImageName = product.MainImageName,
                MainImageTitle = product.MainImageTitle,
                MainImageUri = product.MainImageUri,
                IsExisting = product.IsExisting,
                IsFreeDelivery = product.IsFreeDelivery,
                Weight = product.Weight
            };

        private ProductViewModel CreateProductViewModelFromProductEntity(Product product)
            => new ProductViewModel()
            {
                Id = product.ProductId.ToString(),
                ProductName = product.ProductName,
                ProductTitle = product.ProductTitle,
                ProductDescription = product.ProductDescription,
                MainImageName = product.MainImageName,
                MainImageTitle = product.MainImageTitle,
                MainImageUri = product.MainImageUri,
                IsExisting = product.IsExisting,
                IsFreeDelivery = product.IsFreeDelivery,
                Weight = product.Weight
            };

        private IEnumerable<ProductViewModel> CreateProductViewModelsFromProducts(IEnumerable<Product> products)
        {
            ICollection<ProductViewModel> productViewModels = new List<ProductViewModel>();

            foreach (var product in products)
                productViewModels.Add(
                     new ProductViewModel()
                     {
                         Id = product.ProductId.ToString(),
                         ProductName = product.ProductName,
                         ProductTitle = product.ProductTitle,
                         ProductDescription = product.ProductDescription,
                         MainImageName = product.MainImageName,
                         MainImageTitle = product.MainImageTitle,
                         MainImageUri = product.MainImageUri,
                         IsExisting = product.IsExisting,
                         IsFreeDelivery = product.IsFreeDelivery,
                         Weight = product.Weight
                     });


            return productViewModels;
        }

        private void ValidateProductName(string productName)
        {
            if (string.IsNullOrEmpty(productName) || string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException(nameof(productName), "Product Name cannot be nul.l");
        }

        private void ValidateProductTitle(string productTitle)
        {
            if (string.IsNullOrEmpty(productTitle) || string.IsNullOrWhiteSpace(productTitle))
                throw new ArgumentException(nameof(productTitle), "Product Title cannot be null.");
        }

        //private async Task OnCreated(Product product)
        //    => await _feeWrapper.CreateProductAsync(product).ConfigureAwait(false);

        //private async Task OnUpdate(Product product)
        //    => await _feeWrapper.UpdateProductAsync(product).ConfigureAwait(false);

        //private async Task OnDelete(int productId)
        //    => await _feeWrapper.DeleteProductAsync(productId).ConfigureAwait(false);

        private void OnCreated(Product product)
             => _messagePublisher.SendMessage<Product>(product);

        private void OnUpdate(Product product)
            => _messagePublisher.SendMessage<Product>(product);

        private void OnDelete(int productId)
            => _messagePublisher.SendMessage<int>(productId);

        #endregion Private
    }
}

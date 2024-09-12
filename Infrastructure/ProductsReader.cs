using FurnitureStore3.Domain.Entities;
using FurnitureStore3.Domain.Services;

namespace FurnitureStore3.Infrastructure
{
    public class ProductsReader : IProductsReader
    {
        private readonly IRepository<Product> repository;
        private readonly IRepository<Category> categories;
        private readonly IRepository<PriceStory> priceStorys;

        public ProductsReader(IRepository<Product> products, IRepository<Category> categories, IRepository<PriceStory> priceStorys)
        {
            this.repository = products;
            this.categories = categories;
            this.priceStorys = priceStorys;
        }
        public async Task<Product?> FindProductAsync(int ProductId) =>
    await repository.FindAsync(ProductId);
        public async Task<List<Product>> GetAllProductsAsync() => await repository.GetAllAsync();
        public async Task<List<Product>> FindProductsAsync(string searchString, int categoryId, int productVisibility) => (searchString, categoryId, productVisibility) switch
        {
            ("" or null, 0, 1) => await repository.GetAllAsync(),
            ("" or null, 0, 0) => await repository.FindWhere(product => product.Visibility == 0),
            (_, 0, 1) => await repository.FindWhere(product => product.Name.Contains(searchString) && product.Visibility == 1),
            (_, 0, 0) => await repository.FindWhere(product => product.Name.Contains(searchString) && product.Visibility == 0),
            (_, _, 1 ) => await repository.FindWhere(product => product.CategoryId == categoryId && (product.Name.Contains(searchString) && product.Visibility == 1)),
            (_, _, 0 ) => await repository.FindWhere(product => product.CategoryId == categoryId && (product.Name.Contains(searchString)) && product.Visibility == 0),
        };
        public async Task<List<Category>> GetCategoriesAsync() => await categories.GetAllAsync();
    }
}

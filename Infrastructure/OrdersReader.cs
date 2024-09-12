using FurnitureStore3.Domain.Entities;
using FurnitureStore3.Domain.Services;

namespace FurnitureStore3.Infrastructure
{
    public class OrdersReader : IOrdersReader
    {
        private readonly IRepository<Product> repository;
        private readonly IRepository<Category> categories;
        private readonly IRepository<Order> orders;
        private readonly IRepository<User> users;
        private readonly IRepository<CompleteOrder> completeOrders;
        private readonly IRepository<PriceStory> priceStorys;

        public OrdersReader(IRepository<Product> products, IRepository<Category> categories, IRepository<Order> orders, IRepository<CompleteOrder> completeOrders, IRepository<PriceStory> priceStorys, IRepository<User> users)
        {
            this.repository = products;
            this.users = users;
            this.categories = categories;            
            this.orders = orders;
            this.completeOrders = completeOrders;
            this.priceStorys = priceStorys;
        }
        public async Task<Product?> FindProductAsync(int ProductId) =>
    await repository.FindAsync(ProductId);
        public async Task<Order?> FindOrderAsync(int OrderId) =>
    await orders.FindAsync(OrderId);
        public async Task<List<Product>> GetAllProductsAsync() => await repository.GetAllAsync();
        //public async Task<List<Order>> GetAllOrdersAsync() => await repository.GetAllAsync();
        public async Task<List<Product>> FindProductsAsync(string searchString, int categoryId) => (searchString, categoryId) switch
        {
            ("" or null, 0) => await repository.GetAllAsync(),
            (_, 0) => await repository.FindWhere(product => product.Name.Contains(searchString)),
            (_, _) => await repository.FindWhere(product => product.CategoryId == categoryId &&(product.Name.Contains(searchString)))

        };
        public async Task<List<Category>> GetCategoriesAsync() => await categories.GetAllAsync();

        public async Task<List<PriceStory>> GetAllPriceStorysAsync() => await priceStorys.GetAllAsync123();
        public async Task<List<Order>> GetAllOrdersAsync() => await orders.GetAllAsync123();
        public async Task<List<User>> GetAllUsersAsync() => await users.GetAllAsync();
        public async Task<List<CompleteOrder>> GetAllCompleteOrders() => await completeOrders.GetAllAsync();
    }
}

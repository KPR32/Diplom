using FurnitureStore3.Domain.Entities;

namespace FurnitureStore3.Domain.Services
{
    public interface IOrdersReader
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<User>> GetAllUsersAsync();
        Task<List<Product>> FindProductsAsync(string searchString, int categoryId);
        Task<Product?> FindProductAsync(int productId);
        Task<Order?> FindOrderAsync(int orderId);
        Task<List<Product>> GetAllProductsAsync();
        Task<List<PriceStory>> GetAllPriceStorysAsync();
        Task<List<CompleteOrder>> GetAllCompleteOrders();
        Task<List<Category>> GetCategoriesAsync();
    }
}

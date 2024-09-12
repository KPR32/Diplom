using FurnitureStore3.Domain.Entities;
using FurnitureStore3.Domain.Services;

namespace FurnitureStore3.Infrastructure
{
    public class OrdersService : IOrdersService
    {
        private readonly IRepository<Order> orders;
        private readonly IRepository<CompleteOrder> completeOrders;

        public OrdersService(IRepository<Order> orders, IRepository<CompleteOrder> completeOrders)
        {
            this.orders = orders;
            this.completeOrders = completeOrders;
        }
        

        public async Task AddOrder(Order order)
        {
            await orders.AddAsync(order);
        }
        public async Task DeleteOrder(Order order)
        {
            await orders.DeleteAsync(order);
        }
        
        public async Task AddCompleteOrder(CompleteOrder completeOrder)
        {
            await completeOrders.AddAsync(completeOrder);
        }
    }
}

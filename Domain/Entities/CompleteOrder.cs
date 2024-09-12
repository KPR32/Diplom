using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FurnitureStore3.Domain.Entities
{
    public class CompleteOrder : Entity
    {
        public List<Order> Orders { get; set; }
        public List<Product> Products { get; set; }
        public string ClientUserId { get; set; }
        public string OrderProductsId { get; set; }
        public string OrderProductsName { get; set; }
        public string OrderProductsCount { get; set; }
        public string? OrderProductsPrice { get; set; }
        public int TotalPrice { get; set; }
        public DateTime OrderStart { get; set; }
        public DateTime OrderFinish { get; set; }

        [StringLength(100)]
        public string OrderAddress { get; set; } = null!;

        public string? TypePay { get; set; }
        public string? Key { get; set; }

    }
}

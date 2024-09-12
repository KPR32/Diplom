using System.ComponentModel.DataAnnotations;

namespace FurnitureStore3.Domain.Entities
{
    public class PriceStory : Entity
    {
        public int ProductId { get; set; }
        public int ProductNewPrice { get; set; }
        public DateTime ChangePrice { get; set; }
    }
}

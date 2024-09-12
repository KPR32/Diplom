using FurnitureStore3.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FurnitureStore3.ViewModels
{
    public class OrderViewModel
    {
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
        public List<PriceStory>? PriceStorys { get; set; }
        public List<CompleteOrder>? CompleteOrders { get; set; }
        public List<CompleteOrder>? CompleteOrders1 { get; set; }
        //public List<Category>? Categories1 { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        //public DateTime OrderStart { get; set; } = DateTime.Now;
        //[Display(Name = "Выберите дату доставки заказа")]
        //[Range(typeof(DateTime), "22.05.2024", "29.05.2024", ErrorMessage = "введите корректные значения даты доставки (до 7 календарных дней, начиная с текущего)")]
        //public DateTime OrderFinish { get; set; } = DateTime.Now;

        //[MinLength(6, ErrorMessage = "Введите адрес корректно")]
        //[MaxLength(1000, ErrorMessage = "Введите адрес корректно")]
        //[Display(Name = "Введите адрес доставки товара (Укажите улицу, дом, кв.)")]
        //[Required(ErrorMessage = "Введите адрес доставки")]
        //public string OrderAddress { get; set; } = null!;

        public List<SelectListItem> Categories { get; set; } = new();
        //public List<SelectListItem> Products { get; set; } = new();             
    }
}

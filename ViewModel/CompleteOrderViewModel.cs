using FurnitureStore3.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FurnitureStore3.ViewModels
{
    public class CompleteOrderViewModel
    {
        public List<Order>? Orders { get; set; }
        public List<Product>? Products { get; set; }
        public List<SelectListItem>? Categories { get; set; } = new();
        public int[] OrderProductsId { get; set; }
        public string[] OrderProductsName { get; set; }
        public string[]? OrderProductsPrice { get; set; }

        //[Range(1, 10, ErrorMessage = "Введите корректное число(от 1 до 10)")]
        [Display(Name = "Заказ не совершен. Выберите число товаров")]
        [Required(ErrorMessage = "Заказ не совершен. Введите число товаров")]
        public int[] OrderProductsCount { get; set; }
        public int TotalPrice { get; set; }
        public DateTime OrderStart { get; set; } = DateTime.Now;

        [Display(Name = "Выберите дату доставки заказа")]
        [Range(typeof(DateTime), "27.06.2024", "05.07.2024", ErrorMessage = "Заказ не совершен. Введите корректные значения даты доставки (до 7 календарных дней, начиная с текущего)")]
        public DateTime OrderFinish { get; set; } = DateTime.Now;

        [MinLength(6, ErrorMessage = "Заказ не совершен. Введите адрес корректно")]
        [MaxLength(1000, ErrorMessage = "Заказ не совершен. Введите адрес корректно")]
        [Display(Name = "Введите адрес доставки товара (Укажите улицу, дом, кв.)")]
        [Required(ErrorMessage = "Заказ не совершен. Введите адрес доставки")]
        public string OrderAddress { get; set; }
        public string? TypePay { get; set; }
    }
}

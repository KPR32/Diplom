﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FurnitureStore3.ViewModels
{
    
    public class UpdateProductViewModel
    {        
        public int Id { get; set; }

        [MinLength(2, ErrorMessage = "Название не может быть короче двух символов")]
        [MaxLength(150, ErrorMessage = "Название не может быть таким длинным")]
        [Display(Name = "Название товара")]
        [Required(ErrorMessage = "Введите название товара")]
        public string ProductName { get; set; } = string.Empty;


        [MinLength(2, ErrorMessage = "Описание не может быть короче двух символов")]
        [MaxLength(1000, ErrorMessage = "Описание не может быть таким длинным")]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Введите описание товара")]
        public string Description { get; set; } = string.Empty;


        [Display(Name = "Вес")]
        [Range(0.1, 10000, ErrorMessage = "Введите корректный вес")]
        public double Weight { get; set; }



        [Display(Name = "Цена")]
        [Range(1, 100000, ErrorMessage = "Введите корректную цену")]
        public int Price { get; set; }

        [Display(Name = "Изображение")]
        [Required(ErrorMessage = "Добавьте изображение")]
        public IFormFile Photo { get; set; }
        public string? PhotoString { get; set; }



        [Required(ErrorMessage = "Укажите категорию")]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        
        [Range(0, 100, ErrorMessage = "Скидка должна иметь число от 0 до 100")]
        [Display(Name = "Скидка")]
        public int Discount { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();

        
        [Display(Name = "Количество")]
        public int CurrentCount { get; set; }
    }
}

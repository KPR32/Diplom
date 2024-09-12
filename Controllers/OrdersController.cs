using FurnitureStore3.Domain.Entities;
using FurnitureStore3.Domain.Services;
using FurnitureStore3.Infrastructure;
using FurnitureStore3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestSharp;
using System.Numerics;
using System;
using System.Web;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using FurnitureStore3.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata.Ecma335;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;

namespace FurnitureStore3.Controllers
{
    public class OrdersController: Controller
    {
        private readonly IOrdersReader reader;        
        private readonly IOrdersService ordersService1;
        private readonly IProductsService productsService;
        private readonly IWebHostEnvironment appEnvironment;


        public OrdersController(IOrdersReader reader,
           IOrdersService ordersService1,
           IWebHostEnvironment appEnvironment)
        {
            this.reader = reader;
            this.ordersService1 = ordersService1;
            this.appEnvironment = appEnvironment;            
        }

        [Authorize]
        public async Task<IActionResult> CompletedOrders()
        {
            var Orders1 = await reader.GetAllCompleteOrders();
            var Orders2 = Orders1.OrderByDescending(o => o.OrderFinish);
            IEnumerable<CompleteOrder> MyFunction()
            {
                return Orders2.ToList();
            }
            var myList = MyFunction().ToList();
            var viewModel = new ProductsCatalogViewModel
            {
                Orders = await reader.GetAllOrdersAsync(),
                Categories = await reader.GetCategoriesAsync(),
                Products = await reader.GetAllProductsAsync(),
                Users = await reader.GetAllUsersAsync(),
                CompleteOrders = myList
            };
            User.Identity.ToString();
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CheckStory(int ProductId)
        {
            var categories = await reader.GetCategoriesAsync();
            var products = await reader.GetAllProductsAsync();
            var Products1 = await reader.GetAllProductsAsync();
            int c1 = 0;
            foreach (Product product1 in products)
            {
                if (product1.Id == ProductId)
                {
                    c1 = product1.CategoryId;
                }
            }
            var Orders1 = await reader.GetAllCompleteOrders();
            var Orders2 = Orders1.OrderByDescending(o => o.OrderStart);
            IEnumerable<CompleteOrder> MyFunction()
            {
                return Orders2.ToList();
            }
            var myList = MyFunction().ToList();
            var viewModel = new OrderViewModel
            {
                ProductId = ProductId,
                CategoryId = c1,
                Products = await reader.GetAllProductsAsync(),
                CompleteOrders = await reader.GetAllCompleteOrders(),
                CompleteOrders1 = myList,
                PriceStorys = await reader.GetAllPriceStorysAsync(),
                Orders = await reader.GetAllOrdersAsync()
            };
            // загружаем список категорий (List<Category>)

            var product = await reader.FindProductAsync(ProductId);
            if (product is null)
            {
                return NotFound();
            }


            var productVm = new UpdateProductViewModel
            {
                Id = product.Id,
                ProductName = product.Name,
                Weight = Convert.ToDouble(product.Weight),
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                PhotoString = product.ImageUrl
            };

            var items = categories.Select(c =>
                new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            viewModel.Categories.AddRange(items);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var Orders1 = await reader.GetAllOrdersAsync();
            var Orders2 = Orders1.OrderBy(o=>o.ProductId);            
            IEnumerable<Order> MyFunction()
            {
                return Orders2.ToList();
            }
            var myList = MyFunction().ToList();

            var viewModel = new CompleteOrderViewModel
            {
                Orders = myList,                
                Products = await reader.GetAllProductsAsync()
            };
            var categories = await reader.GetCategoriesAsync();
            var items = categories.Select(c =>
                new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            viewModel.Categories.AddRange(items);            
            User.Identity.ToString();
            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> MyOrders(CompleteOrderViewModel orderVm)
        {
            var Orders1 = await reader.GetAllOrdersAsync();
            var Orders2 = Orders1.OrderBy(o => o.ProductId);
            IEnumerable<Order> MyFunction()
            {
                return Orders2.ToList();
            }
            var myList = MyFunction().ToList();
            var orderVm1 = new CompleteOrderViewModel
            {
                Products = await reader.GetAllProductsAsync(),
                Orders = myList
            };

            var Products = await reader.GetAllProductsAsync();
            var Orders = myList;

            if (!ModelState.IsValid)
            {
                return View(orderVm1);
            }
            if (orderVm.TypePay is null)
            {
                return View(orderVm1);
            }

            try
            {
                string a = "";
                string b = "";
                int c = 0;
                string d = "";
                string e = "";
                for (int i = 0;i < orderVm.OrderProductsCount.Length; i++)
                {                    
                    a += orderVm.OrderProductsCount[i].ToString();
                    a += ",";
                    b += orderVm.OrderProductsName[i].ToString();
                    b += ",";
                    d += (Convert.ToInt32(orderVm.OrderProductsPrice[i]) * orderVm.OrderProductsCount[i]).ToString();
                    d += ",";
                    e += orderVm.OrderProductsId[i].ToString();
                    e += ",";
                    foreach (Product product in Products)
                    {
                        if(product.Name == orderVm.OrderProductsName[i])
                        {
                            c += product.Price * orderVm.OrderProductsCount[i];
                            product.SoldCount += orderVm.OrderProductsCount[i];
                            product.CurrentCount -= orderVm.OrderProductsCount[i];
                            if (product.CurrentCount <= 0)
                            {
                                product.Visibility = 0;
                            }
                        }
                    }
                }
                a = a.Remove(a.Length - 1);
                b = b.Remove(b.Length - 1);
                d = d.Remove(d.Length - 1);
                string salt()
                {
                    using (Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes("password", 16)) // Генерация 128-битного ключа
                    {
                        return Convert.ToBase64String(keyGenerator.GetBytes(16));
                    }
                }
                string EncryptString(string plainText, string key)
                {
                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = Convert.FromBase64String(key);
                        aesAlg.IV = new byte[16]; // Инициализационный вектор

                        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                        using (MemoryStream msEncrypt = new MemoryStream())
                        {
                            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                                {
                                    swEncrypt.Write(plainText);
                                }
                            }
                            return Convert.ToBase64String(msEncrypt.ToArray());
                        }
                    }
                }
                string key = salt();
                var completeOrder = new CompleteOrder                                
                {
                    OrderProductsId = e,
                    OrderProductsName = b,
                    OrderProductsCount = a,
                    OrderProductsPrice = d,
                    ClientUserId = User.Identity.Name,
                    TotalPrice = c,
                    OrderStart = DateTime.UtcNow,
                    OrderFinish = orderVm.OrderFinish,
                    OrderAddress = EncryptString(orderVm.OrderAddress, key),
                    //OrderAddress = orderVm.OrderAddress,
                    TypePay = orderVm.TypePay,
                    Key = key
                };
                string wwwroot = appEnvironment.WebRootPath; // получаем путь до wwwroot               
                await ordersService1.AddCompleteOrder(completeOrder);
                foreach(Order order in Orders)
                {
                    if(order.ClientUserId == User.Identity.Name)
                    {
                        await ordersService1.DeleteOrder(order);
                    }
                }
            }
            catch (IOException)
            {
                ModelState.AddModelError("ioerror", "Не удалось сделать заказ.");
                return View(orderVm);

            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных.");
                return View(orderVm);
            }

            return RedirectToAction("Index", "products");
        }

        [HttpGet]        
        public async Task<IActionResult> AddOrder(int ProductId)
        {
            var categories = await reader.GetCategoriesAsync();
            var products = await reader.GetAllProductsAsync();
            var Products1 = await reader.GetAllProductsAsync();
            int c1 = 0;
            foreach(Product product1 in products)
            {
                if (product1.Id == ProductId)
                {
                    c1 = product1.CategoryId;
                }                
            }
            var viewModel = new OrderViewModel
            { 
                ProductId = ProductId,
                CategoryId = c1,
                Products= await reader.GetAllProductsAsync(),
                Orders = await reader.GetAllOrdersAsync()
            };  
            // загружаем список категорий (List<Category>)
            
            


            var product = await reader.FindProductAsync(ProductId);
            if (product is null)
            {
                return NotFound();
            }


            var productVm = new UpdateProductViewModel
            {
                Id = product.Id,
                ProductName = product.Name,
                Weight = Convert.ToDouble(product.Weight),
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                PhotoString = product.ImageUrl
            };

            var items = categories.Select(c =>
                new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            viewModel.Categories.AddRange(items);

            return View(viewModel);
        }

        [HttpPost]        
        public async Task<IActionResult> AddOrder(OrderViewModel orderVm)
        {
            
            // загружаем список категорий (List<Category>)
            var categories = await reader.GetCategoriesAsync();
            var products = await reader.GetAllProductsAsync();
            // получаем элементы для <select> с помощью нашего листа категорий
            // (List<SelectListItem>)
            var items = categories.Select(c =>
                new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

            var items1 = products.Select(p => new SelectListItem { Value = p.Name, Text = p.Id.ToString() });
            // добавляем список в модель представления            
            orderVm.Categories.AddRange(items);            


            

            
            var product = await reader.FindProductAsync(orderVm.ProductId);
            
            if (product is null)
            {
                ModelState.AddModelError("not_found", "Товар не найден!");
                return View(orderVm);
            }


            try
            {
                var order = new Order
                {
                    ClientUserId = User.Identity.Name,
                    ProductId = orderVm.ProductId,
                    Price = orderVm.Price,
                    OrderStart = DateTime.Now,
                    OrderFinish = DateTime.Now,
                    OrderAddress = "0"
                };
                string wwwroot = appEnvironment.WebRootPath; // получаем путь до wwwroot               
                await ordersService1.AddOrder(order);
            }
            catch (IOException)
            {
                ModelState.AddModelError("ioerror", "Не удалось сохранить файл.");
                return View(orderVm);

            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных.");
                return View(orderVm);
            }

            return RedirectToAction("Index", "products");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await reader.FindOrderAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
            await ordersService1.DeleteOrder(order);
            return View(order);
        }


        [HttpPost]        
        public async Task<IActionResult> DeleteOrderPost(int id)
        {
            var order = await reader.FindOrderAsync(id);
            try
            {
                await ordersService1.DeleteOrder(order);
            }
            catch (Exception)
            {
                ModelState.AddModelError("database", "Ошибка при удалении");
                return View(order);

            }
            return RedirectToAction("Index", "Products");
        }
    }
}


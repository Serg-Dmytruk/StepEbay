using Microsoft.EntityFrameworkCore;
using StepEbay.Data;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Models.Products;

namespace StepEbay.Admin.Api.Common.Services.DbSeeder
{
    public class Seeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductDbService _productDbService;
        private readonly IProductStateDbService _productStateDbService;
        private readonly ICategoryDbService _categoryDbService;
        public Seeder(ApplicationDbContext context, IProductDbService product, ICategoryDbService category, IProductStateDbService productState)
        {
            _context = context;
            _productDbService = product;
            _categoryDbService = category;
            _productStateDbService = productState;
        }

        public async Task SeedApplication()
        {
            await _context.Database.MigrateAsync();

            await AddCategories();
            await AddProductStates();
            await AddProducts();
        }

        public async Task AddCategories()
        {
            if(!await _categoryDbService.AnyByName("Солодощі"))
                await _categoryDbService.Add(new Category() { Name = "Солодощі" });

            if (!await _categoryDbService.AnyByName("Іграшки"))
                await _categoryDbService.Add(new Category() { Name = "Іграшки" });

            if (!await _categoryDbService.AnyByName("Цифрові товари"))
                await _categoryDbService.Add(new Category() { Name = "Цифрові товари" });

            await _context.SaveChangesAsync();
        }

        public async Task AddProductStates()
        {
            if (!await _productStateDbService.AnyStateByName("Б/У"))
                await _productStateDbService.Add(new ProductState() { Name = "Б/У" });

            if (!await _productStateDbService.AnyStateByName("Новий"))
                await _productStateDbService.Add(new ProductState() { Name = "Новий" });

            await _context.SaveChangesAsync();
        }

        public async Task AddProducts()
        {
            var states = await _productStateDbService.GetAllProducts();
            var categories = await _categoryDbService.GetAllCategories();
            var rand = new Random();

            if (!await _productDbService.AnyProductsByTitle("Кіндер Сюрприз"))
                
                await _productDbService.Add(new Product() {
                    DateCreated = DateTime.Now,
                    Title = "Кіндер Сюрприз",
                    Image = "none",
                    Price = 25,
                    //CategoryId = categories[rand.Next(0, categories.Count())].Id,
                    //ProductStateId = states[rand.Next(0, states.Count())].Id,
                    Category = categories[rand.Next(0, categories.Count())],
                    ProductState = states[rand.Next(0, states.Count())],
                    Count = 200,
                    ByNow = true,
                    Description = "опис відсутній" 
                });
            
            if (!await _productDbService.AnyProductsByTitle("Шоколад Мілка"))
                await _productDbService.Add(new Product() {
                    DateCreated = DateTime.Now,
                    Title = "Шоколад Мілка",
                    Image = "none",
                    Price = 40,
                    //CategoryId = categories[rand.Next(0, categories.Count())].Id,
                    //ProductStateId = states[rand.Next(0, states.Count())].Id,
                    Category = categories[rand.Next(0, categories.Count())],
                    ProductState = states[rand.Next(0, states.Count())],
                    Count = 200,
                    ByNow = true,
                    Description = "з арахісовим маслом" 
                });
            
            if (!await _productDbService.AnyProductsByTitle("Морські камінці"))
                await _productDbService.Add(new Product() {
                    DateCreated = DateTime.Now,
                    Title = "Морські камінці",
                    Image = "none",
                    Price = 20,
                    //CategoryId = categories[rand.Next(0, categories.Count())].Id,
                    //ProductStateId = states[rand.Next(0, states.Count())].Id,
                    Category = categories[rand.Next(0, categories.Count())],
                    ProductState = states[rand.Next(0, states.Count())],
                    Count = 200,
                    ByNow = true,
                    Description = "ціна за 100г" 
                });
            
            if (!await _productDbService.AnyProductsByTitle("Пістолет"))
                await _productDbService.Add(new Product() {
                    DateCreated = DateTime.Now,
                    Title = "Пістолет",
                    Image = "none",
                    Price = 250,
                    //CategoryId = categories[rand.Next(0, categories.Count())].Id,
                    //ProductStateId = states[rand.Next(0, states.Count())].Id,
                    Category = categories[rand.Next(0, categories.Count())],
                    ProductState = states[rand.Next(0, states.Count())],
                    Count = 100,
                    ByNow = true,
                    Description = "револьвер, пістони" 
                });
            
            if (!await _productDbService.AnyProductsByTitle("Лазерний меч"))
                await _productDbService.Add(new Product() {
                    DateCreated = DateTime.Now,
                    Title = "Лазерний меч",
                    Image = "none",
                    Price = 800,
                    //CategoryId = categories[rand.Next(0, categories.Count())].Id,
                    //ProductStateId = states[rand.Next(0, states.Count())].Id,
                    Category = categories[rand.Next(0, categories.Count())],
                    ProductState = states[rand.Next(0, states.Count())],
                    Count = 50,
                    ByNow = true,
                    Description = "меч з зоряних війн" 
                });
            
            if (!await _productDbService.AnyProductsByTitle("Набір: маленький лікар"))
                await _productDbService.Add(new Product() {
                    DateCreated = DateTime.Now,
                    Title = "Набір: маленький лікар",
                    Image = "none",
                    Price = 500,
                    //CategoryId = categories[rand.Next(0, categories.Count())].Id,
                    //ProductStateId = states[rand.Next(0, states.Count())].Id,
                    Category = categories[rand.Next(0, categories.Count())],
                    ProductState = states[rand.Next(0, states.Count())],
                    Count = 30,
                    ByNow = true,
                    Description = "опис відсутній" 
                });
            
            if (!await _productDbService.AnyProductsByTitle("Witcher 3"))
                await _productDbService.Add(new Product() {
                    DateCreated = DateTime.Now,
                    Title = "Witcher 3",
                    Image = "none",
                    Price = 500,
                    //CategoryId = categories[rand.Next(0, categories.Count())].Id,
                    //ProductStateId = states[rand.Next(0, states.Count())].Id,
                    Category = categories[rand.Next(0, categories.Count())],
                    ProductState = states[rand.Next(0, states.Count())],
                    Count = 20,
                    ByNow = true,
                    Description = "гра witcher 3, steam" 
                });
            
            if (!await _productDbService.AnyProductsByTitle("Підписка Netflix"))
                await _productDbService.Add(new Product() {
                    DateCreated = DateTime.Now,
                    Title = "Підписка Netflix",
                    Image = "none",
                    Price = 100,
                    //CategoryId = categories[rand.Next(0, categories.Count())].Id,
                    //ProductStateId = states[rand.Next(0, states.Count())].Id,
                    Category = categories[rand.Next(0, categories.Count())],
                    ProductState = states[rand.Next(0, states.Count())],
                    Count = 50,
                    ByNow = true,
                    Description = "термін підписки: 2 місяці" 
                });
            
            if (!await _productDbService.AnyProductsByTitle("Steam картка - 25$"))
                await _productDbService.Add(new Product() {
                    DateCreated = DateTime.Now,
                    Title = "Steam картка - 25$",
                    Image = "none",
                    Price = 1000,
                    //CategoryId = categories[rand.Next(0, categories.Count())].Id,
                    //ProductStateId = states[rand.Next(0, states.Count())].Id,
                    Category = categories[rand.Next(0, categories.Count())],
                    ProductState = states[rand.Next(0, states.Count())],
                    Count = 70,
                    ByNow = true,
                    Description = "опис відсутній" 
                });

            await _context.SaveChangesAsync();
        }
    }
}

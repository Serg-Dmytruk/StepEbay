using Microsoft.EntityFrameworkCore;
using StepEbay.Data;
using StepEbay.Data.Models.Users;
using StepEbay.Data.Common.Services.BetsDbServices;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Models.Products;
using StepEbay.Data.Models.Bets;
using StepEbay.Data.Common.Services.UserDbServices;
using BC = BCrypt.Net.BCrypt;

namespace StepEbay.Admin.Api.Common.Services.DbSeeder
{
    public class Seeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductDbService _productDbService;
        private readonly IProductStateDbService _productStateDbService;
        private readonly ICategoryDbService _categoryDbService;
        private readonly IPurchaseTypeDbService _purchesTypeDbService;
        private readonly IUserDbService _userDbService;
        public Seeder(ApplicationDbContext context, IUserDbService user, IProductDbService product,
            ICategoryDbService category, IProductStateDbService productState, IPurchaseTypeDbService purchaseType)
        {
            _context = context;
            _productDbService = product;
            _categoryDbService = category;
            _productStateDbService = productState;
            _purchesTypeDbService = purchaseType;
            _userDbService = user;
        }

        public async Task SeedApplication()
        {
            await _context.Database.MigrateAsync();

            await AddUser();
            await AddPurchesType();
            await AddCategories();
            await AddProductStates();
            await AddProducts();
        }

        public async Task AddUser()
        {
            if(!await _userDbService.AnyByNickName("admin"))
            {
                var pass = BC.HashPassword("123456qQ");
                _context.Users.Add(new User()
                {
                    NickName = "admin",
                    Password = pass,
                    FullName = "Admin Admin",
                    Email = "adminmail@gmail.com",
                    Created = DateTime.UtcNow,
                    IsEmailConfirmed = true,
                });
            }

            if (!await _userDbService.AnyByNickName("user_user"))
            {
                var pass = BC.HashPassword("123456qQ");
                _context.Users.Add(new User()
                {
                    NickName = "user_user",
                    Password = pass,
                    FullName = "User User",
                    Email = "usermail@gmail.com",
                    Created = DateTime.UtcNow,
                    IsEmailConfirmed = true,
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddPurchesType()
        {
            if (!await _purchesTypeDbService.AnyByName("Продаж"))
                await _purchesTypeDbService.Add(new PurchaseType() { Type = "Продаж" });

            if (!await _purchesTypeDbService.AnyByName("Аукціон"))
                await _purchesTypeDbService.Add(new PurchaseType() { Type = "Аукціон" });

            await _context.SaveChangesAsync();
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
            var states = await _productStateDbService.GetAll();
            var categories = await _categoryDbService.GetAll();
            var purchaseTypes = await _purchesTypeDbService.GetAll();
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
                    Description = "опис відсутній" ,
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
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
                    Description = "з арахісовим маслом",
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
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
                    Description = "ціна за 100г",
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
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
                    Description = "револьвер, пістони",
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
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
                    Description = "меч з зоряних війн",
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
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
                    Description = "опис відсутній",
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
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
                    Description = "гра witcher 3, steam",
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
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
                    Description = "термін підписки: 2 місяці",
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
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
                    Description = "опис відсутній",
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
                });

            await _context.SaveChangesAsync();
        }
    }
}

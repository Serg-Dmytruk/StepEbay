using Microsoft.EntityFrameworkCore;
using StepEbay.Data;
using StepEbay.Data.Models.Users;
using StepEbay.Data.Common.Services.BetsDbServices;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Models.Products;
using StepEbay.Data.Models.Bets;
using StepEbay.Data.Common.Services.UserDbServices;
using BC = BCrypt.Net.BCrypt;
using StepEbay.Data.Common.Services.AuthDbServices;
using StepEbay.Data.Models.Auth;

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
        private readonly IRoleDbService _roleDbService;
        private readonly IUserRoleDbService _userRoleDbService;
        public Seeder(ApplicationDbContext context, IUserDbService user, IProductDbService product, IRoleDbService role,
            ICategoryDbService category, IProductStateDbService productState, IPurchaseTypeDbService purchaseType, IUserRoleDbService userRoles)
        {
            _context = context;
            _productDbService = product;
            _categoryDbService = category;
            _productStateDbService = productState;
            _purchesTypeDbService = purchaseType;
            _userDbService = user;
            _roleDbService = role;
            _userRoleDbService = userRoles;
        }

        public async Task SeedApplication()
        {
            await _context.Database.MigrateAsync();

            await AddRoles();
            await AddUsers();
            await AddPurchesType();
            await AddCategories();
            await AddProductStates();
            await AddProducts();
        }

        public async Task AddRoles()
        {
            if (!await _roleDbService.AnyByName("admin"))
                await _roleDbService.Add(new Role() { Name = "admin" });

            if (!await _roleDbService.AnyByName("manager"))
                await _roleDbService.Add(new Role() { Name = "manager" });
        }

        public async Task AddUsers()
        {
            await AddUser("admin", "123456qQ", "admin_admin", "adminmail@gmail.com", "manager");
            await AddUser("user_user", "123456qQ", "user_user", "usermail@gmail.com", "admin");
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

        private async Task AddProduct(string title, string image, decimal price, int count, bool byNow, string desc )
        {
            var states = await _productStateDbService.GetAll();
            var categories = await _categoryDbService.GetAll();
            var purchaseTypes = await _purchesTypeDbService.GetAll();
            var rand = new Random();

            if (!await _productDbService.AnyProductsByTitle(title))

                await _productDbService.Add(new Product()
                {
                    DateCreated = DateTime.Now,
                    Title = title,
                    Image = image,
                    Price = price,
                    Category = categories[rand.Next(0, categories.Count())],
                    ProductState = states[rand.Next(0, states.Count())],
                    Count = count,
                    ByNow = byNow,
                    Description = desc,
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
                });

            await _context.SaveChangesAsync();
        }

        public async Task AddProducts()
        {
            await AddProduct("Кіндер Сюрприз", "none", 25, 200, true, "опис відсутній");
            await AddProduct("Шоколад Мілка", "none", 40, 200, true, "з арахісовим маслом");
            await AddProduct("Морські камінці", "none", 20, 200, true, "ціна за 100г");
            await AddProduct("Пістолет", "none", 250, 100, true, "револьвер, пістони");
            await AddProduct("Лазерний меч", "none", 800, 50, true, "меч з зоряних війн");
            await AddProduct("Набір: маленький лікар", "none", 500, 15, true, "опис відсутній");
            await AddProduct("Witcher 3", "none", 500, 15, true, "гра witcher 3, steam");
            await AddProduct("Підписка Netflix", "none", 100, 15, true, "термін підписки: 2 місяці");
            await AddProduct("Steam картка - 25$", "none", 1000, 70, true, "опис відсутній");
        }

        private async Task AddUser(string userName, string pass, string fullName, string email, string _role)
        {
            if (!await _userDbService.AnyByNickName(userName))
            {
                var role = await _roleDbService.GetByName(_role);

                if (role is not null)
                {
                    var hashPass = BC.HashPassword(pass);
                    await _userDbService.Add(new User()
                    {
                        NickName = userName,
                        Password = hashPass,
                        FullName = fullName,
                        Email = email,
                        Created = DateTime.UtcNow,
                        IsEmailConfirmed = true,
                    });

                    var user = await _userDbService.GetUserByNickName(userName);
                    await _userRoleDbService.Add(new UserRole() { Role = role, User = user });

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}

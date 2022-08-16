using Microsoft.EntityFrameworkCore;
using StepEbay.Common.Constans;
using StepEbay.Data;
using StepEbay.Data.Common.Services.AuthDbServices;
using StepEbay.Data.Common.Services.BetsDbServices;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Data.Models.Auth;
using StepEbay.Data.Models.Bets;
using StepEbay.Data.Models.Products;
using StepEbay.Data.Models.Users;
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
        private readonly IRoleDbService _roleDbService;
        private readonly IUserRoleDbService _userRoleDbService;
        private readonly IPurchesDbService _purchesDbService;
        private readonly IPurchesStateDbService _purchesStateDbService;

        public Seeder(ApplicationDbContext context, IUserDbService user, IProductDbService product, IRoleDbService role,
            ICategoryDbService category, IProductStateDbService productState, IPurchaseTypeDbService purchaseType,
            IUserRoleDbService userRoles, IPurchesDbService purchesDbService, IPurchesStateDbService purchesStateDbService)
        {
            _context = context;
            _productDbService = product;
            _categoryDbService = category;
            _productStateDbService = productState;
            _purchesTypeDbService = purchaseType;
            _userDbService = user;
            _roleDbService = role;
            _userRoleDbService = userRoles;
            _purchesDbService = purchesDbService;
            _purchesStateDbService = purchesStateDbService;
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
            await AddPurchesState();
            await AddPurchase();
        }

        public async Task AddRoles()
        {
            if (!await _roleDbService.AnyByName(AccountRolesConstant.ADMIN))
                await _roleDbService.Add(new Role() { Name = AccountRolesConstant.ADMIN });

            if (!await _roleDbService.AnyByName(AccountRolesConstant.MANAGER))
                await _roleDbService.Add(new Role() { Name = AccountRolesConstant.MANAGER });
        }

        public async Task AddUsers()
        {
            await AddUser("admin", "123456qQ", "admin_admin", "adminmail@gmail.com", AccountRolesConstant.ADMIN);
            await AddUser("user_user", "123456qQ", "user_user", "usermail@gmail.com", AccountRolesConstant.MANAGER);
        }

        public async Task AddPurchesState()
        {
            if (!await _purchesStateDbService.AnyByState(PurchaseStatesConstant.OPEN))
                await _purchesStateDbService.Add(new PurchaseState { State = PurchaseStatesConstant.OPEN });

            if (!await _purchesStateDbService.AnyByState(PurchaseStatesConstant.CLOSE))
                await _purchesStateDbService.Add(new PurchaseState { State = PurchaseStatesConstant.CLOSE });
        }

        public async Task AddPurchase()
        {
            var user = await _userDbService.GetUserByNickName("admin");
            var product = await _context.Products.Include(x => x.PurchaseType).FirstOrDefaultAsync(x => x.PurchaseType.Type == PurchaseTypesConstant.AUCTION);

            await _purchesDbService.Add(new Purchase
            {
                PoductId = product.Id,
                UserId = user.Id,
                PurchasePrice = product.Price,
                PurchaseState = await _context.PurchaseStates.FirstOrDefaultAsync(x => x.State == "open"),              
            });

        }

        public async Task AddPurchesType()
        {
            if (!await _purchesTypeDbService.AnyByName(PurchaseTypesConstant.SALE))
                await _purchesTypeDbService.Add(new PurchaseType() { Type = PurchaseTypesConstant.SALE });

            if (!await _purchesTypeDbService.AnyByName(PurchaseTypesConstant.AUCTION))
                await _purchesTypeDbService.Add(new PurchaseType() { Type = PurchaseTypesConstant.AUCTION });

            await _context.SaveChangesAsync();
        }

        public async Task AddCategories()
        {
            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.FOOD))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.FOOD });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.FORKIDS))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.FORKIDS });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.FORGAMERS))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.FORGAMERS });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.INDUSTRIAL))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.INDUSTRIAL });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.FASHION))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.FASHION });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.HEALTHANDBUATY))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.HEALTHANDBUATY });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.HOMEANDGARDEN))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.HOMEANDGARDEN });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.SPORT))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.SPORT });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.AUTOANDPARTS))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.AUTOANDPARTS });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.COLLECTIBLE))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.COLLECTIBLE });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.ELECTRONIC))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.ELECTRONIC });

            await _context.SaveChangesAsync();
        }

        public async Task AddProductStates()
        {
            if (!await _productStateDbService.AnyStateByName(ProductStateConstant.USED))
                await _productStateDbService.Add(new ProductState() { Name = ProductStateConstant.USED });

            if (!await _productStateDbService.AnyStateByName(ProductStateConstant.NEW))
                await _productStateDbService.Add(new ProductState() { Name = ProductStateConstant.NEW });

            await _context.SaveChangesAsync();
        }

        private async Task AddProduct(string title, string image, decimal price, int count, bool byNow, string desc)
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
                    DateClose = DateTime.UtcNow.AddDays(1)
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

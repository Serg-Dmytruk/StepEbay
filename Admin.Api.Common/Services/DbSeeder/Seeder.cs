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
            await AddUser("new_user", "123456qQ", "new_user", "new_user@gmail.com", AccountRolesConstant.MANAGER);
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
            if (!await _purchesDbService.Any(product.Id))
            {
                await _purchesDbService.Add(new Purchase
                {
                    PoductId = product.Id,
                    UserId = user.Id,
                    PurchasePrice = product.Price,
                    PurchaseState = await _context.PurchaseStates.FirstOrDefaultAsync(x => x.State == "open"),
                });
            }
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
            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.TELEPHONE))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.TELEPHONE });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.CLOTH))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.CLOTH });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.SPORT))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.SPORT });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.BUATY))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.BUATY });

            if (!await _categoryDbService.AnyByName(ProductCategoryConstant.TOY))
                await _categoryDbService.Add(new Category() { Name = ProductCategoryConstant.TOY });

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

        private async Task AddProduct(string title, string image, decimal price, string desc, string category)
        {
            var states = await _productStateDbService.GetAll();
            var categorie = await _categoryDbService.GetByName(category);
            var purchaseTypes = await _purchesTypeDbService.GetAll();
            var rand = new Random();

            if (!await _productDbService.AnyProductsByTitle(title))

                await _productDbService.Add(new Product()
                {
                    DateCreated = DateTime.Now,
                    OwnerId = (await _userDbService.GetUserByNickName("admin")).Id,
                    Title = title,
                    Image = image,
                    Price = price,
                    Category = categorie,
                    ProductState = states[rand.Next(0, states.Count())],
                    Description = desc,
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
                    DateClose = DateTime.UtcNow.AddMinutes(10)
                });

            await _context.SaveChangesAsync();
        }

        public async Task AddProducts()
        {
            await AddProduct("Nokia 2310", string.Empty, 7500, "Смартфон Nokia 2310", ProductCategoryConstant.TELEPHONE);
            await AddProduct("Nokia 5310", string.Empty, 12527, "Смартфон Nokia 5310", ProductCategoryConstant.TELEPHONE);
            await AddProduct("Sony 5510", string.Empty, 15600, "Смартфон Sony 5510", ProductCategoryConstant.TELEPHONE);
            await AddProduct("Sony 8000", string.Empty, 88000, "Смартфон Sony 8000", ProductCategoryConstant.TELEPHONE);
            await AddProduct("Asus 5", string.Empty, 21000, "Смартфон Asus 5", ProductCategoryConstant.TELEPHONE);
            await AddProduct("Asus 4+", string.Empty, 16000, "Смартфон Asus 4+", ProductCategoryConstant.TELEPHONE);

            await AddProduct("Спортивна куртка", string.Empty, 600, "Спортивна куртка Nike", ProductCategoryConstant.CLOTH);
            await AddProduct("Не cпортивна куртка", string.Empty, 200, "Не спортивна куртка не Nike", ProductCategoryConstant.CLOTH);
            await AddProduct("Напів спортивна куртка", string.Empty, 400, "Напів спортивна куртка можливо Nike", ProductCategoryConstant.CLOTH);
            await AddProduct("Джинси жіночі", string.Empty, 500, "Джинси Jins", ProductCategoryConstant.CLOTH);
            await AddProduct("Футболка", string.Empty, 200, "Футболка T-shirt", ProductCategoryConstant.CLOTH);
            await AddProduct("Краватка", string.Empty, 800, "Чорна краватка", ProductCategoryConstant.CLOTH);

            await AddProduct("Баскетбольний м'яч", string.Empty, 400, "Баскетбольний м'яч Sporty", ProductCategoryConstant.SPORT);
            await AddProduct("Футбольний м'яч", string.Empty, 800, "Футбольний м'яч Sporty", ProductCategoryConstant.SPORT);
            await AddProduct("Скакалка", string.Empty, 150, "Скакалка Sporty", ProductCategoryConstant.SPORT);
            await AddProduct("Гантелі 10кг", string.Empty, 900, "Одна гантеля 10 кг", ProductCategoryConstant.SPORT);

            await AddProduct("Крем для рук", string.Empty, 130, "Крем для рук Viche", ProductCategoryConstant.BUATY);
            await AddProduct("Крем для ніг", string.Empty, 100, "Крем для ніг Viche", ProductCategoryConstant.BUATY);
            await AddProduct("Крем для голови", string.Empty, 200, "Крем для голови Viche", ProductCategoryConstant.BUATY);
            await AddProduct("Мило Viche", string.Empty, 40, "Мило Viche", ProductCategoryConstant.BUATY);
            await AddProduct("Рідке мило Viche", string.Empty, 80, "Рідке мило Viche", ProductCategoryConstant.BUATY);
            await AddProduct("Шампунь", string.Empty, 40, "Шампунь headandshoulders 200мл", ProductCategoryConstant.BUATY);
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

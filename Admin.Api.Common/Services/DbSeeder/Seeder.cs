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

        private async Task AddProduct(string title, string image, decimal price, string desc, string additionalInfo, string category, int rate)
        {
            var states = await _productStateDbService.GetAll();
            var categorie = await _categoryDbService.GetByName(category);
            var purchaseTypes = await _purchesTypeDbService.GetAll();
            var rand = new Random();

            if (!await _productDbService.AnyProductsByTitle(title))

                await _productDbService.Add(new Product()
                {
                    DateCreated = DateTime.UtcNow,
                    OwnerId = (await _userDbService.GetUserByNickName("admin")).Id,
                    Title = title,
                    Image = image,
                    Price = price,
                    Category = categorie,
                    ProductState = states[rand.Next(0, states.Count())],
                    Description = desc,
                    PurchaseType = purchaseTypes[rand.Next(0, purchaseTypes.Count())],
                    DateClose = DateTime.UtcNow.AddMinutes(10),
                    AditionalInfo = additionalInfo,
                    Rate = rate
                });

            await _context.SaveChangesAsync();
        }

        public async Task AddProducts()
        {
            var rand = new Random();

            await AddProduct("Мобільний телефон Samsung Galaxy M32 6/128 GB Light Blue (SM-M325FLBGSEK)", "samsung_galaxy_m32.png", 8555, "Екран (6.4\", Super AMOLED, 2400x1080) / MediaTek Helio G80 (2.0 ГГц + 1.8 ГГц) / основна квадрокамера: 64 Мп + 8 Мп + 2 Мп + 2 Мп, фронтальна камера: 20 Мп / RAM 6 ГБ / 128 ГБ вбудованої пам'яті + microSD (до 1 ТБ) / 3G / LTE / GPS / підтримка 2 SIM-карток (Nano-SIM) / Android 11 / 5000 мА·год", "", ProductCategoryConstant.TELEPHONE, rand.Next(3, 6));
            await AddProduct("Мобільний телефон Apple iPhone 14 128GB Starlight (MPUR3RX/A)", "284913536.jpg", 41499, "Екран (6.1\", OLED (Super Retina XDR), 2532x1170) / Apple A15 Bionic / подвійна основна камера: 12 Мп + 12 Мп, фронтальна камера: 12 Мп / 128 ГБ вбудованої пам'яті / 3G / LTE / 5G / GPS / підтримка 2 SIM-карток (eSIM) / iOS 16", "", ProductCategoryConstant.TELEPHONE, rand.Next(3, 6));
            await AddProduct("Мобільний телефон Samsung Galaxy S21 FE 6/128GB Lavender (SM-G990BLVDSEK/SM-G990BLVFSEK)", "245951562.jpg", 24999, "Екран (6.4\", Dynamic AMOLED 2X, 2340x1080) / Qualcomm Snapdragon 888 (2.84 ГГц) / потрійна основна камера: 12 Мп + 12 Мп + 8 Мп, фронтальна 32 Мп / RAM 6 ГБ / 128 ГБ вбудованої пам'яті / 3G / LTE / 5G / GPS / підтримка 2 SIM-карток (Nano-SIM) / Android 11 / 4500 мА·год", "", ProductCategoryConstant.TELEPHONE, rand.Next(3, 6));
            await AddProduct("Мобільний телефон Motorola G32 6/128GB Grey (PAUU0013RS)", "282291084.jpg", 7699, "Экран (6.5\", LCD, 2400x1080) / Qualcomm Snapdragon 680 (2.4 ГГц) / основная тройная камера: 50 Мп + 8 Мп + 2 Мп, фронтальная камера: 16 Мп / RAM 6 ГБ / 128 ГБ встроенной памяти + microSD (до 1 ТБ) / 3G / LTE / GPS / поддержка 2х SIM-карт (Nano-SIM) / Android 12 / 5000 мА*ч", "", ProductCategoryConstant.TELEPHONE, rand.Next(3, 6));
            await AddProduct("Мобільний телефон Nokia G10 3/32 GB Blue (719901148421)", "175052344.jpg", 3999, "Екран (6.5\", IPS, 1600x720) / MediaTek Helio G25 (2.0 ГГц) / потрійна основна камера: 13 Мп + 2 Мп + 2 Мп, фронтальна 8 Мп / RAM 3 ГБ / 32 ГБ вбудованої пам'яті + microSD (до 512 ГБ) / 3G / LTE / GPS / підтримка 2 SIM-карток (Nano-SIM) / Android 11 / 5050 мА·год", "", ProductCategoryConstant.TELEPHONE, rand.Next(3, 6));
            await AddProduct("Мобільний телефон Apple iPhone 11 128GB (PRODUCT) Red (MHDK3)", "152740976.jpg", 25499, "Екран (6.1\", IPS (Liquid Retina HD), 1792x828)/Apple A13 Bionic/основна подвійна камера: 12 Мп + 12 Мп, фронтальна камера: 12 Мп/RAM 4 ГБ/128 ГБ вбудованої пам'яті/3G/LTE/GPS/ГЛОНАСС/Nano-SIM/iOS 13/3046 мА·год", "", ProductCategoryConstant.TELEPHONE, rand.Next(3, 6));
            await AddProduct("Мобільний телефон Samsung Galaxy A53 5G 6/128GB Black (SM-A536EZKDSEK)", "263968716.jpg", 17299, "Екран (6.5\", Super AMOLED, 2400x1080)/ Samsung Exynos 1280 (2 x 2.4 ГГц + 6 x 2.0 ГГц)/ основна квадрокамера: 64 Мп + 12 Мп + 5 Мп + 5 Мп, фронтальна 32 Мп/ RAM 6 ГБ/ 128 ГБ вбудованої пам'яті + microSD (до 1 ТБ)/ 3G/ LTE/ 5G/ GPS/ A-GPS/ ГЛОНАСС/ BDS/ підтримка 2х SIM-карт (Nano-SIM)/ Android 12/ 5000 мА*год", "", ProductCategoryConstant.TELEPHONE, rand.Next(3, 6));
            await AddProduct("Мобільний телефон Samsung Galaxy A13 4/128GB Black (SM-A135FZKKSEK)", "263916044.jpg", 7999, "Екран (6.6\", PLS, 2408x1080) / Samsung Exynos 850 (2.0 ГГц) / основна квадрокамера: 50 Мп + 5 Мп + 2 Мп + 2 Мп, фронтальна 8 Мп / RAM 4 ГБ / 128 ГБ вбудованої пам'яті + microSD (до 1 ТБ) / 3G / LTE / GPS / підтримка 2х SIM-карт (Nano-SIM) / Android 12 / 5000 мА*год", "", ProductCategoryConstant.TELEPHONE, rand.Next(3, 6));
            await AddProduct("Мобільний телефон Samsung Galaxy M13 4/64GB Deep Green (SM-M135FZGDSEK)", "277025943.jpg", 7299, "Экран (6.6\", PLS, 2408x1080) / Samsung Exynos 850 (2.0 ГГц) / тройная основная камера: 50 Мп + 5 Мп + 2 Мп, фронтальная камера: 8 Мп / RAM 4 ГБ / 64 ГБ встроенной памяти + microSD (до 1 ТБ) / 3G / LTE / GPS / поддержка 2х SIM-карт (Nano-SIM) / Android 12 / 5000 мА*ч", "", ProductCategoryConstant.TELEPHONE, rand.Next(3, 6));
            await AddProduct("Мобільний телефон Apple iPhone 13 128GB Pink (MLPH3HU/A)", "221214151.jpg", 35999, "Екран (6.1\", OLED (Super Retina XDR), 2532x1170) / Apple A15 Bionic / подвійна основна камера: 12 Мп + 12 Мп, фронтальна камера: 12 Мп / 128 ГБ вбудованої пам'яті / 3G / LTE / 5G / GPS / Nano-SIM, eSIM / iOS 15", "", ProductCategoryConstant.TELEPHONE, rand.Next(3, 6));
            await AddProduct("Мобільний телефон Xiaomi Redmi 10 2022 4/128GB Pebble White", "220464250.jpg", 7999, "Екран (6.5\", 2400x1080) / Mediatek Helio G88 (2.0 ГГц + 1.8 ГГц) / основна квадрокамера: 50 Мп + 8 Мп + 2 Мп + 2 Мп, фронтальна 8 Мп / RAM 4 ГБ / 128 ГБ вбудованої пам'яті + microSD (до 512 ГБ) / 3G / LTE / GPS / підтримка 2 SIM-карток (Nano-SIM) / Android 11 / 5000 мА·год", "", ProductCategoryConstant.TELEPHONE, rand.Next(3, 6));

            await AddProduct("Куртка Puma Ferrari Race Mt7 Ecolit Jkt 53582502 S Rosso Corsa (4064537821299)", "288922454.jpg", 6290, "Розмір\r\nS\r\nОсобливості моделі\r\nЗ капюшоном\r\nСтиль\r\nСпортивний\r\nКолір\r\nЧервоний\r\nМатеріал верху\r\nПоліестер\r\nДовжина\r\nКороткий\r\nСклад\r\n100% полиэстер\r\nКраїна-виробник товару\r\nВ'єтнам\r\nКраїна реєстрації бренду\r\nНімеччина", "", ProductCategoryConstant.CLOTH, rand.Next(3, 6));
            await AddProduct("Світшот Guess M2YQ27-KBA10-JBLK S Jet Black A996 (7621826345323)", "283267218.jpg", 2790, "Сезон\r\nВесняний\r\nОсінній\r\nРозмір\r\nS\r\nСтиль\r\nПовсякденний (casual)\r\nКолір\r\nЧорний\r\nМатеріал\r\nБавовна\r\nПоліестер\r\nОсобливості крою\r\nВільні\r\nСклад\r\n74% хлопок, 26% полиэстер\r\nПринт\r\nНапис\r\nКраїна-виробник товару\r\nТуреччина\r\nКраїна реєстрації бренду\r\nСША", "", ProductCategoryConstant.CLOTH, rand.Next(3, 6));
            await AddProduct("Джинси Prodigy 32158-2 40 Темно-сині (2000002473336)", "289449186.jpg", 1182, "Посадка\r\nВисока\r\nМоделі\r\nRegular Fit\r\nРозмір\r\n40\r\nОсобливості моделі\r\nЗ кишенями\r\nКолір\r\nТемно-синій\r\nОсобливості крою\r\nШирокі\r\nЗастібка\r\nЗмійка\r\nСклад\r\n94.5% хлопок, 4% полиэстер, 1.5% эластан\r\nКраїна-виробник товару\r\nТуреччина\r\nКраїна реєстрації бренду\r\nТуреччина", "", ProductCategoryConstant.CLOTH, rand.Next(3, 6));
            await AddProduct("Куртка дитяча Minoti 11COAT 1 37364JNR 110-116 см Темно-синя (5059030938512)", "287682045.jpg", 1529, "Дитячий вік\r\n5 років\r\n6 років\r\nЗріст\r\n110 см\r\n116 см\r\nСезон\r\nВесняний\r\nДемісезонний\r\nЗимовий\r\nОсінній\r\nКолір\r\nСиній\r\nОсобливості моделі\r\nЗ капюшоном\r\nПринт\r\nОднотонний\r\nМатеріал\r\nНейлон\r\nСклад\r\n100% нейлон\r\nКраїна-виробник товару\r\nМ'янма\r\nКраїна реєстрації бренду\r\nВеликобританія", "", ProductCategoryConstant.CLOTH, rand.Next(3, 6));
            await AddProduct("Спортивний костюм Puma Loungewear Suit 67002579 XS Blue Wash (4065449065030)", "288767404.jpg", 3190, "Призначення\r\nДля повсякденного носіння\r\nДля фітнесу\r\nРозмір\r\nXS\r\nСезон\r\nДемісезонний\r\nКолір\r\nБлакитний\r\nТип костюму\r\nЗ кофтою\r\nВид костюму\r\nДвійка\r\nСклад\r\n66% хлопок, 34% полиэстер\r\nМатеріал\r\nБавовна\r\nПоліестер\r\nДодаткові характеристики\r\nДовжина штанів - 95,5 см, внутрішній шов - 70 см.\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренду\r\nНімеччина", "", ProductCategoryConstant.CLOTH, rand.Next(3, 6));
            await AddProduct("Парка Braggart 58555 54 (XXL) Сіра з синім (br2000001532959)", "275449321.jpg", 3927, "Сезон\r\nЗимовий\r\nРозмір\r\n2XL\r\nМатеріал наповнювача\r\nThinsulate\r\nОсобливості моделі\r\nВодостійка\r\nЗ капюшоном\r\nЗ кишенями\r\nЗ хутром\r\nНа змійці\r\nТемпературний режим\r\nДля низьких температур\r\nСтиль\r\nПовсякденний (casual)\r\nКолір\r\nСиній\r\nМатеріал верху\r\nНейлон\r\nДовжина\r\nСтандартна\r\nМатеріал підкладки\r\nНейлон\r\nСклад\r\nВерх: з вітро- та водозахисним покриттям (100% нейлон)\r\nПідкладка: 100% нейлон\r\nНаповнювач: 100% тинсулейт\r\nОсобливості крою\r\nВільні\r\nПринт\r\nОднотонний\r\nДодаткові характеристики\r\nОсобливості: до -22°C ( характеристика дійсна під час руху від 4 км/год та швидкості вітру не більше 3 м/с).\r\n\r\nРекомендації щодо догляду: ручне прання при 30 градусах з використовуванням рідкого миючого засобу, без засобів відбілювання. Сушіння в горизонтальному стані, не використовувати автоматичний віджим. Невелике забруднення можно протерти вологою тканиною.\r\nТемпературний режим\r\nВід -10°C до -25°C\r\nКраїна-виробник товару\r\nНімеччина\r\nКраїна реєстрації бренду\r\nНімеччина", "", ProductCategoryConstant.CLOTH, rand.Next(3, 6));
            await AddProduct("Парка зимова Lenne Maya 22330/124 110 см Рожева (4741593132193)", "296367309.jpg", 4396, "Дитячий вік\r\n5 років\r\nЗріст\r\n110 см\r\nВид\r\nПарки\r\nСезон\r\nЗимовий\r\nКолір\r\nРожевий\r\nОсобливості моделі\r\nЗ капюшоном\r\nЗ хутром\r\nМатеріал\r\nПоліамід\r\nСклад\r\nМатериал верха: 100% полиамид Асtive Plus 10000/10000\r\nПідкладка: 100% полиэстер\r\nКраїна-виробник товару\r\nЕстонія\r\nКраїна реєстрації бренду\r\nЕстонія", "", ProductCategoryConstant.CLOTH, rand.Next(3, 6));
            await AddProduct("Штани Koton 3WAK40127UW-999 42 Black (8683352733691)", "284273775.jpg", 1199, "Моделі\r\nSlim Fit\r\nРозмір\r\n42\r\nСтиль\r\nДіловий\r\nПовсякденний (casual)\r\nКолір\r\nЧорний\r\nПосадка\r\nВисока\r\nОсобливості моделі\r\nНа змійці\r\nУкорочені\r\nМатеріал\r\nБавовна\r\nЕластан\r\nПоліестер\r\nСклад\r\n52% хлопок, 45% полиэстер, 3% эластан\r\nПринт\r\nОднотонний\r\nКраїна-виробник товару\r\nТуреччина\r\nКраїна реєстрації бренду\r\nТуреччина", "", ProductCategoryConstant.CLOTH, rand.Next(3, 6));
            await AddProduct("Пуховик Nike W Nsw Tf City Jkt DH4079-100 S (195242512784)", "288046386.jpg", 9969, "Розмір\r\nS\r\nСезон\r\nДемісезонний\r\nЗимовий\r\nОсінній\r\nОсобливості моделі\r\nБез капюшона\r\nЗ кишенями\r\nНа змійці\r\nСтиль\r\nПовсякденний (casual)\r\nСпортивний\r\nМатеріал верху\r\nПоліестер\r\nКолір\r\nБілий\r\nТемпературний режим\r\nДля низьких температур\r\nДовжина\r\nКоротка\r\nДовжина рукава\r\nЗ довгими рукавами\r\nОсобливості крою\r\nОверсайз\r\nВид\r\nКуртка-пуховик\r\nМатеріал підкладки\r\nПоліестер\r\nСклад\r\nМатериал верха: 100% полиэстер\r\nМатериал подкладки: 100% полиэстер\r\nМатериал наполнителя: 100% утиный пух\r\nКраїна-виробник товару\r\nВ'єтнам\r\nКраїна реєстрації бренду\r\nСША\r\nМатеріал наповнювача\r\nПух", "", ProductCategoryConstant.CLOTH, rand.Next(3, 6));
            await AddProduct("Джемпер Mango 37085816-37 XS (8445661928386)", "285940120.jpg", 999, "Розмір\r\nXS\r\nКолір\r\nХакі\r\nМатеріал\r\nПоліамід\r\nПоліестер\r\nСклад\r\n78% полиэстер, 22% полиамид\r\nПринт\r\nОднотонний\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренду\r\nІспанія", "", ProductCategoryConstant.CLOTH, rand.Next(3, 6));
            await AddProduct("Піжама (сорочка + штани) Key MNS 429 B22 L mix принт (5904765648772)", "284659495.jpg", 2180, "Сезон\r\nЗимовий\r\nТип комплекту\r\nШтани+кофта\r\nКолір\r\nСиній\r\nРозмір\r\nL\r\nСклад\r\n100% хлопок\r\nПринт\r\nКлітинка\r\nМатеріал\r\nБавовна\r\nДодаткові характеристики\r\nРекомендации по уходу: стирать в холодной воде\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренду\r\nПольща", "", ProductCategoryConstant.CLOTH, rand.Next(3, 6));

            await AddProduct("Стіл для настільного тенісу Donic Outdoor Roller 1000 Антрацит (230291-A)", "177844156.jpg", 28990, "Вид\r\nСтоли\r\nКлас\r\nАматорські\r\nЗастосування\r\nВсепогодний стіл\r\nКонструкція\r\nСкладана\r\nКолір поверхні стола\r\nАнтрацит\r\nМатеріал стільниці\r\nМеламін\r\nОсобливості\r\nБічна крайка\r\nКонтейнер для ракеток\r\nНаявність коліщаток\r\nСітка у комплекті\r\nМатеріал сітки\r\nНейлон\r\nТовщина стільниці\r\n6 мм\r\nРозмір (ДхШхВ)\r\n274 x 152.5 x 76 см\r\nВага\r\n74 кг\r\nКомплект постачання\r\nСтіл, нейлонова сітка, кріплення\r\nДодаткові характеристики\r\nОфіційні розміри Федерації ITTF\r\nВідповідає європейським нормам EN-14468\r\nПрофіль опори 25 х 25 мм\r\nКраїна-виробник товару\r\nНімеччина\r\nГабарити упаковки\r\n158 x 135 x 142 см\r\nКраїна реєстрації бренду\r\nНімеччина\r\nГарантія\r\n24 місяці", "", ProductCategoryConstant.SPORT, rand.Next(3, 6));
            await AddProduct("Сітка для тенісу Sanqiang Average з кріпленням (СН0307)", "11166702.jpg", 517, "Вид\r\nСітки\r\nКлас\r\nАматорські\r\nЗастосування\r\nВсепогодний стіл\r\nМатеріал сітки\r\nНейлон\r\nРозмір (ДхШхВ)\r\n183 х 15.25 см\r\nВага\r\n0.35 кг\r\nКомплект постачання\r\nСітка, 2 гвинтові кріплення\r\nТип кріплення сітки\r\nГвинтове кріплення\r\nРегульований натяг сітки\r\nЄ\r\nКолір сітки\r\nСиня\r\nКраїна-виробник товару\r\nКитай\r\nГабарити упаковки\r\n37 х 17.5 х 5 см\r\nКраїна реєстрації бренду\r\nКитай\r\nГарантія\r\n14 днів", "", ProductCategoryConstant.SPORT, rand.Next(3, 6));
            await AddProduct("Ракетка для настільного тенісу Donic Appelgren 300 (703003)", "240451368.jpg", 299, "Клас\r\n** Для початківців\r\nТип\r\nРакетки\r\nСтиль гри\r\nДля захисного стилю\r\nНаявність сертифіката ITTF\r\nНi\r\nФорма ручки\r\nАнатомічна\r\nШвидкість за 100-бальною шкалою\r\n30\r\nОбертання за 100-бальною шкалою\r\n40\r\nКонтроль за 100-бальною шкалою\r\n90\r\nТовщина накладки\r\n1 мм\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренду\r\nНімеччина\r\nГарантія\r\n1 місяць", "", ProductCategoryConstant.SPORT, rand.Next(3, 6));
            await AddProduct("Стіл для настільного тенісу Donic Outdoor Roller 800-5 (230296)", "10598954.jpg", 25550, "Вид\r\nСтоли\r\nКлас\r\nАматорські\r\nЗастосування\r\nВсепогодний стіл\r\nКонструкція\r\nСкладана\r\nКолір поверхні стола\r\nСиній\r\nМатеріал стільниці\r\nМеламін\r\nОсобливості\r\nНаявність сертифіката ITTF 1\r\nТовщина стільниці\r\n5 мм\r\nРозмір (ДхШхВ)\r\n274 x 152, 5 x 76 см\r\nВага\r\n60 кг\r\nКомплект постачання\r\nСтіл, нейлонова сітка, кріплення\r\nКраїна-виробник товару\r\nНімеччина\r\nКраїна реєстрації бренду\r\nНімеччина\r\nГарантія\r\n24 місяці", "", ProductCategoryConstant.SPORT, rand.Next(3, 6));
            await AddProduct("Стіл для настільного тенісу Donic Indoor Roller 900 Синій (230289-B)", "13551642.jpg", 19990, "Вид\r\nСтоли\r\nКлас\r\nАматорські\r\nЗастосування\r\nДля закритих приміщень\r\nКолір поверхні стола\r\nСиній\r\nМатеріал стільниці\r\nДСП\r\nОсобливості\r\nНаявність коліщаток\r\nМатеріал сітки\r\nНейлон\r\nТовщина стільниці\r\n19 мм\r\nРозмір (ДхШхВ)\r\n274 x 152.5 x 76 см\r\nВага\r\n76 кг\r\nКомплект постачання\r\nСтіл, нейлонова сітка, кріплення\r\nКраїна-виробник товару\r\nНімеччина\r\nКраїна реєстрації бренду\r\nНімеччина\r\nГарантія\r\n6 місяців", "", ProductCategoryConstant.SPORT, rand.Next(3, 6));
            await AddProduct("Ракетка для настільного тенісу Atemi 1000A (10050)", "10591751.jpg", 1435, "Клас\r\n***** Для професіоналів\r\nТип\r\nРакетки\r\nСтиль гри\r\nУніверсальна\r\nНаявність сертифіката ITTF\r\nТак\r\nШвидкість за 100-бальною шкалою\r\n93\r\nОбертання за 100-бальною шкалою\r\n92\r\nКонтроль за 100-бальною шкалою\r\n90\r\nТовщина накладки\r\n2 мм\r\nКраїна-виробник товару\r\nЕстонія\r\nКраїна реєстрації бренду\r\nЕстонія\r\nГарантія\r\n14 днів\r\nМатеріал\r\nДерево", "", ProductCategoryConstant.SPORT, rand.Next(3, 6));
            await AddProduct("М'яч футбольний Newt Rnx Champion League №5 NE-F-FLB (NE-F-FLB)", "178594660.jpg", 594, "Розмір\r\n№5\r\nВид\r\nФутбольні\r\nВага\r\n450 г\r\nТип\r\nМ'яч\r\nШов\r\nРучний шов\r\nМатеріал\r\nБутил\r\nЛатекс\r\nСинтетична шкіра\r\nТекстиль\r\nКраїна-виробник товару\r\nПакистан\r\nГабарити упаковки\r\n30 х 10 х 10 см\r\nКраїна реєстрації бренду\r\nУкраїна\r\nГарантія\r\n14 днів", "", ProductCategoryConstant.SPORT, rand.Next(3, 6));
            await AddProduct("Дартс класичний YiWu 45 см (52AN)", "217018663.jpg", 882, "Тип\r\nДартс\r\nВид\r\nКласичний\r\nМатеріали\r\nФлокований папір; дротики: сталь/латунь\r\nДодаткові характеристики\r\nЗ металевим кільцем з одного боку\r\nТримач для підвішування циферблата зверху\r\nПринт спереду для стандартної гри у дартс\r\nПринт ззаду для гри 1-9/9-1\r\nтовщина: 1.9 см\r\nДіаметр, см\r\n45\r\nПризначення\r\nПрофесійний\r\nОсобливості\r\nДвосторонній\r\nКомплектація\r\nДартс, 6 дротиків\r\nКолір\r\nMulticolour\r\nКраїна-виробник товару\r\nКитай\r\nГабарити упаковки\r\n45.3 x 45.8 x 2.5 см\r\nКраїна реєстрації бренду\r\nКитай\r\nГарантія\r\n14 днів", "", ProductCategoryConstant.SPORT, rand.Next(3, 6));
            await AddProduct("Свисток суддівський Champion 5 шт", "219110202.jpg", 150, "Тип\r\nСвистки\r\nМатеріал\r\nМетал\r\nКолір\r\nХром\r\nДодаткові характеристики\r\nНеіржавка сталь\r\nВага: 0.02 г\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренду\r\nУкраїна\r\nГарантія\r\n12 місяців", "", ProductCategoryConstant.SPORT, rand.Next(3, 6));
            await AddProduct("Набір для настільного тенісу Newt Vitor NE-VT-5 (2 ракетки, 3 кульки)", "255726324.jpg", 236, "Клас\r\n* Для аматорів\r\nТип\r\nНабір\r\nСтиль гри\r\nУніверсальна\r\nНаявність сертифіката ITTF\r\nНi\r\nФорма ручки\r\nКонічна\r\nШвидкість за 100-бальною шкалою\r\n30\r\nТип основи\r\nOFF\r\nТип накладки\r\nКороткі шипи\r\nОбертання за 100-бальною шкалою\r\n30\r\nКонтроль за 100-бальною шкалою\r\n80\r\nТовщина накладки\r\n0.7 мм\r\nКомплектація\r\n2 ракетки, 3 м'ячі, чохол\r\nДодаткові характеристики\r\n\\\r\nКраїна-виробник товару\r\nКитай\r\nГабарити упаковки\r\n30 х 17 х 5 см\r\nКраїна реєстрації бренду\r\nУкраїна\r\nГарантія\r\n1 місяць\r\nМатеріал\r\nДерево", "", ProductCategoryConstant.SPORT, rand.Next(3, 6));
            await AddProduct("М'ячі для великого тенісу HEAD Championship 3B", "198789915.jpg", 249, "Кількість в упаковці, шт.\r\n3\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренду\r\nАвстрія\r\nГарантія\r\n1 місяць", "", ProductCategoryConstant.SPORT, rand.Next(3, 6));

            await AddProduct("Туш для вій Maybelline New York Lash Sensational Sky High Cosmic Black Космічно-чорна 7.2 мл", "289395070.jpg", 250, "Вид\r\nОб'ємна\r\nКількість у наборі\r\n1\r\nКолір\r\nЧорний\r\nКраїна виробник\r\nІталія\r\nСклад\r\nAqua / water / eau, propylene glycol, styrene/acrylates/ammonium methacrylate copolymer, polyurethane-35, cera alba / beeswax / cire dabeille, synthetic fluorphlogopite, glyceryl stearate, cetyl alcohol, peg-200 glyceryl stearate, ethylenediamine/stearyl dimer dilinoleate copolymer, copernicia cerifera cera / carnauba wax / cire de carnauba, stearic acid, palmitic acid, ethylene/va copolymer, alcohol denat., paraffin, aminomethyl propanediol, phenoxyethanol, caprylyl glycol, glycerin, hydroxyethylcellulose, butylene glycol, rayon, xanthan gum, caprylic/capric triglyceride, sodium laureth sulfate, disodium edta, myristic acid, tetrasodium edta, pentaerythrityl tetra-di-t-butyl hydroxyhydrocinnamate, potassium sorbate, silica, soluble collagen, bambusa vulgaris extract, trisodium edta [+/- may contain / peut contenir ci 77491, ci 77492, ci 77499 / iron oxides, ci 77007 / ultramarines, mica, ci 77891 / titanium dioxide, ci 75470 / carmine, ci 77288 / chromium oxide greens, ci 77742 / manganese violet, ci 77510 / ferric ferrocyanide f.I.L. D250527/2\r\nКраїна реєстрації бренду\r\nСША\r\nКлас косметики\r\nМас-маркет\r\nКатегорія\r\nДля жінок\r\nСерія\r\nLash sensational\r\nОб'єм\r\n7.2 мл", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));
            await AddProduct("Палетка тіней для повік NYX Professional Makeup Ultimate Utopia 40 г", "139288346.jpg", 999, "Фініш\r\nШимерна\r\nКількість відтінків\r\n40\r\nКількість у наборі\r\n1\r\nФормат\r\nПалетка\r\nКолір\r\nНюдовий\r\nКраїна виробник\r\nКитай\r\nКраїна реєстрації бренду\r\nСША\r\nКлас косметики\r\nПрофесійна\r\nКатегорія\r\nДля жінок\r\nСерія\r\nProfessional\r\nВага\r\n40 г\r\nДодаткові характеристики\r\nдля нанесення тіней використовуйте плоский пензель. Для розтушовування вибирайте круглий або скошений пензель із довшим ворсом. Для стійкішого макіяжу використовуйте разом з основою.", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));
            await AddProduct("Набір пензлів для макіяжу Supretto 8 шт. Білий", "119989822.jpg", 299, "Вид\r\nНабір пензлів для макіяжу\r\nКількість у наборі\r\n8\r\nКраїна виробник\r\nКитай\r\nКраїна реєстрації бренду\r\nУкраїна\r\nГарантія\r\n14 днів", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));
            await AddProduct("Туш для вій Pupa Vamp! Explosive Lashes Mascara 110 Explosive Black 12 мл", "194955894.jpg", 287, "Вид\r\nОб'ємна\r\nКількість відтінків\r\n1\r\nКількість у наборі\r\n1\r\nКолір\r\nЧорний\r\nКраїна виробник\r\nІталія\r\nСклад\r\nУнікальний мікс з рисового воску, воску Акації і полімерів робить вії щільними та дарує їм дивовижний об'єм до самих кінчиків. Желювальні речовини і плівкоутворювальні агенти забезпечують еластичне покриття для бездоганного та стійкого результату. Спеціальні пігменти гарантують насичений колір з одного нанесення.\r\nКраїна реєстрації бренду\r\nІталія\r\nКлас косметики\r\nМідл-маркет\r\nОб'єм\r\n12 мл", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));
            await AddProduct("Водостійка туш для вій Maybelline New York Lash Sensational Sky High чорна 6 мл", "176255970.jpg", 250, "Вид\r\nПодовжувальна\r\nОсобливості\r\nВодостійка\r\nЩітка\r\nКонсусна\r\nКількість відтінків\r\n1\r\nКількість у наборі\r\n1\r\nКолір\r\nЧорний\r\nКраїна виробник\r\nІталія\r\nСклад\r\nIsododecane cera alba / beeswax copernicia cerifera cera / carnauba wax disteardimonium hectorite aqua / water alcohol denat. Allyl stearate/va copolymer oryza sativa cera / rice bran wax paraffin polyvinyl laurate vp/eicosene copolymer propylene carbonate talc synthetic beeswax ethylenediamine/stearyl dimer dilinoleate copolymer peg-30 glyceryl stearate rayon hydrogenated jojoba oil caprylic/capric triglyceride silica pentaerythrityl tetra-di-t-butyl hydroxyhydrocinnamate bambusa vulgaris extract bht [+/- may contain ci 77491, ci 77492, ci 77499 / iron oxides ci 77007 / ultramarines mica ci 77891 / titanium dioxide ci 75470 / carmine ci 77288 / chromium oxide greens ci 77742 / manganese violet ci 77510 / ferric ferrocyanide ]\r\nКраїна реєстрації бренду\r\nСША\r\nКлас косметики\r\nМас-маркет\r\nОб'єм\r\n6 мл", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));
            await AddProduct("Пензлик для макіяжу двосторонній Professional Titania 2915", "61985297.png", 509, "Країна виробник\r\nНімеччина\r\nКраїна реєстрації бренду\r\nНімеччина", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));
            await AddProduct("Гель 3в1 Nivea Men Fresh для обличчя та рук з вітаміном Е 75 мл", "282783397.jpg", 119, "Призначення\r\nЗволожувальне\r\nПоживне\r\nВид\r\nГель\r\nВік\r\nВід 18\r\nЧас застосування\r\nУніверсально\r\nКількість у наборі\r\n1\r\nСфера застосування\r\nОбличчя\r\nКраїна виробник\r\nНімеччина\r\nТип шкіри\r\nДля всіх типів\r\nСклад\r\nAqua,Alcohol Denat.,Glycerin,PEG-8,Menthol,Mentha Aquatica Extract,Distarch Phosphate,Carbomer,PEG-40 Hydrogenated Castor Oil,Acrylates/C10-30 Alkyl Acrylate Crosspolymer,Sodium Hydroxide,Sodium Sulfate,Phenoxyethanol,Linalool,Limonene,Citronellol,Alpha-Isomethyl Ionone,Parfum,CI 42090\r\nСпосіб застосування\r\nНанести невелику кількість гелю на вологу шкіру обличчя і рук.\r\nКраїна реєстрації бренду\r\nНімеччина\r\nКлас косметики\r\nМас-маркет\r\nКатегорія\r\nДля чоловіків\r\nОб'єм\r\n75 мл", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));
            await AddProduct("Лімітовані гідрогелеві патчі під очі Mermade Love U 60 шт", "280282509.jpg", 239, "Призначення\r\nПроти темних кіл\r\nВид\r\nПатчі\r\nТип\r\nГідрогелеві\r\nКількість у наборі\r\n60\r\nСфера застосування\r\nЗона навколо очей\r\nКраїна виробник\r\nУкраїна\r\nТип шкіри\r\nДля всіх типів\r\nСклад\r\nAqua/Water, Glycerin, Chondrus crispus, Gellan gum,Lavandula Angustifolia (Lavender) Flower Extract, Chamomilla Recutita (Matricaria) Flower Extract, Hydroxyethylcellulose, Hyaluronic acid, D-Panthenol,Hydrolyzed Collagen, Betaine, Xanthan gum, Butylene Glycol, Argania Spinosa Kernel Oil, Ethylhexylglycerin, Phenoxyethanol, Allantoin, Parfum/​Fragrance, Mica, Potassium chloride\r\nЕкотренди\r\nНе тестувалася на тваринах\r\nКраїна реєстрації бренду\r\nУкраїна\r\nКлас косметики\r\nМас-маркет", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));
            await AddProduct("Мінеральна вода-спрей Моршинська 150 мл (4820017001434)", "12100496.jpg", 115, "Призначення\r\nЗволожувальне\r\nВид\r\nМінеральна вода\r\nКількість у наборі\r\n1\r\nКраїна виробник\r\nЧехія\r\nТип шкіри\r\nДля всіх типів\r\nСклад\r\nПриродна мінеральна вода Моршинська, азот, пропіленгліколь, етилгексилгліцерин, октенідин hcl, токоферол.\r\nСпосіб застосування\r\nРозпиліть необхідну кількість мінеральної води-спрею на обличчя, шию або тіло. За потреби ніжно промокніть залишки серветкою. Можна наносити на макіяж. Підходить для щоденного використання.\r\nДія\r\nЗволожує\r\nТонізує\r\nКраїна реєстрації бренду\r\nУкраїна\r\nКлас косметики\r\nМас-маркет\r\nОб'єм\r\n150 мл", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));
            await AddProduct("Міцелярна вода для зняття водостійкого макіяжу Nivea Make Up Expert 400 мл", "282762038.jpg", 122, "Призначення\r\nДля зняття макіяжу\r\nОчищуюче\r\nКількість у наборі\r\n1\r\nВид\r\nВода\r\nКраїна виробник\r\nНімеччина\r\nЧас застосування\r\nУніверсально\r\nТип шкіри\r\nДля всіх типів\r\nДодаткові характеристики\r\nНе містить: Парфуми\r\nСклад\r\nAqua, Isododecane, Glycerin, C15-19 Alkane, Camellia Sinensis Leaf Extract, Vaccinium Myrtillus Fruit Extract, Isopropyl Palmitate, Caprylyl/Capryl Glucoside, Coco-Caprylate/ Caprate, Sodium Chloride, Trisodium EDTA, Sodium Hydroxide, Benzethonium Chloride, Phenoxyethanol, CI 60725, CI 61565\r\nСпосіб застосування\r\nСтрусити, намочити спонж міцелярною водою й акуратно протерти ним обличчя й зону навколо очей.\r\nКраїна реєстрації бренду\r\nНімеччина\r\nКлас косметики\r\nМас-маркет\r\nСерія\r\nMake up Expert\r\nОб'єм\r\n400 мл", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));
            await AddProduct("Денний крем Nivea Cellular Expert Lift SPF 30 50 мл", "282986491.jpg", 349, "Призначення\r\nАнтивіковий\r\nЗахисне\r\nВид\r\nКрем\r\nВік\r\nВід 18\r\nЧас застосування\r\nДень\r\nКількість у наборі\r\n1\r\nСфера застосування\r\nОбличчя\r\nКраїна виробник\r\nПольща\r\nТип шкіри\r\nДля всіх типів\r\nДодаткові характеристики\r\nМістить: Активатор колагену, гіалуронова кислота\r\nСклад\r\nCI 47005,CI 15985,Parfum,Limonene,Benzyl Alcohol,Phenoxyethanol,Sodium Hydroxide,Trisodium EDTA,Acrylates/C10-30 Alkyl Acrylate Crosspolymer,Ammonium Acryloyldimethyltaurate/VP Copolymer,Xanthan Gum,Caprylyl Glycol,Ethylhexylglycerin,Dimethicone,Sodium Cetearyl Sulfate,1-Methylhydantoin-2-Imide,Creatine,Sodium Hyaluronate,Glyceryl Stearate,Cocoglycerides,Phenylbenzimidazole Sulfonic Acid,Tapioca Starch,Methylpropanediol,Cetearyl Alcohol,Octocrylene,Ethylhexyl Salicylate,Butyl Methoxydibenzoylmethane,Homosalate,Glycerin,Aqua;\r\nСпосіб застосування\r\nНанесіть крем на очищену шкіру обличчя легкими масажними круговими рухами.\r\nКраїна реєстрації бренду\r\nНімеччина\r\nКлас косметики\r\nМас-маркет\r\nКатегорія\r\nДля жінок\r\nСерія\r\nHyaluron\r\nОб'єм\r\n50 мл", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));
            await AddProduct("Відновлювальний крем CeraVe для всіх типів шкіри навколо очей 14 мл", "172022887.jpg", 269, "Призначення\r\nВідновлююче\r\nЗволожувальне\r\nВид\r\nКрем\r\nВік\r\nВід 18\r\nЧас застосування\r\nУніверсально\r\nКількість у наборі\r\n1\r\nІнгредієнти\r\nГіалуронова кислота\r\nСфера застосування\r\nЗона навколо очей\r\nКраїна виробник\r\nФранція\r\nТип шкіри\r\nСуха\r\nЧутлива\r\nСклад\r\nAqua / Water, Niacinamide, Cetyl Alcohol, Caprylic/Capric Triglyceride, Glycerin, Propanediol, Isononyl Isononanoate, Jojoba Esters, Peg-20 Methyl Glucose Sesquistearate, Cetearyl Alcohol, Dimethicone, Methyl Glucose Sesquistearate, Phenoxyethanol, Sorbitol, Behentrimonium Methosulfate, Carbomer, Laureth-4, Triethanolamine, Tocopherol, Butylene Glycol, Prunus Amygdalus Dulcis Oil / Sweet Almond Oil, Hydrogenated Vegetable Oil, Ethylhexylglycerin, Tetrasodium Edta, Sodium Lauroyl Lactylate, Sodium Hydroxide, Zinc Citrate, Asparagopsis Armata Extract, Equisetum Arvense Extract, Maltodextrin, Aloe Barbadensis Leaf Extract, Ascophyllum Nodosum Extract, Chrysanthellum Indicum Extract, Ceramide Np, Ceramide Ap, Phytosphingosine, Cholesterol, Xanthan Gum, Potassium Sorbate, Sodium Hyaluronate, Ceramide Eop.\r\nСпосіб застосування\r\nНанести невелику кількість крему легкими масажними рухами на зону навколо очей. Уникати потрапляння в очі. У разі потрапляння в очі негайно промийте їх водою.\r\nДія\r\nОсвіжає\r\nОсвітлює\r\nПриховує ознаки втоми\r\nКраїна реєстрації бренду\r\nСША\r\nКлас косметики\r\nДерматокосметика\r\nКатегорія\r\nДля жінок\r\nДля чоловіків\r\nОб'єм\r\n14 мл", "", ProductCategoryConstant.BUATY, rand.Next(3, 6));

            await AddProduct("Конструктор LEGO Classic Кубики та будинки 270 деталей ", "192258594.jpg", 679, "Серія\r\nClassic\r\nМатеріал\r\nПластик\r\nВид\r\nКонструктори\r\nСтать дитини\r\nДівчинка\r\nХлопчик\r\nКількість деталей\r\n270\r\nТема набору\r\nАбстракція\r\nКонструкції\r\nВік\r\n10 років\r\n11 років\r\n12 років\r\n13 років\r\n14 років\r\n15 років\r\n4 роки\r\n5 років\r\n6 років\r\n7 років\r\n8 років\r\n9 років\r\nГабарити упаковки\r\n26.2 х 19.1 х 7.2 см\r\nКолір\r\nРізнобарвний\r\nКраїна-виробник товару\r\nУгорщина\r\nКраїна реєстрації бренду\r\nДанія", "", ProductCategoryConstant.TOY, rand.Next(3, 6));
            await AddProduct("Конструктор LEGO Ninjago Рейдер джунглів 127 деталей ", "191557968.jpg", 349, "Серія\r\nNinjago\r\nМатеріал\r\nПластик\r\nВид\r\nКонструктори\r\nСтать дитини\r\nДівчинка\r\nХлопчик\r\nКількість деталей\r\n127\r\nТема набору\r\nНіндзя\r\nВік\r\n10 років\r\n11 років\r\n12 років\r\n13 років\r\n14 років\r\n15 років\r\n7 років\r\n8 років\r\n9 років\r\nГабарити упаковки\r\n15.7 х 14.1 х 6.1 см\r\nКолір\r\nРізнобарвний\r\nКраїна-виробник товару\r\nЧехія\r\nКраїна реєстрації бренду\r\nДанія", "", ProductCategoryConstant.TOY, rand.Next(3, 6));
            await AddProduct("Конструктор LEGO DUPLO My First Пожежний вертоліт і поліцейська машина 14 деталей", "192130087.jpg", 299, "Серія\r\nDUPLO\r\nМатеріал\r\nПластик\r\nВид\r\nКонструктори\r\nСтать дитини\r\nДівчинка\r\nХлопчик\r\nКількість деталей\r\n14\r\nТема набору\r\nВертольоти\r\nПожежні\r\nВік\r\n1 рік\r\n2 роки\r\n3 роки\r\n4 роки\r\n5 років\r\nКраїна-виробник товару\r\nУгорщина\r\nКраїна реєстрації бренду\r\nДанія", "", ProductCategoryConstant.TOY, rand.Next(3, 6));
            await AddProduct("Набір наклейок 160 шт. Тварини Djeco ", "10608398.jpg", 129, "Комплект постачання\r\n4 аркуші з наклейками по 40 штук\r\nКраїна-виробник товару\r\nФранція\r\nКраїна реєстрації бренда\r\nКитай\r\nГарантія\r\n14 днів\r\nВид\r\nНабори наклейок", "", ProductCategoryConstant.TOY, rand.Next(3, 6));
            await AddProduct("Ароматний набір для творчості Scentos Мегакреатив ", "17596089.jpg", 880, "Комплект постачання\r\n13 фломастерів\r\n12 кольорових олівців\r\n8 маркерів\r\nБагатоколірна кулькова ручка\r\n7 гелевих ручок\r\n19 воскових олівців\r\n2 аркуші з наклейками\r\nВага\r\n1250 г\r\nКолір\r\nРізнобарвний\r\nВік\r\n10 років\r\n11 років\r\n12 років\r\n13 років\r\n14 років\r\n15 років\r\n16 років\r\n3 роки\r\n3.5 роки\r\n4 роки\r\n5 років\r\n6 років\r\n7 років\r\n8 років\r\n9 років\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренда\r\nСША\r\nГарантія\r\n14 днів", "", ProductCategoryConstant.TOY, rand.Next(3, 6));
            await AddProduct("Коляска Smoby Toys Maxi-Cosi & Quinny 3 в 1 зі знімною люлькою", "12272711.jpg", 1880, "Аксесуари\r\nКоляски\r\nРозмір\r\n52 x 38.5 x 65.5 см\r\nМатеріал\r\nПластик, текстиль, метал\r\nОсобливості\r\nВік дитини від 3 до 8 років\r\nВік\r\n3 роки\r\n3.5 роки\r\n4 роки\r\n5 років\r\n6 років\r\n7 років\r\n8 років\r\nКраїна-виробник товару\r\nІспанія\r\nКраїна реєстрації бренду\r\nФранція\r\nГарантія\r\n14 днів", "", ProductCategoryConstant.TOY, rand.Next(3, 6));
            await AddProduct("Кран Dickie Toys Вантажний на ДК 100 см", "17361978.jpg", 1049, "Тип\r\nКрани\r\nЖивлення\r\n4 x AA\r\nГабарити\r\n21 х 56 х 12 см\r\nКомплектація\r\nБаштовий кран на дистанційному керуванні\r\n2 аксесуари для переміщення дрібного та габаритного вантажу\r\nКолір\r\nЖовтий\r\nТип керування\r\nWi-Fi-керування\r\nВік\r\n3 роки\r\n3.5 роки\r\n4 роки\r\n5 років\r\n6 років\r\n7 років\r\n8 років\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренду\r\nНімеччина\r\nГарантія\r\n14 днів", "", ProductCategoryConstant.TOY, rand.Next(3, 6));
            await AddProduct("Ігровий набір з лялькою Dream Seekers - Хоуп з аксесуарами", "196589284.jpg", 359, "Комплектація\r\n1 лялька з крилами, знімна стрічка для волосся, гребінець, 3 шпильки для волосся, знімне взуття, знімна спідниця, упаковка-стенд для ляльки з кишенею для зберігання секретів\r\nТип\r\nКолекційні ляльки\r\nЛяльки з аксесуарами\r\nВік\r\n10 років\r\n5 років\r\n6 років\r\n7 років\r\n8 років\r\n9 років\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренду\r\nСША", "", ProductCategoryConstant.TOY, rand.Next(3, 6));
            await AddProduct("Інтерактивний електронний навчальний робот-художник Quincy Kiddisvit", "33384076.jpg", 3236, "Вид\r\nРоботи\r\nВік\r\n3 роки\r\n3.5 роки\r\n4 роки\r\n5 років\r\n6 років\r\n7 років\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренду\r\nКитай\r\nГарантія\r\n3 місяці\r\nДодаткові характеристики\r\nЖивлення: 1 акумулятор Li-Ion\r\nРозміри\r\n17.5х17.5х15\r\nКолір\r\nЗелений\r\nКомплектація\r\n1 робот Квінсі\r\n34 картки з літерами\r\n10 карток з цифрами\r\n24 картки для малювання\r\n4 картки\r\nКабель USB", "", ProductCategoryConstant.TOY, rand.Next(3, 6));
            await AddProduct("Гнучкий трек Smoby Флекстрим з машинкою зі світловими ефектами 184 елементи 440 см", "254078572.jpg", 1515, "Довжина траси\r\n440 см\r\nКомплект постачання\r\nДо набору входить: трек, машинка, додатковий корпус для машинки та стартова платформа.\r\nВік\r\n10 років\r\n4 роки\r\n5 років\r\n6 років\r\n7 років\r\n8 років\r\n9 років\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренду\r\nФранція\r\nГарантія\r\n14 днів", "", ProductCategoryConstant.TOY, rand.Next(3, 6));
            await AddProduct("Вертоліт з 3-канальним і/ч, гіроскопом і водяною гарматою WL Toys Spray Copter Blue", "10605407.jpg", 1650, "Тип\r\nВертольоти\r\nРадіус дії\r\nдо 15 м\r\nЖивлення\r\nВбудований акумулятор Li-Pol 240 мА·год\r\nДля пульта 6 х AA батарей\r\nГабарити\r\n18.5 х 11.5 х 4.5 см\r\nКолір\r\nСиній\r\nТип керування\r\nІЧ-керування\r\nКраїна-виробник товару\r\nКитай\r\nКраїна реєстрації бренду\r\nКитай\r\nГарантія\r\n1 місяць", "", ProductCategoryConstant.TOY, rand.Next(3, 6));
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

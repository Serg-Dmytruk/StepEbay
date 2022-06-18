namespace StepEbay.Admin.Api.Common.Services.DbSeeder
{
    public interface ISeeder
    {
        public Task SeedApplication();
        public Task AddCategories();
        public Task AddProductStates();
        public Task AddProducts();
    }
}

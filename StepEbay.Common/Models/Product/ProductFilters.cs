namespace StepEbay.Main.Common.Models.Product
{
    public class ProductFilters
    {
        public List<ProductStateDto> States { get; set; } = new List<ProductStateDto>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public decimal PriceStart { get; set; } = 0;
        public decimal PriceEnd { get; set; } = 100000;
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}

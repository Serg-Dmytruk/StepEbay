namespace StepEbay.Main.Common.Models.Product
{
    public class ProductFilters
    {
        public int State { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public int PriceStart { get; set; }
        public int PriceEnd { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}

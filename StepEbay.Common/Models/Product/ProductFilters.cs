namespace StepEbay.Main.Common.Models.Product
{
    public class ProductFilters
    {
        public int state { get; set; }
        public List<Category> categories { get; set; } = new List<Category>();
        public int priceStart { get; set; }
        public int priceEnd { get; set; }
    }

    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool selected { get; set; }
    }
}

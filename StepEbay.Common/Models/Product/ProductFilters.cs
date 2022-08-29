namespace StepEbay.Main.Common.Models.Product
{
    public class ProductFilters
    {
        public List<State> States { get; set; } = new List<State>();
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

    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}

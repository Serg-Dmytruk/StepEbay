namespace StepEbay.Main.Common.Models.Product
{
    public class ProductFilterInfo
    {
        public List<int> Categories { get; set; }
        public List<int> States { get; set;}
        public decimal PriceStart { get; set; } = 0;
        public decimal PriceEnd { get; set; } = 100000;
    }
}

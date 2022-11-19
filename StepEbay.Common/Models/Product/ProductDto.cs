namespace StepEbay.Main.Common.Models.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string AdditionalInfo { get; set; }
        public int Rate { get; set; }
        public int CategoryId { get; set; }
        public int StateId { get; set; }
        public int OwnerId { get; set; }
        public int PurchaseTypeId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateClosed { get; set; }
        public Dictionary<string, string> ProductDescs { get; set; }
    }
}

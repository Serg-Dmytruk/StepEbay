namespace StepEbay.Main.Common.Models.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int StateId { get; set; }
        public int OwnerId { get; set; }
        public int PurchaseTypeId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

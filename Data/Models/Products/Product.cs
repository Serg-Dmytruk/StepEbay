using StepEbay.Data.Models.Bets;
using StepEbay.Data.Models.Default;
using StepEbay.Data.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StepEbay.Data.Models.Products
{
    public class Product : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? DateClose { get; set; }
        public bool IsActive { get; set; }
        public int Rate { get; set; }
        public string AditionalInfo { get; set; }


        [ForeignKey("Owner")]
        public int OwnerId { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [ForeignKey("ProductState")]
        public int ProductStateId { get; set; }

        [ForeignKey("PurchaseType")]
        public int PurchaseTypeId { get; set; }

        public virtual User Owner { get; set; }
        public virtual PurchaseType PurchaseType { get; set; }
        public virtual ProductState ProductState { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Favorite> Favorites { get; set; }
        public virtual List<Purchase> Purchases { get; set; }
        public virtual List<ProductDesc> Descriptions { get; set; }
    }
}
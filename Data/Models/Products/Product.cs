using StepEbay.Data.Models.Default;
using StepEbay.Data.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace StepEbay.Data.Models.Products
{
    public class Product : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public Category Category { get; set; }
        public ProductState ProductState { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool ByNow { get; set; }
        public int Count { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}
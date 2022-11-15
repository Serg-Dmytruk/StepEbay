using StepEbay.Data.Models.Default;
using System.ComponentModel.DataAnnotations;

namespace StepEbay.Data.Models.Products
{
    public class ProductDesc : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
    }
}

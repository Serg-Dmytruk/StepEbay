using StepEbay.Data.Models.Default;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StepEbay.Data.Models.Products
{
    public class ProductDesc : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string About { get; set; }

        public virtual Product Product { get; set; }
    }
}

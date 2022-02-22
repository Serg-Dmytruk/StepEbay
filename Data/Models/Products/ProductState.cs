using StepEbay.Data.Models.Default;
using System.ComponentModel.DataAnnotations;

namespace StepEbay.Data.Models.Products
{
    public class ProductState : IDbServiceEntity<byte>
    {
        [Key]
        public byte Id { get; set; }
        public string Name { get; set; }
    }
}

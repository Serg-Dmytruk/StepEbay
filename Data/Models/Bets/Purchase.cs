using StepEbay.Data.Models.Products;
using StepEbay.Data.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StepEbay.Data.Models.Bets
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User{ get; set; }

        [ForeignKey("Product")]
        public int PoductId { get; set; }

        public virtual Product Product { get; set; }

        public decimal PurchasePrice { get; set; }


    }
}

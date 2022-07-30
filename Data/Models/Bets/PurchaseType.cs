using StepEbay.Data.Models.Default;
using System.ComponentModel.DataAnnotations;

namespace StepEbay.Data.Models.Bets
{
    public class PurchaseType : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }
}

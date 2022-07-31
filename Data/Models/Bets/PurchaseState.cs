using StepEbay.Data.Models.Default;
using System.ComponentModel.DataAnnotations;

namespace StepEbay.Data.Models.Bets
{
    public  class PurchaseState : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string State { get; set; }
    }
}

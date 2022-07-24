using System.ComponentModel.DataAnnotations;

namespace StepEbay.Data.Models.Bets
{
    public class PurchaseType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }
}

namespace StepEbay.Main.Common.Models.Bet
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public decimal PurchasePrice { get; set; }
        public int UserId { get; set; }
        public int PoductId { get; set; }
        public int PurchaseStateId { get; set; }
    }
}

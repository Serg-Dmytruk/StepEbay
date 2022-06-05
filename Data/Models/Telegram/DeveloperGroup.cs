using StepEbay.Data.Models.Default;
using System.ComponentModel.DataAnnotations;

namespace StepEbay.Data.Models.Telegram
{
    public class DeveloperGroup : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Group { get; set; }
    }
}

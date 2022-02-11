using StepEbay.Data.Models.Default;
using StepEbay.Data.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StepEbay.Data.Models.Auth
{
    public class RefreshToken : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime UpdateTime { get; set; }
        public string Value { get; set; }
    }
}

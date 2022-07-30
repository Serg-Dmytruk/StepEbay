using StepEbay.Data.Models.Default;
using StepEbay.Data.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StepEbay.Data.Models.Auth
{
    public class UserRole : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public virtual User User{ get; set; }
        public virtual Role Role { get; set; }
    }
}

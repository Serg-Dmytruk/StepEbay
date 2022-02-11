using StepEbay.Data.Models.Default;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Auth
{
    public class Role : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

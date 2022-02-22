using StepEbay.Data.Models.Default;
using StepEbay.Data.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Data.Models.Users
{
    public class Favorite : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}

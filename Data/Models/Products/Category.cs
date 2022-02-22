using StepEbay.Data.Models.Default;
using System.ComponentModel.DataAnnotations;

namespace StepEbay.Data.Models.Products
{
	public class Category : IDbServiceEntity<int>
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<Product> Products { get; set; }
	}
}

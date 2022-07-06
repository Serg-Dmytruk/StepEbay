using StepEbay.Data.Models.Default;
using System.ComponentModel.DataAnnotations;

namespace StepEbay.Data.Models.Users
{
	public class User : IDbServiceEntity<int>
	{
		[Key]
		public int Id { get; set; }
		public DateTime Created { get; set; }
		public string FullName { get; set; }
		public string NickName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Adress { get; set; }
		public bool IsEmailConfirmed { get; set; }
		public Guid Guid { get; set; }
	}
}

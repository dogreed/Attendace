using System.ComponentModel.DataAnnotations;

namespace Attendace.Models
{
	public class Role
	{

		[Key]
		public int RoleId { get; set; }

		[Required, MaxLength(50)]
		public string RoleName { get; set; } = null!;

		public ICollection<User> Users { get; set; } = new List<User>();
	}
}

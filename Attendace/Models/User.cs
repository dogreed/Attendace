
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendace.Models
{
	

	public class User
	{
		[Key]
		public int UserId { get; set; }

		[Required, MaxLength(100)]
		public string Name { get; set; } = null!;

		[Required, EmailAddress, MaxLength(150)]
		public string Email { get; set; } = null!;

		[Required]
		public string PasswordHash { get; set; } = null!;

		[ForeignKey(nameof(Role))]
		public int RoleId { get; set; }

		public Role Role { get; set; } = null!;

		public Student? Student { get; set; }
		public Faculty? Faculty { get; set; }
		public Admin? Admin { get; set; }
	}

}

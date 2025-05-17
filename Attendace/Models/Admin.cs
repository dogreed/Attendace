using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendace.Models
{

	public class Admin
	{
		[Key]
		public int AdminId { get; set; }

		[ForeignKey(nameof(User))]
		public int UserId { get; set; }

		public User User { get; set; } = null!;

		[ForeignKey(nameof(Department))]
		public int DepartmentId { get; set; }

		public Department Department { get; set; } = null!;
	}
}

using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendace.Models
{
	public class Faculty
	{
		[Key]
		public int FacultyId { get; set; }

		[ForeignKey(nameof(User))]
		public int UserId { get; set; }

		public User User { get; set; } = null!;

		[ForeignKey(nameof(Department))]
		public int DepartmentId { get; set; }

		public Department Department { get; set; } = null!;

		public ICollection<FacultyCourse> FacultyCourses { get; set; } = new List<FacultyCourse>();
		public ICollection<Routine> Routines { get; set; } = new List<Routine>();
	}
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendace.Models
{
	public class Course
	{
		[Key]
		public int CourseId { get; set; }

		[Required, MaxLength(100)]
		public string CourseName { get; set; } = null!;

		[Required]
		public int Semester { get; set; }

		[Required, MaxLength(9)]
		public string AcademicYear { get; set; } = null!;

		[ForeignKey(nameof(Department))]
		public int DepartmentId { get; set; }
		public Department Department { get; set; } = null!;

		public ICollection<FacultyCourse> FacultyCourses { get; set; } = new List<FacultyCourse>();
		public ICollection<Routine> Routines { get; set; } = new List<Routine>();
		public ICollection<QRSession> QRSessions { get; set; } = new List<QRSession>();
	}
}

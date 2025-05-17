using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendace.Models
{
	public class FacultyCourse
	{
		[Key]
		public int FacultyCourseId { get; set; }

		[ForeignKey(nameof(Faculty))]
		public int FacultyId { get; set; }

		public Faculty Faculty { get; set; } = null!;

		[ForeignKey(nameof(Course))]
		public int CourseId { get; set; }

		public Course Course { get; set; } = null!;
	}
}

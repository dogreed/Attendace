using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendace.Models
{
	public class Routine
	{
		[Key]
		public int RoutineId { get; set; }

		[ForeignKey(nameof(Course))]
		public int CourseId { get; set; }

		public Course Course { get; set; } = null!;

		[ForeignKey(nameof(Faculty))]
		public int FacultyId { get; set; }

		public Faculty Faculty { get; set; } = null!;

		[Required, MaxLength(10)]
		public string DayOfWeek { get; set; } = null!; // Could be Monday, Tuesday etc.

		[Required]
		public TimeSpan StartTime { get; set; }

		[Required]
		public TimeSpan EndTime { get; set; }
	}
}

using Attendace.Models;
using System.ComponentModel.DataAnnotations;

namespace Attendace.ViewModel
{
	public class RoutineFormViewModel
	{
		public int? RoutineId { get; set; }

		[Required]
		public int CourseId { get; set; }
		[Required]
		public int? DepartmentId { get; set; }

		[Required]
		public int FacultyId { get; set; }

		[Required]
		public string DayOfWeek { get; set; }

		[Required]
		public TimeSpan StartTime { get; set; }

		[Required]
		public TimeSpan EndTime { get; set; }


		public List<Department>? Departments { get; set; }
		public List<Course>? Courses { get; set; }
		public List<Faculty>? Faculties { get; set; }
	}
}

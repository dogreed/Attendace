using Attendace.Models;

namespace Attendace.ViewModel
{
	public class CourseAssignmentViewModel
	{
		public int FacultyId { get; set; }
		public int DepartmentId { get; set; }
		public List<int> SelectedCourseIds { get; set; } = new List<int>();

		// For dropdowns
		public List<Faculty> Faculties { get; set; } = new();
		public List<Department> Departments { get; set; } = new();
		public List<Course> Courses { get; set; } = new();
	}
}

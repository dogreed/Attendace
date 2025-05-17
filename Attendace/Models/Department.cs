using System.ComponentModel.DataAnnotations;
namespace Attendace.Models
{
	

	public class Department
	{
		[Key]
		public int DepartmentId { get; set; }

		[Required, MaxLength(100)]
		public string DepartmentName { get; set; } = null!;

		public ICollection<Student> Students { get; set; } = new List<Student>();
		public ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();
		public ICollection<Admin> Admins { get; set; } = new List<Admin>();
		public ICollection<Course> Courses { get; set; } = new List<Course>();
	}

}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendace.Models
{
	public class Student
	{
		[Key]
		public int StudentId { get; set; }

		[ForeignKey(nameof(User))]
		public int UserId { get; set; }

		public User User { get; set; } = null!;

		[Required, MaxLength(20)]
		public string RollNo { get; set; } = null!;

		[Required]
		public int Semester { get; set; }

		[Required, MaxLength(9)] // e.g. "2024-2025"
		public string AcademicYear { get; set; } = null!;

		[ForeignKey(nameof(Department))]
		public int DepartmentId { get; set; }

		public Department Department { get; set; } = null!;

		public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
	}
	}

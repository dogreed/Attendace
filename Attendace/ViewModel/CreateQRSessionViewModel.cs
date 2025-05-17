using Attendace.Models;
using System.ComponentModel.DataAnnotations;

namespace Attendace.ViewModel
{
	public class CreateQRSessionViewModel
	{
		public int CourseId { get; set; }

		[Required]
		[Range(1, 120)]
		public int DurationMinutes { get; set; } = 5;

		public bool IsFlexible { get; set; } = false;

		public List<Course>? AvailableCourses { get; set; }
	}
}

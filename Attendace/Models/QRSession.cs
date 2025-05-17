using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendace.Models
{
	public class QRSession
	{
		[Key]
		public int QRSessionId { get; set; }

		[ForeignKey(nameof(Course))]
		public int CourseId { get; set; }

		public Course Course { get; set; } = null!;

		[Required]
		public string Token { get; set; } = null!;

		[Required]
		public DateTime GeneratedAt { get; set; }

		[Required]
		public DateTime ExpiresAt { get; set; }

		public bool IsFlexible { get; set; } = false;
		public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
	}
}

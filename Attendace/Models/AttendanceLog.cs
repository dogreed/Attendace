using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendace.Models
{
	public class AttendanceLog
	{
		[Key]
		public int AttendanceId { get; set; }

		[ForeignKey(nameof(Student))]
		public int StudentId { get; set; }

		public Student Student { get; set; } = null!;

		[ForeignKey(nameof(QRSession))]
		public int QRSessionId { get; set; }

		public QRSession QRSession { get; set; } = null!;

		[Required]
		public DateTime TimeStamp { get; set; }
	}
}

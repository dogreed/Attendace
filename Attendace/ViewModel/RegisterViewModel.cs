using System.ComponentModel.DataAnnotations;

namespace Attendace.ViewModel
{
	public class RegisterViewModel
	{
		[Required, EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		public string Name { get; set; } = null!;

		[Required, DataType(DataType.Password)]
		public string Password { get; set; } = null!;

		[Required, DataType(DataType.Password), Compare(nameof(Password))]
		public string ConfirmPassword { get; set; } = null!;

		public string? Role { get; set; } // optional, default Student
	}
}

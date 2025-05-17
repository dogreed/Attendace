using Attendace.Data;
using Attendace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Attendace.Controllers
{
	[Authorize(Roles = "Student")]
	public class StudentController : Controller
    {
		private readonly ApplicationDbContext _context;

		public StudentController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Dashboard()
        {
            return View();
        }
		[HttpGet]
		public async Task<IActionResult> Submit(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				return BadRequest("Invalid token.");
			}

			var session = await _context.QRSessions
				.Include(s => s.Course)
				.FirstOrDefaultAsync(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow);

			if (session == null)
			{
				return NotFound("QR Session not found or expired.");
			}

			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null)
			{
				return RedirectToAction("Login", "Account");
			}

			int userId = int.Parse(userIdClaim.Value);
			var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);

			if (student == null)
			{
				return Unauthorized("Only students can mark attendance.");
			}

			var alreadyMarked = await _context.AttendanceLogs.AnyAsync(a =>
				a.StudentId == student.StudentId && a.QRSessionId == session.QRSessionId);

			if (alreadyMarked)
			{
				TempData["Message"] = "You have already marked attendance for this session.";
				return View("AttendanceResult");
			}

			var attendance = new AttendanceLog
			{
				StudentId = student.StudentId,
				QRSessionId = session.QRSessionId,
				TimeStamp = DateTime.UtcNow
			};

			_context.AttendanceLogs.Add(attendance);
			await _context.SaveChangesAsync();

			TempData["Message"] = "Attendance marked successfully.";
			return View("AttendanceResult");
		}

		public IActionResult AttendanceResult()
		{
			return View();
		}
		public IActionResult SubmitAttendance()
		{
			return View();
		}

	}
}


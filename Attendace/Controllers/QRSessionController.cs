using Attendace.Data;
using Attendace.Models;
using Attendace.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendace.Controllers
{
	[Authorize(Roles = "Faculty")]
	public class QRSessionController : Controller
    {
		private readonly ApplicationDbContext _context;

		public QRSessionController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Create()
		{
			var courses = await _context.Courses.ToListAsync(); // You can filter by faculty if needed
			return View(new CreateQRSessionViewModel { AvailableCourses = courses });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateQRSessionViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.AvailableCourses = await _context.Courses.ToListAsync();
				return View(model);
			}

			var token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

			var qrSession = new QRSession
			{
				CourseId = model.CourseId,
				Token = token,
				GeneratedAt = DateTime.UtcNow,
				ExpiresAt = DateTime.UtcNow.AddMinutes(model.DurationMinutes)
			};

			try
			{
				_context.QRSessions.Add(qrSession);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Log the error (or use TempData/ViewBag to show it in view)
				Console.WriteLine("DB Save Failed: " + ex.Message);
				ViewBag.Error = "Could not save session.";
				return View(model);
			}

			return RedirectToAction("QRCode", new { token = qrSession.Token });
		}

		public async Task<IActionResult> QRCode(string token)
		{
			var url = Url.Action("Submit", "Student", new { token }, Request.Scheme);
			ViewBag.QRUrl = url;

			var model = new CreateQRSessionViewModel
			{
				AvailableCourses = await _context.Courses.ToListAsync()
			};

			return View(model);  // pass the model here!
		}

	}
}
	

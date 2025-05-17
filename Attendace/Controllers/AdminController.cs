using Attendace.Data;
using Attendace.Models;
using Attendace.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Attendace.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
    {
		private readonly ApplicationDbContext _context;

		public AdminController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Dashboard()
        {
            return View();
        }

		public async Task<IActionResult> GenerateQR()
		{
			var currentDay = DateTime.Now.DayOfWeek.ToString(); // "Monday", "Tuesday", etc.

			// Get routines for the current day including related Course entity
			var routines = await _context.Routines
				.Include(r => r.Course) // include related Course for each routine
				.Where(r => r.DayOfWeek == currentDay)
				.ToListAsync();

			// Extract distinct courses from routines
			var courses = routines.Select(r => r.Course).Distinct().ToList();

			var model = new CreateQRSessionViewModel
			{
				AvailableCourses = courses
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GenerateQR(CreateQRSessionViewModel model)
		{
			var currentDay = DateTime.Now.DayOfWeek.ToString();

			// Validate model state
			if (!ModelState.IsValid)
			{
				// Reload courses for current day if model is invalid
				var routines = await _context.Routines
					.Include(r => r.Course)
					.Where(r => r.DayOfWeek == currentDay)
					.ToListAsync();

				model.AvailableCourses = routines.Select(r => r.Course).Distinct().ToList();
				return View(model);
			}

			// Check if the selected course is part of today's routine
			var isCourseValid = await _context.Routines
				.AnyAsync(r => r.DayOfWeek == currentDay && r.CourseId == model.CourseId);

			if (!isCourseValid)
			{
				ModelState.AddModelError("CourseId", "Selected course is not scheduled for today.");
				var routines = await _context.Routines
					.Include(r => r.Course)
					.Where(r => r.DayOfWeek == currentDay)
					.ToListAsync();
				model.AvailableCourses = routines.Select(r => r.Course).Distinct().ToList();
				return View(model);
			}

			// Generate token and create QR session
			var token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

			var qrSession = new QRSession
			{
				CourseId = model.CourseId,
				Token = token,
				GeneratedAt = DateTime.UtcNow,
				ExpiresAt = DateTime.UtcNow.AddMinutes(model.DurationMinutes)
			};

			_context.QRSessions.Add(qrSession);
			await _context.SaveChangesAsync();

			return RedirectToAction("QRCode", new { token = qrSession.Token });
		}

		public IActionResult QRCode(string token)
		{
			var url = Url.Action("Submit", "Student", new { token }, Request.Scheme);
			ViewBag.QRUrl = url;

			return View();
		}

		public async Task<IActionResult> CreateRoutine()
		{
			var model = new RoutineFormViewModel
			{
				Departments = await _context.Departments.ToListAsync(),
				Faculties = await _context.Faculties.Include(f => f.User).ToListAsync(),
				Courses = new List<Course>() // empty initially; loaded by AJAX after department select
			};
			ViewBag.Days = new SelectList(new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });

			return View(model);
		}

		public async Task<JsonResult> GetCoursesByDepartment(int departmentId)
		{
			var courses = await _context.Courses
				.Where(c => c.DepartmentId == departmentId)
				.Select(c => new { c.CourseId, c.CourseName })
				.ToListAsync();

			return Json(courses);
		}

		// POST: Save Routine
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateRoutine(RoutineFormViewModel model)
		{
			ViewBag.Days = new SelectList(new[]
			{
			"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"
			}, model.DayOfWeek);


			if (!ModelState.IsValid)
			{
				model.Departments = await _context.Departments.ToListAsync();
				model.Faculties = await _context.Faculties.Include(f => f.User).ToListAsync();
				model.Courses = await _context.Courses.Where(c => c.DepartmentId == model.DepartmentId).ToListAsync();
				return View(model);
			}

			var routine = new Routine
			{
				CourseId = model.CourseId,
				FacultyId = model.FacultyId,
				DayOfWeek = model.DayOfWeek,
				StartTime = model.StartTime,
				EndTime = model.EndTime
			};

			_context.Routines.Add(routine);
			await _context.SaveChangesAsync();

			return RedirectToAction("RoutineList");
		}

		// GET: View All Routines
		public async Task<IActionResult> RoutineList()
		{
			var routines = await _context.Routines
				.Include(r => r.Course)
					.ThenInclude(c => c.Department) // Include Department from Course
				.Include(r => r.Faculty)
					.ThenInclude(f => f.User) // Include Faculty's User
				.ToListAsync();

			return View(routines);
		}


		public async Task<IActionResult> EditRoutine(int id)
		{
			var routine = await _context.Routines
				.Include(r => r.Course)
				.Include(r => r.Faculty)
				.FirstOrDefaultAsync(r => r.RoutineId == id);

			if (routine == null)
				return NotFound();

			var model = new RoutineFormViewModel
			{
				RoutineId = routine.RoutineId,
				DepartmentId = routine.Course.DepartmentId,
				CourseId = routine.CourseId,
				FacultyId = routine.FacultyId,
				DayOfWeek = routine.DayOfWeek,
				StartTime = routine.StartTime,
				EndTime = routine.EndTime,

				// For dropdowns
				Departments = await _context.Departments.ToListAsync(),
				Faculties = await _context.Faculties.Include(f => f.User).ToListAsync(),
				Courses = await _context.Courses.Where(c => c.DepartmentId == routine.Course.DepartmentId).ToListAsync()
			};

			// Provide Days list for dropdown (if using ViewBag)
			ViewBag.Days = new SelectList(
	new[]
	{
		"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"
	}.Select(day => new SelectListItem
	{
		Text = day,
		Value = day
	}), "Value", "Text", model.DayOfWeek);


			return View("CreateRoutine", model);
		}



		// POST: Edit Routine
		[HttpPost]
		[ValidateAntiForgeryToken]
		
		public async Task<IActionResult> EditRoutine(RoutineFormViewModel model)
		{
			

			if (!ModelState.IsValid)
			{
				model.Departments = await _context.Departments.ToListAsync();
				model.Faculties = await _context.Faculties.Include(f => f.User).ToListAsync();
				model.Courses = await _context.Courses
					.Where(c => c.DepartmentId == model.DepartmentId)
					.ToListAsync();
				return View("CreateRoutine", model); // reuse same view
			}

			var routine = await _context.Routines.FindAsync(model.RoutineId);
			if (routine == null) return NotFound();

			routine.CourseId = model.CourseId;
			routine.FacultyId = model.FacultyId;
			routine.DayOfWeek = model.DayOfWeek;
			routine.StartTime = model.StartTime;
			routine.EndTime = model.EndTime;

			await _context.SaveChangesAsync();

			return RedirectToAction("RoutineList");
		}


		// GET: Confirm Delete
		public async Task<IActionResult> DeleteRoutine(int id)
		{
			var routine = await _context.Routines
				.Include(r => r.Course)
				.Include(r => r.Faculty)
				.ThenInclude(f => f.User)
				.FirstOrDefaultAsync(r => r.RoutineId == id);

			if (routine == null) return NotFound();

			return View(routine);
		}

		// POST: Confirm Delete
		[HttpPost, ActionName("DeleteRoutine")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var routine = await _context.Routines.FindAsync(id);
			if (routine == null) return NotFound();

			_context.Routines.Remove(routine);
			await _context.SaveChangesAsync();
			return RedirectToAction("RoutineList");
		}

	}
}

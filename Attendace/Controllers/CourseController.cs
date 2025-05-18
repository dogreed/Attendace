using Attendace.Data;
using Attendace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendace.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CourseController : Controller
    {
		private readonly ApplicationDbContext _context;

		public CourseController(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> List()
		{
			var courses = await _context.Courses.Include(c => c.Department).ToListAsync();
			return View("List", courses);
		}
		// GET: /Course/Add
		public async Task<IActionResult> Add()
		{
			ViewBag.Departments = await _context.Departments.ToListAsync();
			return View(new Course());
		}

		// POST: /Course/Add
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(Course course)
		{
			ModelState.Remove("Department");
			if (ModelState.IsValid)
			{
				_context.Courses.Add(course);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(List));
			}

			ViewBag.Departments = await _context.Departments.ToListAsync();
			return View(course);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var course = await _context.Courses.FindAsync(id);
			if (course == null) return NotFound();

			ViewBag.Departments = await _context.Departments.ToListAsync();
			return View(course);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Course course)
		{
			if (id != course.CourseId) return NotFound();

			ModelState.Remove("Department"); // avoid validation error, if  any foregin key is blocking 
			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(course);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(List));
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_context.Courses.Any(c => c.CourseId == course.CourseId))
						return NotFound();

					throw;
				}
			}

			ViewBag.Departments = await _context.Departments.ToListAsync();
			return View(course);
		}
		public async Task<IActionResult> ConfirmDelete(int id)
		{
			var course = await _context.Courses
				.Include(c => c.Department)
				.FirstOrDefaultAsync(c => c.CourseId == id);

			if (course == null) return NotFound();
			return View(course);
		}
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var course = await _context.Courses.FindAsync(id);
			if (course != null)
			{
				_context.Courses.Remove(course);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(List));
		}





	}
}

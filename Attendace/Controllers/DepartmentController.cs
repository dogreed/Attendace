using Attendace.Data;
using Attendace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendace.Controllers
{
	[Authorize(Roles = "Admin")]
	public class DepartmentController : Controller
    {
		private readonly ApplicationDbContext _context;

		public DepartmentController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var departments = await _context.Departments.ToListAsync();
			return View(departments);
		}
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Department department)
		{
			if (ModelState.IsValid)
			{
				_context.Add(department);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(department);
		}
		// GET: Edit department
		public async Task<IActionResult> Edit(int id)
		{
			var department = await _context.Departments.FindAsync(id);
			if (department == null)
				return NotFound();

			return View(department);
		}
		// POST: Edit department
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Department department)
		{
			if (id != department.DepartmentId)
				return NotFound();

			if (ModelState.IsValid)
			{
				_context.Update(department);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(department);
		}
		// GET: Delete department
		public async Task<IActionResult> Delete(int id)
		{
			var department = await _context.Departments.FindAsync(id);
			if (department == null)
				return NotFound();

			return View(department);
		}

		// POST: Confirm delete
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var department = await _context.Departments.FindAsync(id);
			if (department != null)
			{
				_context.Departments.Remove(department);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}

	}
}

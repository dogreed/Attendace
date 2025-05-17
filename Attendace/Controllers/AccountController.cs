using Attendace.Data;
using Attendace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Attendace.ViewModel;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Attendace.Controllers
{
    public class AccountController : Controller
    {
		private readonly ApplicationDbContext _context;
		public AccountController(ApplicationDbContext context)
		{
			_context = context;
		}
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			if (_context.Users.Any(u => u.Email == model.Email))
			{
				ModelState.AddModelError("Email", "Email already registered.");
				return View(model);
			}

			if (model.Password != model.ConfirmPassword)
			{
				ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
				return View(model);
			}

			// Find Role entity for the selected role name (or default to "Student")
			var roleName = model.Role ?? "Student";
			var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
			if (role == null)
			{
				ModelState.AddModelError("", "Role not found.");
				return View(model);
			}

			var hasher = new PasswordHasher<User>();
			var user = new User
			{
				Name = model.Name,
				Email = model.Email,
				RoleId = role.RoleId,
				PasswordHash = ""
			};
			user.PasswordHash = hasher.HashPassword(user, model.Password);

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			TempData["SuccessMessage"] = "Registration successful. Please login.";
			return RedirectToAction(nameof(Login));
		}

		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = await _context.Users.Include(u => u.Role)
										   .FirstOrDefaultAsync(u => u.Email == model.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "Invalid email or password.");
				return View(model);
			}

			var hasher = new PasswordHasher<User>();
			var result = hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
			if (result == PasswordVerificationResult.Success)
			{
				var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
			new Claim(ClaimTypes.Name, user.Name),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.Role.RoleName)
		};

				var identity = new ClaimsIdentity(claims, "MyCookieAuth");
				var principal = new ClaimsPrincipal(identity);

				await HttpContext.SignInAsync("MyCookieAuth", principal);

				return user.Role.RoleName switch
				{
					"SuperAdmin" => RedirectToAction("Dashboard", "SuperAdmin"),
					"Admin" => RedirectToAction("Dashboard", "Admin"),
					"Faculty" => RedirectToAction("Dashboard", "Faculty"),
					"Student" => RedirectToAction("Dashboard", "Student"),
					_ => RedirectToAction("Index", "Home"),
				};
			}

			ModelState.AddModelError("", "Invalid email or password.");
			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync("MyCookieAuth");
			return RedirectToAction("Login", "Account");
		}
		public IActionResult AccessDenied()
		{
			return View();
		}
	}


}

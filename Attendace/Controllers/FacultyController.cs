using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendace.Controllers
{
	[Authorize(Roles = "Faculty")]
	public class FacultyController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}

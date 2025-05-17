using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendace.Controllers
{
	[Authorize(Roles = "SuperAdmin")]
	public class SuperAdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}

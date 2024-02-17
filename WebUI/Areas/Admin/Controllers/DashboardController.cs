using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area(nameof(Admin))]
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

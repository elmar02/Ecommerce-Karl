using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Super Admin,Admin")]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #region Role List
        public IActionResult Index()
        {
            var roles = _roleService.GetRoles();
            if (!roles.Success)
                return Redirect("/error/500");
            return View(roles.Data);
        }
        #endregion
        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            var result = await _roleService.CreateRoleAsync(roleName);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                return View();
            }
            return Redirect($"/{AdminName.Admin}/roles");
        }
        #endregion
        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _roleService.DeleteRoleAsync(id);
            return Redirect($"/{AdminName.Admin}/roles");
        }
        #endregion
    }
}

using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.Models;
using Entities.DTOs.UserDTOs;
namespace WebUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Super Admin,Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        #region User List
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetUserListAsync();
            if (!users.Success)
                return Redirect("/error/500");
            return View(users.Data);
        }
        #endregion
        #region Add Role
        public async Task<IActionResult> AddRole(string id) 
        {
            var result = await _userService.GetOtherRolesAsync(id);
            if (!result.Success)
                return Redirect("/error/500");
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string id,string roleName)
        {
            var result = await _userService.AddRoleToUserAsync(id,roleName);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                var result1 = await _userService.GetOtherRolesAsync(id);
                if (!result1.Success)
                    return Redirect("/error/500");
                return View(result1.Data);
            }
            return Redirect($"/{AdminName.Admin}/users");
        }
        #endregion
        #region Delete Role
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _userService.GetRemoveRolesAsync(id);
            if (!result.Success)
                return Redirect("/error/500");
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id,string roleName)
        {
            var result = await _userService.RemoveRoleFromUserAsync(id,roleName);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                var result1 = await _userService.GetRemoveRolesAsync(id);
                if (!result1.Success)
                    return Redirect("/error/500");
                return View(result1.Data);
            }
            return Redirect($"/{AdminName.Admin}/users");
        }
        #endregion
    }
}

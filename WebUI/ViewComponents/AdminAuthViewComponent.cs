using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebUI.ViewComponents
{
    public class AdminAuthViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public AdminAuthViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _userService.GetAdminAuthAsync();
            if (!result.Success)
                return Content(result.Message);
            return View("AdminAuth", result.Data);
        }
    }
}

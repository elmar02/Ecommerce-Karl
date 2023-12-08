using Business.Abstract;
using Business.Concrete;
using Core.Helper;
using Entities.DTOs.UserDTOs;
using Entities.DTOs.VerifyDTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IVerificationCodeService _verificationCodeService;

        public AuthController(IUserService userService, IVerificationCodeService verificationCodeService)
        {
            _userService = userService;
            _verificationCodeService = verificationCodeService;
        }

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var result = await _userService.LoginAsync(loginDTO);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                return View();
            }
            return Redirect("/");
        }
        #endregion
        #region Register
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var result = await _userService.RegisterAsync(registerDTO);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                return View();
            }
            return Redirect($"/auth/verifyEmail?linkId={result.Data}");
        }
        #endregion
        #region Edit Profile
        public async Task<IActionResult> EditProfile()
        {
            var result = await _userService.GetProfileAsync();
            if (!result.Success) return Redirect("/auth/login");
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileDTO editProfileDTO)
        {
            var result = await _userService.EditProfileAsync(editProfileDTO);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                return View();
            }
            return Redirect("/");
        }
        #endregion
        #region Change Password
        public IActionResult ChangePassword()
        {
            return View();
        }
        #endregion
        #region Logout
        [HttpPost]
        public IActionResult Logout()
        {
            return Redirect;
        }
        #endregion
        #region VerifyEmail
        public IActionResult VerifyEmail(string linkId)
        {
            if (string.IsNullOrEmpty(linkId)) return NotFound();
            var result = _verificationCodeService.FindVerificationCodeByLinkId(linkId);
            if (!result.Success) return NotFound();
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(CheckVerifyDTO checkVerifyDTO)
        {
            int code = checkVerifyDTO.CodeArray.ConvertCode();
            var result = await _userService.VerifyEmailAsync(checkVerifyDTO.Id,code);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                return View(checkVerifyDTO);
            }
            return Redirect("/auth/login");
        }
        #endregion
    }
}

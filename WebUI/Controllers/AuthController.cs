using Business.Abstract;
using Core.Helper;
using Entities.DTOs.UserDTOs;
using Entities.DTOs.VerifyDTOs;
using Microsoft.AspNetCore.Authorization;
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
                return View(loginDTO);
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
                return View(registerDTO);
            }
            return Redirect($"/auth/verifyEmail?linkId={result.Data}");
        }
        #endregion
        #region Edit Profile
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var result = await _userService.GetProfileAsync();
            if (!result.Success) return Redirect("/auth/login");
            return View(result.Data);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileDTO editProfileDTO)
        {
            var result = await _userService.EditProfileAsync(editProfileDTO);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                return View(editProfileDTO);
            }
            return Redirect("/");
        }
        #endregion
        #region Change Password
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            var result = await _userService.ChangePasswordAsync(changePasswordDTO);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                return View(changePasswordDTO);
            }
            ViewData["Success"] = result.Message;
            return View();
        }
        #endregion
        #region Logout
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var result = await _userService.LogoutAsync();
            if (!result.Success)
                return Redirect("/error/500");
            return Redirect("/");
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

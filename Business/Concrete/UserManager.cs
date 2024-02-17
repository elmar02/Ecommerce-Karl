using AutoMapper;
using Business.Abstract;
using Core.Helper.MailHelper;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmailService _emailService;
        private readonly IVerificationCodeService _verificationCodeService;
        public UserManager(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IRoleService roleManager, IHttpContextAccessor contextAccessor, IEmailService emailHelper, IVerificationCodeService verificationCodeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _contextAccessor = contextAccessor;
            _emailService = emailHelper;
            _verificationCodeService = verificationCodeService;
        }

        public async Task<IResult> EditProfileAsync(EditProfileDTO editProfileDTO)
        {
            try
            {
                var updatedUser = await _userManager.FindByIdAsync(editProfileDTO.Id);
                if (updatedUser == null) return new ErrorResult("Unexpected error occured");
                updatedUser.FirstName = editProfileDTO.FirstName;
                updatedUser.LastName = editProfileDTO.LastName;
                await _userManager.UpdateAsync(updatedUser);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public async Task<IDataResult<EditProfileDTO>> GetProfileAsync()
        {
            try
            {
                var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (userId == null) return new ErrorDataResult<EditProfileDTO>();
                var currentUser = await _userManager.FindByIdAsync(userId);
                if (currentUser == null) return new ErrorDataResult<EditProfileDTO>();
                var map = _mapper.Map<EditProfileDTO>(currentUser);
                return new SuccessDataResult<EditProfileDTO>(map);
            }
            catch (Exception)
            {
                return new ErrorDataResult<EditProfileDTO>();
            }
        }

        public async Task<IResult> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var checkUser = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (checkUser == null) return new ErrorResult("User not Found!");
                var result = await _signInManager.PasswordSignInAsync(checkUser, loginDTO.Password, loginDTO.RememberMe, true);
                if (!result.Succeeded) return new ErrorResult("Email or Password is incorrect!");
                if (!checkUser.EmailConfirmed) return new ErrorResult("Please verify this email before sign in!");
                return new SuccessResult("Login Successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public async Task<IResult> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult();
            }
        }

        public async Task<IDataResult<string>> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
                var isValidEmail = _emailService.IsValidEmail(registerDTO.Email);
                if (!isValidEmail) 
                    return new ErrorDataResult<string>(message: "Invalid Email!");

                if (registerDTO.Password != registerDTO.ConfirmPassword) 
                    return new ErrorDataResult<string>();

                var checkUser = await _userManager.FindByEmailAsync(registerDTO.Email);
                if (checkUser != null) 
                    return new ErrorDataResult<string>(message: "Email is already used!");

                var newUser = _mapper.Map<User>(registerDTO);
                newUser.UserName = newUser.Email;

                int code = new Random().Next(100000, 1000000);
                var emailResult = await _emailService.SendEmailAsync(newUser.Email, newUser.FirstName, "Verification Code", GetBody(newUser.FirstName, code));
                if(!emailResult.Success)
                    return new ErrorDataResult<string>(message: emailResult.Message);

                var result = await _userManager.CreateAsync(newUser, registerDTO.Password);
                if (!result.Succeeded) 
                    return new ErrorDataResult<string>(message: string.Join('\n', result.Errors.Select(x=>x.Description)));

                await FirstRegisterAsync();
                var hasUserRole = await _userManager.IsInRoleAsync(newUser, "User");
                if (!hasUserRole) 
                    await _userManager.AddToRoleAsync(newUser, "User");

                var verifyResult = _verificationCodeService.CreateVerificationCode(newUser.Id, code);
                if(!verifyResult.Success) 
                    return new ErrorDataResult<string>(message: verifyResult.Message);

                return new SuccessDataResult<string>(data: verifyResult.Data,message: "Register Succesfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<string>(message: ex.Message);
            }
        }

        public async Task<IResult> VerifyEmailAsync(int id, int code)
        {
            try
            {
                var verificationCode = _verificationCodeService.GetVerificationCodeById(id);
                if (!verificationCode.Success) return new ErrorResult("Unexpected error occured!");

                var result = _verificationCodeService.IsValidVerificationCode(code, verificationCode.Data);
                if (!result.Success) return new ErrorResult(result.Message);

                var user = await _userManager.FindByIdAsync(verificationCode.Data.UserId);
                if (user == null) return new ErrorResult("User not Found");

                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return new SuccessResult("Email verified succesfully!");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }

        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                if (changePasswordDTO.NewPassword != changePasswordDTO.ConfirmNewPassword)
                    return new ErrorResult("Confirm Password doesnt match!");

                var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return new ErrorResult();

                var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword,changePasswordDTO.NewPassword);
                if (!result.Succeeded)
                {
                    return new ErrorResult("Current password is incorrect");
                }
                return new SuccessResult("Password Changed Successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        private async Task FirstRegisterAsync()
        {
            try
            {
                var users = _userManager.Users.ToList();
                if (users.Count != 1) return;
                var user = users[0];

                await _roleManager.CreateRoleAsync("Super Admin");
                await _roleManager.CreateRoleAsync("User");
                await _roleManager.CreateRoleAsync("Admin");

                await _userManager.AddToRoleAsync(user, "Super Admin");
                await _userManager.AddToRoleAsync(user, "User");
            }
            catch (Exception)
            {

            }
        }
        private static string GetBody(string name, int code)
        {
            return
                $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <title>Email Verification</title>\r\n</head>\r\n<body style=\"font-family: Arial, sans-serif;\">\r\n\r\n  <div style=\"max-width: 600px; margin: 0 auto;\">\r\n    <h2>Email Verification Code</h2>\r\n    <p>Dear {name},</p>\r\n    <p>Your verification code is: <strong>{code}</strong></p>\r\n    <p>Please use this code to complete your verification process.</p>\r\n    <p>If you did not request this code, please disregard this email.</p>\r\n    <br>\r\n    <p>Best Regards,<br>Karl Ecommerce</p>\r\n  </div>\r\n\r\n</body>\r\n</html>\r\n";
        }

        public async Task<IDataResult<List<string>>> GetUserRolesAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return new ErrorDataResult<List<string>>();
                var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
                return new SuccessDataResult<List<string>>(userRoles);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<string>>();
            } 
        }

        public async Task<IDataResult<List<UserListDTO>>> GetUserListAsync()
        {
            try
            {
                var users = _userManager.Users
                .Select(x => new UserListDTO()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = (x.FirstName + " " + x.LastName),
                    UserRoles = new List<string>()
                })
                .ToList();

                foreach (var item in users)
                {
                    var userRoles = await GetUserRolesAsync(item.Id);
                    if (!userRoles.Success)
                        return new ErrorDataResult<List<UserListDTO>>();
                    item.UserRoles = userRoles.Data;
                }
                return new SuccessDataResult<List<UserListDTO>>(users);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<UserListDTO>>();
            }
        }

        public async Task<IDataResult<AdminAuthDTO>> GetAdminAuthAsync()
        {
            try
            {
                var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(userId);
                var map = _mapper.Map<AdminAuthDTO>(user);
                return new SuccessDataResult<AdminAuthDTO>(map);
            }
            catch (Exception)
            {
                return new ErrorDataResult<AdminAuthDTO>("");
            }
        }

        public async Task<IResult> AddRoleToUserAsync(string id, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return new ErrorResult("User not found");
                await _userManager.AddToRoleAsync(user, roleName);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public async Task<IDataResult<UserRolesDTO>> GetOtherRolesAsync(string id)
        {
            try
            {
                var userRoles = await GetUserRolesAsync(id);
                var allRoles = _roleManager.GetRoles();
                var otherRoles = allRoles.Data
                    .Select(x=>x.Name)
                    .Where(x=>!userRoles.Data.Contains(x) && x != "Super Admin")
                    .ToList();
                var userRolesDTO = new UserRolesDTO() 
                {
                    Id = id,
                    OtherRoles = otherRoles
                };
                return new SuccessDataResult<UserRolesDTO>(userRolesDTO);
            }
            catch (Exception)
            {
                return new ErrorDataResult<UserRolesDTO>();
            }
        }

        public async Task<IResult> RemoveRoleFromUserAsync(string id, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return new ErrorResult("User not found");
                await _userManager.RemoveFromRoleAsync(user, roleName);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public async Task<IDataResult<UserRolesDTO>> GetRemoveRolesAsync(string id)
        {
            try
            {
                var userRoles = await GetUserRolesAsync(id);
                var otherRoles = userRoles.Data
                    .Where(x =>  x != "Super Admin" && x!= "User")
                    .ToList();
                var userRolesDTO = new UserRolesDTO()
                {
                    Id = id,
                    OtherRoles = otherRoles
                };
                return new SuccessDataResult<UserRolesDTO>(userRolesDTO);
            }
            catch (Exception)
            {
                return new ErrorDataResult<UserRolesDTO>();
            }
        }
    }
}

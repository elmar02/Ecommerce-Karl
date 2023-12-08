using AutoMapper;
using Azure.Core;
using Business.Abstract;
using Core.Helper.MailHelper;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmailHelper _emailHelper;
        private readonly IVerificationCodeService _verificationCodeService;
        public UserManager(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, AppDbContext context, RoleManager<IdentityRole> roleManager, IHttpContextAccessor contextAccessor, IEmailHelper emailHelper, IVerificationCodeService verificationCodeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
            _roleManager = roleManager;
            _contextAccessor = contextAccessor;
            _emailHelper = emailHelper;
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

        public async Task<IDataResult<string>> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
                var isValidEmail = _emailHelper.IsValidEmail(registerDTO.Email);
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
                var emailResult = await _emailHelper.SendEmailAsync(newUser.Email, newUser.FirstName, "Verification Code | Karl-Ecommerce", code);
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

                return new SuccessDataResult<string>(verifyResult.Data,"Register Succesfully");
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

        private async Task FirstRegisterAsync()
        {
            try
            {
                var users = _context.Users.ToList();
                if (users.Count != 1) return;
                var user = users[0];
                var superAdminRole = new IdentityRole()
                {
                    Name = "Super Admin"
                };
                var userRole = new IdentityRole()
                {
                    Name = "User"
                };
                await _roleManager.CreateAsync(superAdminRole);
                await _roleManager.CreateAsync(userRole);

                await _userManager.AddToRoleAsync(user, superAdminRole.Name);
                await _userManager.AddToRoleAsync(user, userRole.Name);
            }
            catch (Exception)
            {

            }
        }
    }
}

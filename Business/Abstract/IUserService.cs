using Core.Utilities.Results.Abstract;
using Entities.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<IResult> LoginAsync(LoginDTO loginDTO);
        Task<IDataResult<string>> RegisterAsync(RegisterDTO registerDTO);
        Task<IDataResult<EditProfileDTO>> GetProfileAsync();
        Task<IResult> EditProfileAsync(EditProfileDTO editProfileDTO);
        Task<IResult> VerifyEmailAsync(int id, int code);
        Task<IResult> LogoutAsync();
        Task<IResult> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);
        Task<IDataResult<List<string>>> GetUserRolesAsync(string id);
        Task<IDataResult<List<UserListDTO>>> GetUserListAsync();
        Task<IDataResult<AdminAuthDTO>> GetAdminAuthAsync();
        Task<IDataResult<UserRolesDTO>> GetOtherRolesAsync(string id);
        Task<IResult> AddRoleToUserAsync(string id,string roleName);
        Task<IDataResult<UserRolesDTO>> GetRemoveRolesAsync(string id);
        Task<IResult> RemoveRoleFromUserAsync(string id,string roleName);
    }
}

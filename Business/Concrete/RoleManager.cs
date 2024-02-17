using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using Entities.DTOs.RoleDTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class RoleManager : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public RoleManager(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IResult> CreateRoleAsync(string roleName)
        {
            try
            {
                roleName = roleName.Trim();
                if (string.IsNullOrEmpty(roleName))
                    return new ErrorResult("Role is empty");
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                    return new ErrorResult("Role is already exist");

                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                    return new ErrorResult(string.Join(" ", result.Errors.Select(x => x.Description)));
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult("Something went wrong! Please try later");
            }
        }

        public async Task<IResult> DeleteRoleAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return new ErrorResult();
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return new ErrorResult();
                await _roleManager.DeleteAsync(role);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult();
            }
        }

        public IDataResult<List<RoleListDTO>> GetRoles()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();
                var map = _mapper.Map<List<RoleListDTO>>(roles);
                return new SuccessDataResult<List<RoleListDTO>>(map);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<RoleListDTO>>();
            }
        }
    }
}

using AutoMapper;
using Entities.Concrete;
using Entities.DTOs.RoleDTOs;
using Entities.DTOs.StockDTOs;
using Entities.DTOs.UserDTOs;
using Entities.DTOs.VerifyDTOs;
using Microsoft.AspNetCore.Identity;
namespace Business.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<RegisterDTO, User>();
            CreateMap<User, EditProfileDTO>().ReverseMap();
            CreateMap<User, AdminAuthDTO>();

            CreateMap<CheckVerifyDTO, VerificationCode>().ReverseMap();
            CreateMap<IdentityRole, RoleListDTO>();

            CreateMap<CreateStockDTO, Stock>();
        }
    }
}

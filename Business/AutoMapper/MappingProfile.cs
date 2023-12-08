using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using Entities.DTOs.VerifyDTOs;
namespace Business.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<RegisterDTO, User>();
            CreateMap<User, EditProfileDTO>().ReverseMap();

            CreateMap<CheckVerifyDTO, VerificationCode>().ReverseMap();
        }
    }
}

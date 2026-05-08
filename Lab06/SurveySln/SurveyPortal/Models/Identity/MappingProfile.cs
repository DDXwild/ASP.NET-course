using AutoMapper;
using SurveyPortal.Models.Identity.DTO;
using SurveyPortal.Models.Identity.Entities;

namespace SurveyPortal.Models.Identity
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserProfileDto>();
            CreateMap<UserProfileDto, User>();

            CreateMap<UserForRegistrationDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}

using AutoMapper;
using QNTM.API.Dtos;
using QNTM.API.Models;

namespace QNTM.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailDto>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>();
        }
    }
}
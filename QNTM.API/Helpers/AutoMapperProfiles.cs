using System.Linq;
using AutoMapper;
using QNTM.API.Dtos;
using QNTM.API.Models;

namespace QNTM.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForChatDto>().ForMember(
                dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                }
            );
            CreateMap<User, UserForDetailDto>().ForMember(
                dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                }
            );
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>();
            CreateMap<ChatForCreationDto, ActiveChat>();
            CreateMap<ChatForReturnDto, ActiveChat>();
            CreateMap<ActiveChat, ChatForCreationDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<Message, MessageDto>()
                    .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => 
                            src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                    .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => 
                            src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}
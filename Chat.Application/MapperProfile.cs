using AutoMapper;
using Chat.Application.Dto.User;
using Chat.Core.Entities.User;
using Chat.Infrastructure.Exceptions;

namespace Chat.Application;

public class MapperProfile:Profile
{
    public MapperProfile(
        )
    {
       CreateMap<UserInfo, UserInfoDto>()
            .ForMember(dest=>dest.SexName,l=>l.MapFrom(a=>a.Sex.GetEnumString()))
            .ForMember(dest=>dest.StatueName,l=>l.MapFrom(a=>a.Statue.GetEnumString()));
        CreateMap<UserInfoDto, UserInfo>();
    }
}

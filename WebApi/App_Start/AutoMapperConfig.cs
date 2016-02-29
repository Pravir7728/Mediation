using AutoMapper;
using Domain;
using WebApi.Infrastructure.Extensions;
using WebApi.Infrastructure.Handlers.Features.User;

#pragma warning disable 618

namespace WebApi
{
    public class AutoMapperConfig
    {
        public static void RegisterMapping()
        {
            Mapper.CreateMap<UserCreateModel, User>().Bidirectional();
            Mapper.CreateMap<GetUserResponse, User>().Bidirectional();
            Mapper.CreateMap<UserUpdateModel, User>().Bidirectional();
        }
    }
}
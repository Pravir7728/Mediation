using AutoMapper;
using Domain;
using WebApi.Extensions;
using WebApi.Handlers.Features.User;

#pragma warning disable 618

namespace WebApi
{
    public class AutoMapperConfig
    {
        public static void RegisterMapping()
        {
            Mapper.CreateMap<UserCreateModel, User>().Bidirectional();
        }
    }
}
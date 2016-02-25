using AutoMapper;
using Domain;
using WebApi.Extensions;
using WebApi.Features;
#pragma warning disable 618

namespace WebApi.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMapping()
        {
            Mapper.CreateMap<UserCreateModel, User>().Bidirectional();
        }
    }
}
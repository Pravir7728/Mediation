using AutoMapper;
using Domain;
using WebApi_Autofac.Extensions;
using WebApi_Autofac.Handlers;

namespace WebApi_Autofac.App_Start
{
    public class AutoMapperConfiguration
    {
        public static void RegisterMapping()
        {
#pragma warning disable 618
            Mapper.CreateMap<UserCreateModel, User>().Bidirectional();
#pragma warning restore 618
        }
    }
}
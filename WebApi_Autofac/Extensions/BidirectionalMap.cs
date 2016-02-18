using AutoMapper;

namespace WebApi_Autofac.Extensions
{
    public static class BidirectionalMap
    {
        public static void Bidirectional<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression)
        {
#pragma warning disable 618
            Mapper.CreateMap<TSource, TDestination>();
#pragma warning restore 618
#pragma warning disable 618
            Mapper.CreateMap<TSource, TDestination>().ReverseMap();
#pragma warning restore 618
        }
    }
}
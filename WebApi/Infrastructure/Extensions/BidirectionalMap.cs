using AutoMapper;

#pragma warning disable 618

namespace WebApi.Infrastructure.Extensions
{
    public static class BidirectionalMap
    {
        public static void Bidirectional<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression)
        {
            Mapper.CreateMap<TSource, TDestination>();
            Mapper.CreateMap<TSource, TDestination>().ReverseMap();
        }
    }
}
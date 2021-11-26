using AutoMapper;

namespace zero.Backoffice.Mapper;

public static class MapperExtensions
{
  public static Paged<TDestination> Map<TSource, TDestination>(this IMapper mapper, Paged<TSource> source)
  {
    return source.MapTo(srcItem => mapper.Map<TSource, TDestination>(srcItem));
  }

  public static Result<TDestination> Map<TSource, TDestination>(this IMapper mapper, Result<TSource> source)
  {
    TDestination model = mapper.Map<TSource, TDestination>(source.Model);
    return source.ConvertTo(model);
  }
}

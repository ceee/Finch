namespace zero.Mapper;

public static class MapperExtensions
{
  //public static TDestination Register<TSource, TDestination>(this IZeroMapper mapper, Action<TSource, TDestination, IZeroMapperContext> map) where TDestination : class, new()
  //{

  //}


  //public static TDestination Register<TSource, TDestination>(this IZeroMapper mapper, Action<TSource, TDestination, IZeroMapperContext> map, TDestination destination)
  //{

  //}


  public static TDestination Map<TSource, TDestination>(this IZeroMapper mapper, TSource source, TDestination destination)
  {
    return mapper.Map(source, typeof(TSource), destination);
  }

  public static TDestination Map<TSource, TDestination>(this IZeroMapper mapper, TSource source)
  {
    return mapper.Map<TDestination>(source, typeof(TSource), default);
  }

  public static Paged<TDestination> Map<TSource, TDestination>(this IZeroMapper mapper, Paged<TSource> source)
  {
    return source.MapTo(srcItem => mapper.Map<TSource, TDestination>(srcItem));
  }

  public static Result<TDestination> Map<TSource, TDestination>(this IZeroMapper mapper, Result<TSource> source)
  {
    TDestination model = mapper.Map<TSource, TDestination>(source.Model);
    return source.ConvertTo(model);
  }


  public static Dictionary<string, TDestination> Map<TSource, TDestination>(this IZeroMapper mapper, Dictionary<string, TSource> source)
  {
    Dictionary<string, TDestination> model = new();

    foreach ((string key, TSource sourceItem) in source)
    {
      model.Add(key, sourceItem == null ? default : mapper.Map<TSource, TDestination>(sourceItem));
    }

    return model;
  }
}

using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using System;
using zero.Core;
using zero.Web.Mapper;

namespace zero.Web
{
  public static class MapperServiceCollectionExtensions
  {
    public static IServiceCollection AddMapper<T>(this IServiceCollection services) where T : class, IMapper
    {
      return services.AddSingleton<IMapper, T>();
    }

    public static IServiceCollection AddMapper<T>(this IServiceCollection services, Action<T> options) where T : class, IMapper, new()
    {
      return services.AddSingleton<IMapper, T>(factory =>
      {
        T mapper = new T();
        options(mapper);
        return mapper;
      });
    }

    public static IServiceCollection AddMapper(this IServiceCollection services) => services.AddMapper<DefaultMapper>();

    public static IServiceCollection AddMapper(this IServiceCollection services, Action<DefaultMapper> options)
    {
      return services.AddSingleton<IMapper, DefaultMapper>(factory =>
      {
        DefaultMapper mapper = new DefaultMapper(factory.GetService<IDocumentStore>());
        options(mapper);
        return mapper;
      });
    }
  }
}

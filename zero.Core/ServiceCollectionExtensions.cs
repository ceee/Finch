using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using zero.Core.Api;
using zero.Core.Renderer;

namespace zero.Core.Entities
{
  public static class ServiceCollectionExtensions
  {
    public static void AddZeroCoreServices(this IServiceCollection services)
    {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

      services.AddAllByInterfaceTransient(typeof(IValidator), typeof(IValidator<>), assemblies, true);
      services.AddAllByInterfaceTransient(typeof(IRenderer), typeof(IRenderer<>), assemblies);

      services.AddTransient<IApplication, Application>(); 
      services.AddTransient<ICountry, Country>();
      services.AddTransient<ILanguage, Language>();
      services.AddTransient<ITranslation, Translation>();
      services.AddTransient<IPage, Page>();


      services.AddTransient(typeof(IApplicationsApi<>), typeof(ApplicationsApi<>));
      services.AddTransient(typeof(ICountriesApi<>), typeof(CountriesApi<>));
      services.AddTransient(typeof(ILanguagesApi<>), typeof(LanguagesApi<>));
      services.AddTransient(typeof(ITranslationsApi), typeof(TranslationsApi));
      services.AddTransient(typeof(ITranslationsApi<>), typeof(TranslationsApi<>));
      services.AddTransient(typeof(ITranslationsApiFacade), typeof(TranslationsApiFacade));
      services.AddTransient(typeof(IPagesApi<>), typeof(PagesApi<>));
      services.AddTransient(typeof(IPageTreeApi<>), typeof(PageTreeApi<>));
    }


    public static void AddAllByInterfaceTransient<TService, TImplementation>(this IServiceCollection services, Assembly[] assemblies)
    {
      services.AddAllByInterfaceTransient(typeof(TService), typeof(TImplementation), assemblies);
    }


    public static void AddAllByInterfaceTransient(this IServiceCollection services, Type serviceType, Type implementingType, Assembly[] assemblies, bool transient = false)
    {
      foreach (Assembly assembly in assemblies)
      {
        foreach (Type type in assembly.GetExportedTypes())
        {
          if (serviceType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract && !type.FullName.StartsWith("Fluent") && type.Name != "AbstractGenericRenderer")
          {
            Type service = type.GetInterfaces().FirstOrDefault(x => x.IsInterface && x.Name == implementingType.Name);

            if (service != null)
            {
              if (transient)
              {
                services.AddTransient(service, type);
              }
              else
              {
                services.AddScoped(service, type);
              }
            }
          }
        }
      }
    }
  }
}

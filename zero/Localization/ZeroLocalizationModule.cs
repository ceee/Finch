using FluentValidation;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace zero.Localization;

internal class ZeroLocalizationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    // hint: think about using https://github.com/nuages-io/nuages-localization
    
    ValidatorOptions.Global.LanguageManager.AddGermanOverrides();
    
    services.AddScoped<ICultureResolver, CultureResolver>();
    services.AddScoped<ICultureService, CultureService>();
    services.AddScoped<ILocalizer, ConfigurationLocalizer>();
    services.AddScoped<IStringLocalizer, StringLocalizer>();
    services.AddScoped(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
    
    services.Configure<RazorViewEngineOptions>(opts => opts.ViewLocationExpanders.Add(new LanguageViewLocationExpander(LanguageViewLocationExpanderFormat.Suffix)));
    services.AddSingleton<IHtmlLocalizerFactory, HtmlLocalizerFactory>();
    services.AddTransient<IHtmlLocalizer, HtmlLocalizer>();
    services.AddTransient<IViewLocalizer, ViewLocalizer>();

    services.AddLocalization();
    

    services.AddOptions<LocalizationOptions>().Bind(configuration.GetSection("Zero:Localization")).Configure(opts =>
    {
      opts.FilePath = "Config/texts.json";
    });
  }
}
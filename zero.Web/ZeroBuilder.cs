using Microsoft.Extensions.DependencyInjection;
using System;
using zero.Core;
using zero.Core.Entities;
using zero.Web.Sections;

namespace zero.Web
{
  public class ZeroBuilder
  {
    public virtual IServiceCollection Services { get; }

    public virtual ZeroOptions Options { get; }


    public ZeroBuilder(IServiceCollection services)
    {
      Services = services;
      Services.AddOptions<ZeroOptions>().Configure(opts => ConfigureDefaults(opts));

      //services.AddAuthorization(opts =>
      //{
      //  opts.AddPolicy("zero.sections.dashboard", builder =>
      //  {
      //    //builder.RequireClaim()
      //  });
      //});
    }


    void ConfigureDefaults(ZeroOptions opts)
    {
      opts.BackofficePath = "/zero";

      opts.Sections.Add<DashboardSection>();
      opts.Sections.Add<PagesSection>(); 
      opts.Sections.Add<ListsSection>();
      opts.Sections.Add<MediaSection>();
      opts.Sections.Add<SettingsSection>();

      SettingsGroup systemSettings = new SettingsGroup("@settings.groups.system");
      systemSettings.Add(Constants.SettingsAreas.Updates, "@settings.system.updates.name", "@settings.system.updates.text", "fth-check-circle");
      systemSettings.Add(Constants.SettingsAreas.Applications, "@settings.system.applications.name", "@settings.system.applications.text", "fth-layers");
      systemSettings.Add(Constants.SettingsAreas.Users, "@settings.system.users.name", "@settings.system.users.text", "fth-users");
      systemSettings.Add(Constants.SettingsAreas.Translations, "@settings.system.translations.name", "@settings.system.translations.text", "fth-type");
      systemSettings.Add(Constants.SettingsAreas.Countries, "@settings.system.countries.name", "@settings.system.countries.text", "fth-map-pin");
      systemSettings.Add(Constants.SettingsAreas.Logging, "@settings.system.logs.name", "@settings.system.logs.text", "fth-file-text");

      SettingsGroup pluginSettings = new SettingsGroup("@settings.groups.plugins");
      pluginSettings.Add("plugins", "@settings.plugins.installed.name", "@settings.plugins.installed.text", "fth-package");
      pluginSettings.Add("createplugin", "@settings.plugins.create.name", "@settings.plugins.create.text", "fth-box");

      opts.SettingsAreas.Add(systemSettings);
      opts.SettingsAreas.Add(pluginSettings);
    }


    public ZeroBuilder WithOptions(Action<ZeroOptions> configureOptions)
    {
      Services.PostConfigure(configureOptions);
      return this;
    }


    //public virtual AuthenticationBuilder AddPolicyScheme(string authenticationScheme, string displayName, Action<PolicySchemeOptions> configureOptions);
   
    //public virtual AuthenticationBuilder AddRemoteScheme<TOptions, THandler>(string authenticationScheme, string displayName, Action<TOptions> configureOptions)
    //  where TOptions : RemoteAuthenticationOptions, new()
    //  where THandler : RemoteAuthenticationHandler<TOptions>;
  }
}

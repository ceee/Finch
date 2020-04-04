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
    }


    void ConfigureDefaults(ZeroOptions opts)
    {
      opts.BackofficePath = "/zero";

      opts.Sections.Add<DashboardSection>();
      opts.Sections.Add<PagesSection>(); 
      opts.Sections.Add<ListsSection>();
      opts.Sections.Add<MediaSection>();
      opts.Sections.Add<SettingsSection>();
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

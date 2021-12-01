using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Endpoints.Mails;

public class MailModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IPermissionProvider, MailPermissions>();
    services.AddSingleton<IMapperProfile, MailMapperProfile>();

    services.Configure<RavenOptions>(opts =>
    {
      opts.Indexes.Add<zero_Api_MailTemplates_Listing>();
    });
  }
}
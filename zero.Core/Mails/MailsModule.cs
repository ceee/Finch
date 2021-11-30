using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Mails;

public class MailsModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IMailProvider, MailProvider>();
    services.AddScoped<IMailDispatcher, LoggerMailDispatcher>();
    services.AddScoped<IMailTemplatesStore, MailTemplatesStore>();
  }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Mails;

internal class ZeroMailModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IMailProvider, MailProvider>();
    services.AddScoped<IMailDispatcher, LoggerMailDispatcher>();
    services.AddScoped<IMailTemplatesStore, MailTemplatesStore>();

    services.AddOptions<MailOptions>().Configure(opts =>
    {
      opts.BuildViewPath = mail => $"~/Views/Mails/{mail.Template.Key.Replace('.', '/')}.cshtml";
    });
  }
}
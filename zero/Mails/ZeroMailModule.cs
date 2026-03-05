using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Mails;

internal class ZeroMailModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IMailProvider, MailProvider>();
    //services.AddScoped<IMailDispatcher, LoggerMailDispatcher>();
    services.AddScoped<IMailDispatcher, PostmarkDispatcher>();

    services.AddOptions<MailOptions>().Bind(configuration.GetSection("Zero:Mails")).Configure(opts =>
    {
      opts.BuildViewPath = mail => $"~/Mails/{mail.ViewKey.Replace('.', '/')}.cshtml";
    });
  }
}
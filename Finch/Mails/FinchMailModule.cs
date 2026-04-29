using Finch.Mails.Dispatchers;
using Finch.Mails.Dispatchers.Postmark;
using Finch.Mails.Dispatchers.Scaleway;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finch.Mails;

internal class FinchMailModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddHttpClient<ScalewayDispatcher>().RemoveAllLoggers();
    services.AddScoped<IMailProvider, MailProvider>();

    // use logger mail dispatcher as default implementation
    // to use other dispatchers .AddMailDispatcher() should be used
    services.AddScoped<IMailDispatcher, LoggerMailDispatcher>();

    services.AddOptions<MailOptions>().Bind(configuration.GetSection("Finch:Mails")).Configure(opts =>
    {
      opts.BuildViewPath = mail => $"~/Mails/{mail.ViewKey.Replace('.', '/')}.cshtml";
    });
  }
}
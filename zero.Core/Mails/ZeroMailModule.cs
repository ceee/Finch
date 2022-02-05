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

    services.Configure<ZeroSearchOptions>(opts =>
    {
      opts.Map<MailTemplate>("fth-mail").Fields("Key").Display((x, res) =>
      {
        res.Description = x.Key;
        res.Url = "/settings/mailtemplates/edit/" + x.Id;
      });
    });
  }
}
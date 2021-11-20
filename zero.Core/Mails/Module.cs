using Microsoft.Extensions.DependencyInjection;

namespace zero.Mails;

internal class MailsModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddScoped<IMailProvider, MailProvider>();
    config.Services.AddScoped<IMailDispatcher, LoggerMailDispatcher>();
  }
}
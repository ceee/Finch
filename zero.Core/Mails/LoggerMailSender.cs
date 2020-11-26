using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace zero.Core.Mails
{
  /// <summary>
  /// Default implementation of an IMailSender which sends the mail to the attached logger
  /// and therefore not using the SMTP channel.
  /// Implementing real mail sending is up to the consuming application.
  /// </summary>
  public class LoggerMailSender : IMailSender
  {
    protected ILogger<LoggerMailSender> Logger { get; set; }


    public LoggerMailSender(ILogger<LoggerMailSender> logger)
    {
      Logger = logger;
    }


    public Task Send(Mail message, CancellationToken token = default)
    {
      Logger.LogInformation("Mail to {to}. Subject: {subject}", message.To[0].Address, message.Subject);
      return Task.CompletedTask;
    }
  }
}

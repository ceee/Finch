using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
  public class LoggerMailDispatcher : IMailDispatcher
  {
    protected Queue<Mail> Queue { get; private set; } = new Queue<Mail>();

    protected ILogger<LoggerMailDispatcher> Logger { get; set; }


    public LoggerMailDispatcher(ILogger<LoggerMailDispatcher> logger)
    {
      Logger = logger;
    }


    /// <inheritdoc />
    public void Enqueue(Mail message)
    {
      Queue.Enqueue(message);
    }


    /// <inheritdoc />
    public Task Send(CancellationToken token = default)
    {
      while (Queue.Count > 0)
      {
        Mail message = Queue.Dequeue();
        Logger.LogInformation("Mail to {to}. Subject: {subject}", message.To[0].Address, message.Subject);
      }

      return Task.CompletedTask;
    }


    /// <inheritdoc />
    public void Dispose() { }
  }
}

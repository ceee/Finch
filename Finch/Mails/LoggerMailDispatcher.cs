using Microsoft.Extensions.Logging;

namespace Finch.Mails;

/// <summary>
/// Default implementation of an IMailSender which sends the mail to the attached logger
/// and therefore not using the SMTP channel.
/// Implementing real mail sending is up to the consuming application.
/// </summary>
public class LoggerMailDispatcher(ILogger<LoggerMailDispatcher> logger) : IMailDispatcher
{
  /// <inheritdoc />
  public int Priority { get; } = -10;

  /// <inheritdoc />
  public bool CanSend() => true;


  /// <inheritdoc />
  public Task Send(Mail message, CancellationToken token = default)
  {
    logger.LogInformation("Mail to {to}. Subject: {subject}", message.To[0].Address, message.Subject);
    return Task.CompletedTask;
  }


  /// <inheritdoc />
  public void Dispose() { }
}
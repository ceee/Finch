namespace Finch.Mails;

public interface IMailDispatcher : IDisposable
{
  /// <summary>
  /// If multiple dispatchers are available, the dispatcher with the highest priority will be used
  /// </summary>
  int Priority { get; }

  /// <summary>
  /// Whether this dispatcher is properly configured and can send mails
  /// </summary>
  bool CanSend();

  /// <summary>
  /// Sends a mail message
  /// </summary>
  Task Send(Mail message, CancellationToken token = default);

  /// <summary>
  /// Whether a certain sender signature is supported by this dispatcher
  /// </summary>
  Task<bool> IsSenderSupported(string email, CancellationToken token = default) => Task.FromResult(true);
}
namespace Mixtape.Mails.Dispatchers;

public interface IMailDispatcher : IDisposable
{
  /// <summary>
  /// Sends a mail message
  /// </summary>
  Task Send(Mail message, CancellationToken token = default);

  /// <summary>
  /// Whether a certain sender signature is supported by this dispatcher
  /// </summary>
  Task<bool> IsSenderSupported(string email, CancellationToken token = default) => Task.FromResult(true);
}
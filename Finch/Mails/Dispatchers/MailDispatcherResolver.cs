namespace Finch.Mails.Dispatchers;

public class MailDispatcherResolver(IEnumerable<IMailDispatcher> dispatchers) : IMailDispatcherResolver
{
  /// <inheritdoc />
  public Task<IMailDispatcher> Resolve()
  {
    IMailDispatcher dispatcher = dispatchers
      .OrderByDescending(x => x.Priority)
      .FirstOrDefault(dispatcher => dispatcher.CanSend());

    return Task.FromResult(dispatcher);
  }
}


public interface IMailDispatcherResolver
{
  /// <summary>
  /// Resolves a mail dispatcher to use for sending emails
  /// </summary>
  Task<IMailDispatcher> Resolve();
}
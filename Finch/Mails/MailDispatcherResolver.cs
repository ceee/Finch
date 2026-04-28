using Microsoft.Extensions.Options;

namespace Finch.Mails;

public class MailDispatcherResolver(IEnumerable<IMailDispatcher> dispatchers, IOptionsMonitor<MailOptions> options)
{
  /// <inheritdoc />
  public IMailDispatcher Resolve()
  {
    return dispatchers
      .OrderByDescending(x => x.Priority)
      .FirstOrDefault(dispatcher => dispatcher.CanSend());
  }
}
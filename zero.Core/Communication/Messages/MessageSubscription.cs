using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace zero.Communication;

internal class MessageSubscription<TMessage, TMessageHandler> : IMessageSubscription
  where TMessage : class, IMessage
  where TMessageHandler : IMessageHandler<TMessage>
{
  public bool CanDeliver<T>()
  {
    return typeof(TMessage).GetTypeInfo().IsAssignableFrom(typeof(T));
  }

  public async Task Deliver(IServiceProvider serviceProvider, object message)
  {
    if (message == null)
    {
      throw new ArgumentNullException(nameof(message));
    }
    if (!(message is TMessage))
    {
      throw new ArgumentException($"{ nameof(message) } must be of type '{typeof(TMessage).FullName}'");
    }

    var handler = serviceProvider.GetRequiredService<TMessageHandler>();
    await handler.Handle((TMessage)message);
  }
}


public interface IMessageSubscription
{
  bool CanDeliver<TMessage>();

  Task Deliver(IServiceProvider serviceProvider, object message);
}

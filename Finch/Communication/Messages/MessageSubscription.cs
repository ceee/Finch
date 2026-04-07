using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

namespace Finch.Communication;

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

    var handlers = serviceProvider.GetServices<TMessageHandler>();

    foreach (var handler in handlers)
    {
      await handler.Handle((TMessage)message);
    }
  }
}


internal class InlineMessageSubscription<TMessage> : IMessageSubscription
  where TMessage : class, IMessage
{
  private Expression<Func<TMessage, Task>> _handler;

  public InlineMessageSubscription(Expression<Func<TMessage, Task>> handler)
  {
    _handler = handler;
  }


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
      throw new ArgumentException($"{nameof(message)} must be of type '{typeof(TMessage).FullName}'");
    }

    await _handler.Compile()((TMessage)message);
  }
}


public interface IMessageSubscription
{
  bool CanDeliver<TMessage>();

  Task Deliver(IServiceProvider serviceProvider, object message);
}

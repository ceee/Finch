namespace Mixtape.Communication;

/// <summary>
/// Indicates a handler that can perform an action when a message of 
/// a certain type is received. (could be an interface rather than concrete type)
/// </summary>
public interface IMessageHandler<TMessage> where TMessage : IMessage
{
  /// <summary>
  /// Method to invoke when a message of type TMessage is published 
  /// </summary>
  Task Handle(TMessage message);
}


/// <summary>
/// Indicates a handler that can perform an action when a message of 
/// a certain type is received. (could be an interface rather than concrete type)
/// </summary>
public interface IBatchMessageHandler<TMessage> : IMessageHandler<TMessage> where TMessage : IMessage
{
  /// <summary>
  /// Method to invoke when a batch of messages of type TMessage are published
  /// </summary>
  Task HandleBatchAsync(IReadOnlyCollection<TMessage> message);
}

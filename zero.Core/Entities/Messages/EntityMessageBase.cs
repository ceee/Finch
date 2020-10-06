using Raven.Client.Documents.Session;
using zero.Core.Messages;

namespace zero.Core.Entities.Messages
{
  public abstract class EntityMessageBase<T> : IMessage
  {
    public IAsyncDocumentSession Session { get; set; }

    public string Id { get; set; }

    public T Model { get; set; }
  }
}

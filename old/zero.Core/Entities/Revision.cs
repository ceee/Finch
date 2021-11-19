using System;

namespace zero.Core.Entities
{
  public class Revision<T> : Revision where T : ZeroEntity
  {
    public T Model { get; set; }
  }

  public class Revision
  {
    public string ModelId { get; set; }

    public RevisionUser User { get; set; }

    public DateTimeOffset Date { get; set; }

    public string ChangeVector { get; set; }
  }

  public class RevisionUser
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string AvatarId { get; set; }
  }
}

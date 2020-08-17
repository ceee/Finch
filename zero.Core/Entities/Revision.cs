using System;

namespace zero.Core.Entities
{
  public class Revision
  {
    public RevisionUser User { get; set; }

    public DateTimeOffset Date { get; set; }

    public string ChangeVector { get; set; }

    public string Json { get; set; }
  }

  public class RevisionUser
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string AvatarId { get; set; }
  }
}

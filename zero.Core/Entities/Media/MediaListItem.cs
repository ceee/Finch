using System;

namespace zero.Core.Entities
{
  public class MediaListItem : IZeroIdEntity, IAppAwareEntity, IZeroDbConventions
  {
    public string Id { get; set; }

    public string ParentId { get; set; }

    public string AppId { get; set; }

    public string Name { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public bool IsFolder { get; set; }

    public string Image { get; set; }

    public long Size { get; set; }

    public bool HasTransparency { get; set; }
  }
}

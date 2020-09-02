using System;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
  /// <inheritdoc />
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
  }
}

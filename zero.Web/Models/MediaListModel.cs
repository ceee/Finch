using zero.Core.Entities;

namespace zero.Web.Models
{
  public class MediaListModel : ListModel
  {
    public bool IsFolder { get; set; }

    public MediaType Type { get; set; }

    public string Name { get; set; }

    public string Source { get; set; }

    public string ThumbnailSource { get; set; }

    public long Size { get; set; }
  }
}

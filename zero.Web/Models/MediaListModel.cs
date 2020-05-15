namespace zero.Web.Models
{
  public class MediaListModel : ListModel
  {
    public bool IsFolder { get; set; }

    public string Name { get; set; }

    public string Source { get; set; }

    public string ThumbnailSource { get; set; }

    public int Size { get; set; }
  }
}

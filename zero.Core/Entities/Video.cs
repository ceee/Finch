namespace zero.Core.Entities
{
  public class Video
  {
    public VideoProvider Provider { get; set; }

    public string VideoId { get; set; }

    public string VideoUrl { get; set; }

    public string VideoPreviewImageUrl { get; set; }

    public string Title { get; set; }

    public string PreviewImageId { get; set; }
  }


  public enum VideoProvider
  {
    Html,
    Youtube,
    Vimeo
  }
}

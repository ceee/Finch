namespace zero.Core.Entities
{
  public class MediaListQuery : ListQuery<Media>
  {
    public string FolderId { get; set; }
  }
}

namespace zero.Core.Entities
{
  public class MediaListQuery : ListQuery<Media, MediaListFilter>
  {

  }


  public class MediaListFilter : IListSpecificQuery
  {
    public string FolderId { get; set; }
  }
}

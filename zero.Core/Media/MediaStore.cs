namespace zero.Media;

public class MediaStore : TreeEntityStore<Media>, IMediaStore
{
  public MediaStore(IStoreContext context) : base(context)
  {
    
  }

  /// <summary>
  /// A media (either file or folder) can only be saved for the following circumstances:
  /// 1. The parent is null (=root)
  /// 2. The parent is a folder
  /// </summary>
  public override async Task<bool> IsAllowedAsChild(Media model, string parentId)
  {
    if (parentId.IsNullOrEmpty())
    {
      return true;
    }

    Media parent = await Load(parentId);
    return parent != null && parent.Type == MediaType.Folder;
  }
}


public interface IMediaStore : ITreeEntityStore<Media>
{
}
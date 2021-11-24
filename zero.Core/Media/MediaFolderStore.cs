namespace zero.Media;

public class MediaFolderStore : EntityStore<MediaFolder>, IMediaFolderStore
{
  public MediaFolderStore(IStoreContext context) : base(context)
  {
    
  }
}


public interface IMediaFolderStore : IEntityStore<MediaFolder>
{
}
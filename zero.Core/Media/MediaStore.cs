namespace zero.Media;

public class MediaStore : EntityStore<Media>, IMediaStore
{
  public MediaStore(IStoreContext context) : base(context)
  {
    
  }
}


public interface IMediaStore : IEntityStore<Media>
{
}
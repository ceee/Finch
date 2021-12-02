namespace zero.Spaces;

public class SpaceStore : EntityStore<Space>, ISpaceStore
{
  public SpaceStore(IStoreContext context) : base(context)
  {
    
  }
}


public interface ISpaceStore : IEntityStore<Space>
{
  
}
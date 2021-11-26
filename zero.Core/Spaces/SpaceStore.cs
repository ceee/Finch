namespace zero.Spaces;

public class SpaceStore : EntityStore<Space>, ISpaceStore
{
  protected ISpaceTypeService SpaceTypes { get; private set; }


  public SpaceStore(IStoreContext context, ISpaceTypeService spaceTypes) : base(context)
  {
    SpaceTypes = spaceTypes;
  }


  /// <inheritdoc />
  public async Task<Space> Empty(string spaceType)
  {
    SpaceType type = SpaceTypes.GetByAlias(spaceType);

    if (type == null)
    {
      return null;
    }

    Space model = await Empty();
    model.SpaceTypeAlias = type.Alias;

    return model;
  }
}


public interface ISpaceStore : IEntityStore<Space>
{
  /// <summary>
  /// Get new instance of an entity for a specific Space type
  /// </summary>
  Task<Space> Empty(string spaceType);
}
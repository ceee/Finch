namespace zero.Spaces;

public class SpaceTypeService : ISpaceTypeService
{
  protected IZeroContext Context { get; private set; }

  protected IHandlerHolder Handler { get; private set; }

  protected FlavorOptions Flavors { get; set; }


  public SpaceTypeService(IZeroContext context, IZeroOptions options, IHandlerHolder handler)
  {
    Context = context;
    Handler = handler;
    Flavors = options.For<FlavorOptions>();
  }


  /// <inheritdoc />
  public IList<SpaceType> GetAll()
  {
    return Flavors.GetAll<Space>().Select(x => (SpaceType)x).ToList();
  }


  /// <inheritdoc />
  public SpaceType GetByAlias(string spaceTypeAlias)
  {
    return Flavors.Get<Space>(spaceTypeAlias) as SpaceType;
  }


  /// <inheritdoc />
  public SpaceType GetByType<T>() where T : Space
  {
    return Flavors.Get<Space, T>() as SpaceType;
  }
}


public interface ISpaceTypeService
{
  /// <summary>
  /// Get all available space types
  /// </summary>
  IList<SpaceType> GetAll();

  /// <summary>
  /// Get a specific space type by alias
  /// </summary>
  SpaceType GetByAlias(string spaceTypeAlias);

  /// <summary>
  /// Get a specific space type by space type. funny, I know.
  /// </summary>
  SpaceType GetByType<T>() where T : Space;
}

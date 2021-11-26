namespace zero.Spaces;

public class SpaceTypeService : ISpaceTypeService
{
  protected IZeroContext Context { get; private set; }

  protected IHandlerHolder Handler { get; private set; }

  protected SpaceOptions SpaceOptions { get; set; }


  public SpaceTypeService(IZeroContext context, IZeroOptions options, IHandlerHolder handler)
  {
    Context = context;
    Handler = handler;
    SpaceOptions = options.For<SpaceOptions>();
  }


  /// <inheritdoc />
  public IList<SpaceType> GetAll()
  {
    return SpaceOptions;
  }


  /// <inheritdoc />
  public SpaceType GetByAlias(string pageTypeAlias)
  {
    return SpaceOptions.FirstOrDefault(x => x.Alias == pageTypeAlias);
  }


  /// <inheritdoc />
  public SpaceType GetByType<T>() where T : Space
  {
    Type type = typeof(T);
    return SpaceOptions.FirstOrDefault(x => x.Type == type);
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

namespace zero.Configuration;

public class IntegrationTypeService : IIntegrationTypeService
{
  protected IZeroContext Context { get; private set; }

  protected IHandlerHolder Handler { get; private set; }

  protected FlavorOptions Flavors { get; set; }


  public IntegrationTypeService(IZeroContext context, IZeroOptions options, IHandlerHolder handler)
  {
    Context = context;
    Handler = handler;
    Flavors = options.For<FlavorOptions>();
  }


  /// <inheritdoc />
  public IEnumerable<IntegrationType> GetAll()
  {
    return Flavors.GetAll<Integration>().Select(x => (IntegrationType)x);
  }


  /// <inheritdoc />
  public IntegrationType GetByAlias(string integrationTypeAlias)
  {
    return Flavors.Get<Integration>(integrationTypeAlias) as IntegrationType;
  }


  /// <inheritdoc />
  public IntegrationType GetByType<T>() where T : Integration
  {
    return Flavors.Get<Integration, T>() as IntegrationType;
  }


  /// <inheritdoc />
  public IEnumerable<IntegrationType> GetByTag(string tag)
  {
    return GetAll().Where(x => x.Tags.Contains(tag, StringComparer.InvariantCultureIgnoreCase));
  }
}


public interface IIntegrationTypeService
{
  /// <summary>
  /// Get all available integration types
  /// </summary>
  IEnumerable<IntegrationType> GetAll();

  /// <summary>
  /// Get a specific integration type by alias
  /// </summary>
  IntegrationType GetByAlias(string integrationTypeAlias);

  /// <summary>
  /// Get a specific integration type by model type.
  /// </summary>
  IntegrationType GetByType<T>() where T : Integration;

  /// <summary>
  /// Get all integrations with a certain tag.
  /// </summary>
  IEnumerable<IntegrationType> GetByTag(string tag);
}

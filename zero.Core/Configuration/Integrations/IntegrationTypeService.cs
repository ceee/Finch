namespace zero.Configuration;

public class IntegrationTypeService : IIntegrationTypeService
{
  protected IZeroContext Context { get; private set; }

  protected IHandlerHolder Handler { get; private set; }

  protected IntegrationOptions Types { get; set; }


  public IntegrationTypeService(IZeroContext context, IZeroOptions options, IHandlerHolder handler)
  {
    Context = context;
    Handler = handler;
    Types = options.For<IntegrationOptions>();
  }


  /// <inheritdoc />
  public IEnumerable<IntegrationType> GetAll()
  {
    return Types;
  }


  /// <inheritdoc />
  public IntegrationType GetByAlias(string integrationTypeAlias)
  {
    return Types.FirstOrDefault(x => x.Alias == integrationTypeAlias);
  }


  /// <inheritdoc />
  public IntegrationType GetByType<T>() where T : Integration
  {
    Type type = typeof(T);
    return Types.FirstOrDefault(x => x.ModelType == type);
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
}

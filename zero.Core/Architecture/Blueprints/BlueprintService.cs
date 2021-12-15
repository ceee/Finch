namespace zero.Architecture;

public class BlueprintService : IBlueprintService
{
  protected IZeroOptions Options { get; set; }

  protected IReadOnlyCollection<Blueprint> Blueprints { get; set; }


  public BlueprintService(IZeroOptions options)
  {
    Options = options;
    Blueprints = Options.For<BlueprintOptions>();
  }


  /// <inheritdoc />
  public bool IsEnabled<T>(T model) => IsEnabled(model.GetType());


  /// <inheritdoc />
  public bool IsEnabled<T>()=> IsEnabled(typeof(T));


  /// <inheritdoc />
  public bool IsEnabled(Type type) => Blueprints.Any(x => x.ContentType.IsAssignableFrom(type));


  /// <inheritdoc />
  public bool TryGetBlueprint<T>(T model, out Blueprint blueprint) => TryGetBlueprint(model.GetType(), out blueprint);


  /// <inheritdoc />
  public bool TryGetBlueprint<T>(out Blueprint blueprint) => TryGetBlueprint(typeof(T), out blueprint);


  /// <inheritdoc />
  public bool TryGetBlueprint(Type type, out Blueprint blueprint)
  {
    blueprint = Blueprints.FirstOrDefault(x => x.ContentType.IsAssignableFrom(type));
    return blueprint != null;
  }

  /// <inheritdoc />
  public Dictionary<Type, Blueprint> GetAllBlueprints()
  {
    return Blueprints.ToDictionary(x => x.ContentType, x => x);
  }
}


public interface IBlueprintService
{
  /// <summary>
  /// Check whether blueprinting functionality is enabled for a certain entity
  /// </summary>
  bool IsEnabled<T>();

  /// <summary>
  /// Check whether blueprinting functionality is enabled for a certain entity
  /// </summary>
  bool IsEnabled<T>(T model);

  bool IsEnabled(Type type);

  bool TryGetBlueprint<T>(T model, out Blueprint blueprint);

  bool TryGetBlueprint<T>(out Blueprint blueprint);

  bool TryGetBlueprint(Type type, out Blueprint blueprint);

  /// <summary>
  /// Get all registered blueprint configurations
  /// </summary>
  Dictionary<Type, Blueprint> GetAllBlueprints();
}

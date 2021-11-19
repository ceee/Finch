using System;
using System.Collections.Generic;
using System.Linq;

namespace zero;

public class BlueprintService : IBlueprintService
{
  protected IZeroOptions Options { get; set; }

  protected IReadOnlyCollection<Blueprint> Blueprints { get; set; }


  public BlueprintService(IZeroOptions options)
  {
    Options = options;
    Blueprints = Options.Blueprints.GetAllItems();
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
}

namespace zero.Architecture;

public enum BlueprintStatus
{
  /// <summary>
  /// This entity is standalone
  /// </summary>
  Standalone = 0,
  /// <summary>
  /// This entity is a blueprint
  /// </summary>
  Parent = 1,
  /// <summary>
  /// This entity is a child of blueprint
  /// </summary>
  Child = 2
}

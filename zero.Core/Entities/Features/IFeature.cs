namespace zero.Core.Entities
{
  /// <summary>
  /// A feature can affect both the backoffice and the frontend
  /// </summary>
  public interface IFeature
  {
    /// <summary>
    /// The alias
    /// </summary>
    string Alias { get; }

    /// <summary>
    /// The name of the feature
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Additional description
    /// </summary>
    string Description { get; }
  }
}

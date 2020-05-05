namespace zero.Core.Entities
{
  /// <inheritdoc />
  public class Feature : IFeature
  {
    /// <inheritdoc />
    public string Alias { get; set; }

    /// <inheritdoc />
    public string Name { get; set; }

    /// <inheritdoc />
    public string Description { get; set; }
  }
}

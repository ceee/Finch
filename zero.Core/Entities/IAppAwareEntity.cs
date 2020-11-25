namespace zero.Core.Entities
{
  /// <summary>
  /// By default most entities are app aware and stored in their own database context.
  /// Exceptions (like media) are stored in the core database and need to be marked as app aware
  /// </summary>
  public interface IAppAwareEntity
  {
    /// <summary>
    /// Associated app id of the entity
    /// </summary>
    string AppId { get; set; }
  }
}

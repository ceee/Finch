namespace zero.Core.Entities
{
  public interface IZeroTypedEntity : IZeroEntity
  {
    /// <summary>
    /// Alias of the used type
    /// </summary>
    string TypeAlias { get; set; }
  }
}

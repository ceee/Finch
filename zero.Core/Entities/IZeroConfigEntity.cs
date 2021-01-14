namespace zero.Core.Entities
{
  public interface IZeroConfigEntity : IZeroEntity
  {
    /// <summary>
    /// Alias of the used type
    /// </summary>
    string TypeAlias { get; set; }
  }
}

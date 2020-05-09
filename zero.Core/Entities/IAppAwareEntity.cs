namespace zero.Core.Entities
{
  public interface IAppAwareEntity
  {
    /// <summary>
    /// Id of the associated application (auto-filled)
    /// </summary>
    string AppId { get; set; }
  }
}

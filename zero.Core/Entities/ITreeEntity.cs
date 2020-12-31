namespace zero.Core.Entities
{
  public interface ITreeEntity : IZeroEntity
  {
    /// <summary>
    /// Id of the parent entity
    /// </summary>
    string ParentId { get; set; }
  }
}

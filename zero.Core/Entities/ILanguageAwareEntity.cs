namespace zero.Core.Entities
{
  public interface ILanguageAwareEntity
  {
    ///// <summary>
    ///// Contains the parent entity in case this is a language variant
    ///// </summary>
    //public string ParentEntityId { get; set; }

    /// <summary>
    /// Language of the entity
    /// </summary>
    string LanguageId { get; set; }
  }
}

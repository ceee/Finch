using System;
using System.Diagnostics;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
  public interface ILanguageAwareEntity
  {
    /// <summary>
    /// Language of the entity
    /// </summary>
    string LanguageId { get; set; }
  }
}

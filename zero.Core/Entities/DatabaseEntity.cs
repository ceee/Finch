using System;
using System.Collections.Generic;
using System.Diagnostics;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
  [DebuggerDisplay("Id = {Id,nq}, Name = {Name}, Alias = {Alias}")]
  public abstract class DatabaseEntity : IDatabaseEntity
  {
    /// <inheritdoc/>
    public string Id { get; set; }

    /// <inheritdoc/>
    [MapAppId]
    public string AppId { get; set; }

    /// <inheritdoc/>
    [Inherit]
    public string Name { get; set; }

    /// <inheritdoc/>
    public string Alias { get; set; }

    /// <inheritdoc/>
    public uint Sort { get; set; }

    /// <inheritdoc/>
    public bool IsActive { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset CreatedDate { get; set; }

    /// <inheritdoc/>
    public IList<LanguageVariant> LanguageVariants { get; set; } = new List<LanguageVariant>();

    /// <inheritdoc />
    public string LanguageId { get; set; }
  }


  public interface IZeroEntity
  {
    /// <summary>
    /// Id of the entity
    /// </summary>
    string Id { get; set; }
  }


  public interface IDatabaseEntity : IZeroEntity
  {
    /// <summary>
    /// Id of the associated application (auto-filled)
    /// </summary>
    string AppId { get; set; }

    /// <summary>
    /// Full name of the entity
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Unique alias which can be used in the frontend and URLs
    /// As generating aliases from the name would not be unique we append an incremental number if the desired alias is not available anymore
    /// </summary>
    string Alias { get; set; }

    /// <summary>
    /// Sort order
    /// </summary>
    uint Sort { get; set; }

    /// <summary>
    /// Whether the entity is visible in the frontend
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Date of creation
    /// </summary>
    DateTimeOffset CreatedDate { get; set; }
  }
}

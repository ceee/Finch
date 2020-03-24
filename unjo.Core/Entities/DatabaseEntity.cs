using System;
using System.Diagnostics;
using unjo.Core.Attributes;

namespace unjo.Core.Entities
{
  [DebuggerDisplay("Id = {Id,nq}, Name = {Name}")]
  public abstract class DatabaseEntity : IDatabaseEntity
  {
    /// <inheritdoc/>
    [GenerateId]
    public string Id { get; set; }

    /// <inheritdoc/>
    [MapAppId]
    public string AppId { get; set; }

    /// <inheritdoc/>
    public string Name { get; set; }

    /// <inheritdoc/>
    public uint Sort { get; set; }

    /// <inheritdoc/>
    public bool IsActive { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset CreatedDate { get; set; }
  }


  public interface IDatabaseEntity
  {
    /// <summary>
    /// Id of the entity
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Id of the associated application (auto-filled)
    /// </summary>
    string AppId { get; set; }

    /// <summary>
    /// Full name of the entity
    /// </summary>
    string Name { get; set; }

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

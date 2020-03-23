using System;
using System.Diagnostics;
using unjo.Core.Attributes;

namespace unjo.Core.Entities
{
  [DebuggerDisplay("Id = {Id,nq}, Name = {Name}")]
  public abstract class DatabaseEntity
  {
    /// <summary>
    /// Id of the entity
    /// </summary>
    [GenerateId]
    public string Id { get; set; }

    /// <summary>
    /// Id of the associated application (auto-filled)
    /// </summary>
    [MapAppId]
    public string AppId { get; set; }

    /// <summary>
    /// Full name of the entity
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Sort order
    /// </summary>
    public uint Sort { get; set; }

    /// <summary>
    /// Whether the entity is visible in the frontend
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Date of creation
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; }
  }
}

using System;
using System.Diagnostics;

namespace zero.Core.Entities
{
  [DebuggerDisplay("Id = {Id,nq}, Name = {Name}, Alias = {Alias}")]
  public abstract class ZeroEntity : IZeroEntity
  {
    /// <inheritdoc/>
    public string Id { get; set; }

    /// <inheritdoc/>
    public string Name { get; set; }

    /// <inheritdoc/>
    public string Alias { get; set; }

    /// <inheritdoc/>
    public uint Sort { get; set; }

    /// <inheritdoc/>
    public bool IsActive { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset CreatedDate { get; set; }
  }


  public interface IZeroEntity : IZeroIdEntity
  {
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

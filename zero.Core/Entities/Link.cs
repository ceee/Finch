using System.Collections.Generic;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
  public class Link : ZeroEntity, ILink
  {
    /// <inheritdoc />
    public string AppId { get; set; }

    /// <inheritdoc />
    public string OriginalId { get; set; }

    /// <inheritdoc />
    public string Collection { get; set; }

    /// <inheritdoc />
    public string Url { get; set; }

    /// <inheritdoc />
    public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
  }


  [Collection("Links")]
  public interface ILink : IZeroEntity, IAppAwareEntity, IZeroDbConventions
  {
    /// <summary>
    /// Id of the associated entity
    /// </summary>
    string OriginalId { get; set; }

    /// <summary>
    /// Collection of the associated entity
    /// </summary>
    string Collection { get; set; }

    /// <summary>
    /// Country code (ISO 3166-1)
    /// </summary>
    string Url { get; set; }

    /// <summary>
    /// Optional parameters
    /// </summary>
    Dictionary<string, string> Params { get; set; }
  }
}

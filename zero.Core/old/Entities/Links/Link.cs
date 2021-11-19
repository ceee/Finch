using Newtonsoft.Json;
using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class Link
  {
    public string Area { get; set; }

    public LinkTarget Target { get; set; }

    public string UrlSuffix { get; set; }

    public string Title { get; set; }

    public Dictionary<string, object> Values { get; set; } = new();

    /// <summary>
    /// [Warning] This field is always empty when bound to the database.
    /// It is only filled in the app-code for routing.
    /// </summary>
    [JsonIgnore]
    public string Url { get; set; }
  }

  public enum LinkTarget
  {
    Default = 0,
    Self = 1,
    Blank = 2
  }
}

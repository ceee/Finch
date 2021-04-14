using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class Link : ILink
  {
    public string Area { get; set; }

    public LinkTarget Target { get; set; }

    public string UrlSuffix { get; set; }

    public string Title { get; set; }

    public Dictionary<string, object> Values { get; set; } = new();

    /// <inheritdoc />
    [JsonIgnore]
    public string Url { get; set; }
  }

  public enum LinkTarget
  {
    Default = 0,
    Self = 1,
    Blank = 2
  }


  public interface ILink
  {
    string Area { get; set; }

    LinkTarget Target { get; set; }

    string UrlSuffix { get; set; }

    string Title { get; set; }

    Dictionary<string, object> Values { get; set; }

    /// <summary>
    /// [Warning] This field is always empty when bound to the database.
    /// It is only filled in the app-code for routing.
    /// </summary>
    [JsonIgnore]
    public string Url { get; set; }
  }
}

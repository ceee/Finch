using System;
using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class Link : ILink
  {
    public string Area { get; set; }

    public LinkTarget Target { get; set; }

    public string UrlSuffix { get; set; }

    public string Label { get; set; }

    public string Title { get; set; }

    public Dictionary<string, object> Values { get; set; } = new();
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

    string Label { get; set; }

    string Title { get; set; }

    Dictionary<string, object> Values { get; set; }
  }
}

using System;
using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class Link
  {
    public string ProviderAlias { get; set; }

    public LinkTarget Target { get; set; }

    public string UrlSuffix { get; set; }

    public string Title { get; set; }

    public Dictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
  }

  public enum LinkTarget
  {
    Default = 0,
    Self = 1,
    Blank = 2
  }
}

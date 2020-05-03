using Newtonsoft.Json;
using System;

namespace zero.Core.Entities
{
  /// <summary>
  /// A list item can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  public class SpaceContent : DatabaseEntity
  {
    public string SpaceAlias { get; set; }
  }
}
using System;
using zero.Core.Entities;

namespace zero.TestData
{
  public class SpaceAttribute : Attribute
  {
    public string Alias { get; set; }

    public SpaceView View { get; set; }

    public string ComponentPath { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Icon { get; set; }

    public SpaceAttribute(string alias, string name, string icon, string description = default, SpaceView view = SpaceView.List)
    {
      Alias = alias;
      Name = name;
      Icon = icon;
      Description = description;
      View = view;
    }
  }
}

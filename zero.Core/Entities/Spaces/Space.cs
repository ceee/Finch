using System;

namespace zero.Core.Entities
{
  public class Space : ISpace
  {
    public string Alias { get; set; }

    public Type Type { get; set; }

    public SpaceView View { get; set; }

    public string ComponentPath { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Icon { get; set; }

    public bool LineBelow { get; set; }

    public bool AllowShared { get; set; }
  }

  public interface ISpace
  {
    string Alias { get; set; }

    string ComponentPath { get; set; }

    string Description { get; set; }

    string Icon { get; set; }

    bool LineBelow { get; set; }

    string Name { get; set; }

    Type Type { get; set; }

    SpaceView View { get; set; }
  
    bool AllowShared { get; set; }
  }
}
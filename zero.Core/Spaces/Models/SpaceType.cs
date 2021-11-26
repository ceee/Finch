namespace zero.Spaces;

public class SpaceType
{
  public string Alias { get; set; }

  public string EditorAlias { get; set; }

  public Type Type { get; set; }

  public SpaceView View { get; set; }

  public string ComponentPath { get; set; }

  public string Name { get; set; }

  public string Description { get; set; }

  public string Icon { get; set; }

  public bool LineBelow { get; set; }
}
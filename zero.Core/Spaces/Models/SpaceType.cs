namespace zero.Spaces;

public class SpaceType : FlavorConfig
{
  public SpaceType(Type type) : base(type) { }

  public string EditorAlias { get; set; }

  public SpaceView View { get; set; }

  public string ComponentPath { get; set; }

  public bool LineBelow { get; set; }
}
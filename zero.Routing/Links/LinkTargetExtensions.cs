namespace zero.Routing;

public static class LinkTargetExtensions
{
  public static string ToTarget(this LinkTarget target)
  {
    return target switch
    {
      LinkTarget.Blank => "_blank",
      _ => "_self"
    };
  }
}

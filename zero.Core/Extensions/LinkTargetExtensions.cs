using zero.Core.Entities;

namespace zero.Core.Extensions
{
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
}

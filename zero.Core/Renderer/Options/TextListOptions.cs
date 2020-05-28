namespace zero.Core.Renderer
{
  public class TextListOptions : TextOptions
  {
    public int Limit { get; set; } = 10;

    public string AddLabel { get; set; } = "@ui.add";
  }
}
namespace zero.Core.Entities
{
  public class PreviewModel
  {
    public string Id { get; set; }

    public string Icon { get; set; }

    public Ref Image { get; set; }

    public string Text { get; set; }

    public string Name { get; set; }

    public bool HasError { get; set; }
  }
}

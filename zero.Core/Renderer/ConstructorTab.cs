namespace zero.Core.Renderer
{
  public class ConstructorTab
  {
    public string Name { get; set; }

    public string Alias { get; set; }

    public ConstructorTab(string alias, string name)
    {
      Alias = alias;
      Name = name;
    }
  }
}

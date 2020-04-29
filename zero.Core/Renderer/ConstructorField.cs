namespace zero.Core.Renderer
{
  public class ConstructorField
  {
    public string Name { get; set; }

    public string Alias { get; set; }

    public ConstructorField(string alias, string name)
    {
      Alias = alias;
      Name = name;
    }
  }
}

namespace zero.Architecture;

public class BlueprintOptions : OptionsEnumerable<Blueprint>, IOptionsEnumerable
{
  public BlueprintOptions()
  {
    Enabled = false;
  }


  public bool Enabled { get; set; }


  public void Add<T>() where T : Blueprint, new()
  {
    Items.Add(new T());
  }


  public void Add<T>(Blueprint<T> implementation) where T : ZeroEntity, new()
  {
    Items.Add(implementation);
  }


  public void Add<T>(Action<Blueprint<T>> createExpression = null) where T : ZeroEntity, new()
  {
    Items.Add(new DefaultBlueprint<T>(createExpression));
  }
}

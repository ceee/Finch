namespace zero.Architecture;

public class BlueprintOptions : List<Blueprint>
{
  public bool Enabled { get; set; } = false;

  public void Add<T>() where T : Blueprint, new()
  {
    Add(new T());
  }

  public void Add<T>(Blueprint<T> implementation) where T : ZeroEntity, new()
  {
    Add(implementation);
  }

  public void Add<T>(Action<Blueprint<T>> createExpression = null) where T : ZeroEntity, new()
  {
    Add(new DefaultBlueprint<T>(createExpression));
  }
}

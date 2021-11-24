namespace zero.Configuration;

public class FeatureOptions : List<Feature>
{
  public void Add<T>() where T : Feature, new()
  {
    Add(new T());
  }

  public void Add(string alias, string name, string description)
  {
    Add(new Feature()
    {
      Alias = alias,
      Name = name,
      Description = description
    });
  }
}
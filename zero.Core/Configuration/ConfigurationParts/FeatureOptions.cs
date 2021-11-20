using zero.Core.Entities;

namespace zero.Configuration;

public class FeatureOptions : OptionsEnumerable<Feature>, IOptionsEnumerable
{
  public void Add<T>() where T : Feature, new()
  {
    Items.Add(new T());
  }

  public void Add(string alias, string name, string description)
  {
    Items.Add(new Feature()
    {
      Alias = alias,
      Name = name,
      Description = description
    });
  }
}

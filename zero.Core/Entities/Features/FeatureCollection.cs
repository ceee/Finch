using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class FeatureCollection : List<IFeature>
  {
    public void Add<T>() where T : IFeature, new()
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
}

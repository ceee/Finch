using zero.Core.Entities;

namespace zero.Core.Options
{
  public class FeatureOptions : ZeroBackofficeCollection<IFeature>, IZeroCollectionOptions
  {
    public FeatureOptions()
    {
      
    }


    public void Add<T>() where T : IFeature, new()
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
}

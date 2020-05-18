using zero.Core.Mapper;

namespace zero.Core.Options
{
  public class MapperOptions : ZeroBackofficeCollection<IMapperConfig>, IZeroCollectionOptions
  {
    public void Add<T>() where T : IMapperConfig, new()
    {
      Items.Add(new T());
    }
  }
}

using Raven.Client.Documents;

namespace zero.Core.Mapper
{
  public interface IMapperConfig
  {
    void Configure(IMapper config);
  }
}

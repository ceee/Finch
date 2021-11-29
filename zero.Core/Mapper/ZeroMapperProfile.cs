namespace zero.Mapper;

public abstract class ZeroMapperProfile : IMapperProfile
{
  /// <inheritdoc />
  public abstract void Configure(IZeroMapper mapper);
}


public interface IMapperProfile
{
  /// <summary>
  /// Configure maps for this profile
  /// </summary>
  void Configure(IZeroMapper mapper);
}
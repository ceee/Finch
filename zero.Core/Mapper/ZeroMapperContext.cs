namespace zero.Mapper;

public class ZeroMapperContext : IZeroMapperContext
{
  /// <inheritdoc />
  public IZeroMapper Mapper { get; }


  public ZeroMapperContext(IZeroMapper mapper)
  {
    Mapper = mapper;
  }
}


public interface IZeroMapperContext
{
  /// <summary>
  /// Access to the runtime mapper
  /// </summary>
  IZeroMapper Mapper { get; }
}
